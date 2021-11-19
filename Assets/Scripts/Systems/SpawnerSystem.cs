// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Collections;
using Unity.Entities;
using Unity.Scenes;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnerSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem endSimEcbSystem;
    private SceneSystem sceneSystem;

    protected override void OnCreate()
    {
        endSimEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        sceneSystem = World.GetOrCreateSystem<SceneSystem>();
    }

    protected override void OnUpdate()
    {
        EntityQuery enemies = GetEntityQuery(ComponentType.ReadOnly<EnemyTag>());
        if (!enemies.IsEmpty)
            return;

        var ecb = endSimEcbSystem.CreateCommandBuffer();

        BufferFromEntity<Child> childrens = GetBufferFromEntity<Child>();
        
        double elapsedTime = Time.ElapsedTime;

        using (var toLoad = new NativeList<WaveSceneData>(Allocator.Temp))
        {
            Entities.ForEach((Entity e,
                DynamicBuffer<WaveSceneData> bw,
                ref SpawnerData s) =>
            {
                if (s.Wave >= bw.Length)
                {
                    ComponentDataFromEntity<TimedDestroyData> timedDestroys = GetComponentDataFromEntity<TimedDestroyData>();
                    if (!timedDestroys.HasComponent(e))
                        ecb.AddComponent(e, new TimedDestroyData
                        {
                            StartTime = elapsedTime,
                            AliveDuration = 1.0,
                        });
                    return;
                }
                
                if (elapsedTime < s.NextSpawnTime)
                    return;

                s.NextSpawnTime = elapsedTime + s.RestDuration;

                if (!s.Resting)
                {
                    s.Resting = true;
                    return;
                }

                s.Resting = false;
                toLoad.Add(bw[s.Wave]);
                s.Wave++;
            }).WithoutBurst().Run();

            foreach (var ws in toLoad)
                sceneSystem.LoadSceneAsync(ws.GUID);
        }
    }
}
