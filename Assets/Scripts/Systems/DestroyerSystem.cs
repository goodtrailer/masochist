// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

public class DestroyerSystem : SystemBase
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;
    private EndSimulationEntityCommandBufferSystem endSimEcbSystem;

    [BurstCompile]
    public struct TriggerEventsJob : ITriggerEventsJobBase
    {
        [ReadOnly]
        public ComponentDataFromEntity<DestroyerTag> Destroyers;
        [ReadOnly]
        public BufferFromEntity<Child> Childrens;

        public EntityCommandBuffer.ParallelWriter Ecb;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity other;
            if (Destroyers.HasComponent(triggerEvent.EntityA))
                other = triggerEvent.EntityB;
            else if (Destroyers.HasComponent(triggerEvent.EntityB))
                other = triggerEvent.EntityA;
            else
                return;

            DestroyHelper.DestroyHierarchy(other, Ecb, Childrens);
        }
    }

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        endSimEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var ecb = endSimEcbSystem.CreateCommandBuffer().AsParallelWriter();
        var destroyers = GetComponentDataFromEntity<DestroyerTag>(true);
        var childrens = GetBufferFromEntity<Child>(true);

        new TriggerEventsJob
        {
            Destroyers = destroyers,
            Ecb = ecb,
            Childrens = childrens,
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency).Complete();
    }
}
