// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;

public class LimitBreakSliderSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((LimitBreakSliderData lbs, in LimitBreakData lb) =>
        {
            if (lb.Cooldown == 0)
                return;

            lbs.Slider.value = 1f + (float)((Time.ElapsedTime - lb.NextUsableTime) / lb.Cooldown);
            lbs.Slider.UseSecondaryColor = lb.IsLimitBroken;
        }).WithoutBurst().Run();
    }
}
