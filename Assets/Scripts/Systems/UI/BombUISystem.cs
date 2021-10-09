// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;

public class BombUISystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((BombUIData bUI, in BombData b) =>
        {
            if (b.Cooldown == 0)
                return;

            bUI.SkillSlider.value = 1f + (float)((Time.ElapsedTime - b.NextUsableTime) / b.Cooldown);
        }).WithoutBurst().Run();
    }
}
