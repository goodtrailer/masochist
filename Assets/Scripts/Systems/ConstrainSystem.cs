// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(MovementSystem))]
public class ConstrainSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation t,
            in ConstrainData c) =>
        {
            t.Value = math.clamp(t.Value, c.Min, c.Max);
        }).ScheduleParallel();
    }
}
