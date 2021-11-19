// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class ScaleAnimSystem : SystemBase
{
    private const float EPSILON = 1e-12F;
    protected override void OnUpdate()
    {
        double elapsedTime = Time.ElapsedTime;
        Entities.ForEach((ref LocalToWorld ltw, ref ScaleAnimData sa) =>
        {
            if (sa.AutoStartTime)
            {
                sa.AutoStartTime = false;
                sa.StartTime = elapsedTime;
            }

            double endTime = sa.StartTime + sa.Duration;
            if (sa.StartTime > elapsedTime || endTime < elapsedTime)
                return;

            float scale = (float)EasingHelper.EaseValue(sa.Easing, elapsedTime,
                        sa.StartTime, endTime, sa.StartValue, sa.EndValue);
            if (scale < EPSILON && scale > 0)
                scale = EPSILON;
            else if (scale > -EPSILON && scale < 0)
                scale = -EPSILON;

            ltw.Value.c0.x = ltw.Value.c1.y = ltw.Value.c2.z = scale;

        }).ScheduleParallel();
    }
}
