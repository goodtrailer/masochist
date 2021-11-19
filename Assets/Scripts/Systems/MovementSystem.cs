// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(BeginSimulationEntityCommandBufferSystem))]
public class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        Entities.ForEach((ref Translation t,
            in VelocityData v) =>
        {
            float3 dir = math.all(v.Direction == float3.zero) ? float3.zero : math.normalize(v.Direction);
            t.Value += v.Speed * dir * deltaTime;
        }).ScheduleParallel();
    }
}
