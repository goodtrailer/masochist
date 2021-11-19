// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

public class DamageSystem : SystemBase
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;
    private EndSimulationEntityCommandBufferSystem endSimEcbSystem;

    [BurstCompile]
    public struct TriggerEventsJob : ITriggerEventsJobBase
    {
        [ReadOnly]
        public BufferFromEntity<Child> Childrens;
        [ReadOnly]
        public ComponentDataFromEntity<DamageData> Damages;
        [ReadOnly]
        public double ElapsedTime;

        public ComponentDataFromEntity<HealthData> Healths;
        public ComponentDataFromEntity<ImmunityData> Immunities;
        public EntityCommandBuffer.ParallelWriter Ecb;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity damager;
            Entity victim;

            if (Damages.HasComponent(triggerEvent.EntityA)
                && Healths.HasComponent(triggerEvent.EntityB))
            {
                damager = triggerEvent.EntityA;
                victim = triggerEvent.EntityB;
            }
            else if (Damages.HasComponent(triggerEvent.EntityB)
                && Healths.HasComponent(triggerEvent.EntityA))
            {
                damager = triggerEvent.EntityB;
                victim = triggerEvent.EntityA;
            }
            else
                return;

            Ecb.DestroyEntity(0, damager);

            ImmunityData i;
            if (Immunities.HasComponent(victim))
            {
                i = Immunities[victim];
                if (i.WearOffTime > ElapsedTime)
                    return;

                i.WearOffTime = ElapsedTime + i.NormalDuration;
                i.LastDuration = i.NormalDuration;
                Immunities[victim] = i;
            }

            HealthData h = Healths[victim];
            h.Health -= Damages[damager].Damage;
            Healths[victim] = h;
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

        new TriggerEventsJob
        {
            Childrens = GetBufferFromEntity<Child>(true),
            Damages = GetComponentDataFromEntity<DamageData>(true),
            ElapsedTime = Time.ElapsedTime,

            Healths = GetComponentDataFromEntity<HealthData>(false),
            Immunities = GetComponentDataFromEntity<ImmunityData>(false),
            Ecb = ecb,
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency).Complete();
    }
}
