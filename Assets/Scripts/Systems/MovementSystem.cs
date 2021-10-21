// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine.Jobs;

[UpdateAfter(typeof(BeginSimulationEntityCommandBufferSystem))]
public class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {

        Entities.ForEach((Entity e,
            ref PhysicsVelocity pv,
            in VelocityData v) =>
        {
            float3 dir = math.all(v.Direction == float3.zero) ? float3.zero : math.normalize(v.Direction);
            pv.Linear = v.Speed * dir;
        }).WithBurst().Schedule();
    }
}
