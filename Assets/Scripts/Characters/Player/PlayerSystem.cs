// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

public class PlayerSystem : SystemBase
{
    private BuildPhysicsWorld m_physicsWorld;
    private EndSimulationEntityCommandBufferSystem m_endSimEcbSystem;

    protected override void OnCreate()
    {
        m_physicsWorld = World.GetExistingSystem<BuildPhysicsWorld>();
        m_endSimEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;
        var collisionWorld = m_physicsWorld.PhysicsWorld.CollisionWorld;
        var ecb = m_endSimEcbSystem.CreateCommandBuffer().AsParallelWriter();

        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));
        bool doSprint = Input.GetButton("Sprint");
        bool doFire = Input.GetButtonDown("Fire");  // TODO FIRE RATE

        Entities.ForEach((ref PlayerComponent p, ref AttackPrefabComponent ap, ref Translation t) =>
        {
            // Moving
            float3 ds = new float3(p.normalizeSpeed ? input.normalized : input, 0);
            ds *= doSprint ? p.speedSprint : p.speed;
            ds *= dt;

            float3 targetX = t.Value + ds * Vector3.right;
            float3 targetY = t.Value + ds * Vector3.up;

            bool hitX = collisionWorld.CastRay(new RaycastInput
            {
                Start = t.Value,
                End = targetX,
                Filter = p.collisionFilter,
            }, out var hitInfoX);
            bool hitY = collisionWorld.CastRay(new RaycastInput
            {
                Start = t.Value,
                End = targetY,
                Filter = p.collisionFilter,
            }, out var hitInfoY);

            t.Value.x = hitX ? hitInfoX.Position.x : targetX.x;
            t.Value.y = hitY ? hitInfoY.Position.y : targetY.y;

            // Firing
            if (doFire)
            {
                Entity attack = ecb.Instantiate(0, ap.entity);
                ecb.SetComponent(0, attack, t);
            }
        }).Schedule();

        m_endSimEcbSystem.AddJobHandleForProducer(Dependency);
    }
}
