// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;

public class LimitBreakUISystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((LimitBreakUIData lbUI, in LimitBreakData lb) =>
        {
            if (lb.Cooldown == 0)
                return;

            lbUI.SkillSlider.value = 1f + (float)((Time.ElapsedTime - lb.NextUsableTime) / lb.Cooldown);
            lbUI.SkillSlider.UseSecondaryColor = lb.IsLimitBroken;
        }).WithoutBurst().Run();
    }
}
