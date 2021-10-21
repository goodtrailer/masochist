// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Transforms;

public class SpawnerSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem endSimEcbSystem;

    protected override void OnCreate()
    {
        endSimEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        EntityQuery enemies = GetEntityQuery(ComponentType.ReadOnly<EnemyTag>());
        if (!enemies.IsEmpty)
            return;

        var ecb = endSimEcbSystem.CreateCommandBuffer().AsParallelWriter();
        BufferFromEntity<Child> childrens = GetBufferFromEntity<Child>();

        double elapsedTime = Time.ElapsedTime;

        Entities.ForEach((Entity e, DynamicBuffer<WavePrefabData> w, ref SpawnerData s) =>
        {
            if (s.Wave >= w.Length)
            {
                DestroyHelper.DestroyHierarchy(e, ecb, childrens);
                return;
            }

            if (elapsedTime < s.NextSpawnTime)
                return;

            if (!s.Resting)
            {
                s.Resting = true;
                s.NextSpawnTime = elapsedTime + s.RestDuration;
                return;
            }

            s.Resting = false;
            ecb.Instantiate(0, w[s.Wave].Entity);
            s.Wave++;
        }).Schedule();
    }
}
