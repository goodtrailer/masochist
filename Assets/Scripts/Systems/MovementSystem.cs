// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
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
            pv.Linear = v.Speed * v.Direction;
        }).WithBurst().Schedule();
    }
}
