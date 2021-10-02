// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using System.ComponentModel;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateBefore(typeof(EndSimulationEntityCommandBufferSystem))]
public class DestructionSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem endSimEcbSystem;
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    [BurstCompile]
    public struct TriggerEventsJob : ITriggerEventsJob
    {
        [ReadOnly(true)]
        public ComponentDataFromEntity<DestroyerTag> Destroyers;

        public EntityCommandBuffer Ecb;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity other;
            if (Destroyers.HasComponent(triggerEvent.EntityA))
                other = triggerEvent.EntityB;
            else if (Destroyers.HasComponent(triggerEvent.EntityB))
                other = triggerEvent.EntityA;
            else
                return;

            Ecb.DestroyEntity(other);
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
        new TriggerEventsJob
        {
            Destroyers = GetComponentDataFromEntity<DestroyerTag>(),
            Ecb = endSimEcbSystem.CreateCommandBuffer(),
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency).Complete();
    }
}
