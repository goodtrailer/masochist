// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Transforms;

public class TimedDestroySystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem endSimEcbSystem;

    protected override void OnCreate()
    {
        endSimEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var ecb = endSimEcbSystem.CreateCommandBuffer().AsParallelWriter();
        var childrens = GetBufferFromEntity<Child>(true);

        double elapsedTime = Time.ElapsedTime;

        Entities.WithReadOnly(childrens)
            .ForEach((Entity e, ref TimedDestroyData td) =>
        {
            if (td.AutoStartTime)
            {
                td.AutoStartTime = false;
                td.StartTime = elapsedTime;
            }

            if (td.StartTime + td.AliveDuration > elapsedTime)
                return;

            DestroyHelper.DestroyHierarchy(e, ecb, childrens);
        }).Schedule();

        endSimEcbSystem.AddJobHandleForProducer(Dependency);
    }
}
