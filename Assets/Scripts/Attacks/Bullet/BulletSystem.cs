// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Transforms;

public class BulletSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;
        Entities.ForEach((ref BulletComponent b,
            ref Translation t,
            ref LocalToWorld ltw) =>
        {
            t.Value += ltw.Up * b.speed * dt;
        }).ScheduleParallel();
    }
}
