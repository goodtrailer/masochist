// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

public class DamagerSystem : SystemBase
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;
    private int trial = 0;

    [BurstCompile]
    public struct TriggerEventsJob : ITriggerEventsJobBase
    {
        [ReadOnly]
        public ComponentDataFromEntity<DamagerTag> Damagers;
        [ReadOnly]
        public ComponentDataFromEntity<HealthData> Healths;
        [ReadOnly]
        public int Trial;

        public void Execute(TriggerEvent triggerEvent)
        {
            /*if (Damagers.HasComponent(triggerEvent.EntityA))
                Debug.Log(string.Format("{0:G} A is damaager", Trial));
            if (Damagers.HasComponent(triggerEvent.EntityB))
                Debug.Log(string.Format("{0:G} B is damager", Trial));*/
        }
    }

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        new TriggerEventsJob
        {
            Damagers = GetComponentDataFromEntity<DamagerTag>(true),
            Healths = GetComponentDataFromEntity<HealthData>(true),
            Trial = trial++,
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency).Complete();
    }
}
