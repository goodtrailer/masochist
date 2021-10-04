// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class HealthSystem : SystemBase
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

        Entities.ForEach((Entity e, in HealthData h) =>
        {
            if (h.Health <= 0)
                DestroyHelper.DestroyHierarchy(e, ecb, childrens);
        }).Schedule();
    }
}
