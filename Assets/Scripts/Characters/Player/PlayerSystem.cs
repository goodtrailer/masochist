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
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;
        var physicsWorld = World.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld;
        var collisionWorld = physicsWorld.CollisionWorld;

        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));
        bool doSprint = Input.GetButton("Sprint");

        Entities.ForEach((ref PlayerComponent c, ref Translation t) =>
        {
            float3 ds = new float3(c.normalizeSpeed ? input.normalized : input, 0);
            ds *= doSprint ? c.speedSprint : c.speed;
            ds *= dt;

            float3 targetX = t.Value + ds * Vector3.right;
            float3 targetY = t.Value + ds * Vector3.up;

            bool hitX = collisionWorld.CastRay(new RaycastInput
            {
                Start = t.Value,
                End = targetX,
                Filter = c.collisionFilter,
            }, out var hitInfoX);
            bool hitY = collisionWorld.CastRay(new RaycastInput
            {
                Start = t.Value,
                End = targetY,
                Filter = c.collisionFilter,
            }, out var hitInfoY);

            t.Value.x = hitX ? hitInfoX.Position.x : targetX.x;
            t.Value.y = hitY ? hitInfoY.Position.y : targetY.y;
        }).Schedule();
    }
}
