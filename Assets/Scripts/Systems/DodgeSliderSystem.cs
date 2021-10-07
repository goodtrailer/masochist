// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;

public class DodgeSliderSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((DodgeSliderData ds, in DodgeData d) =>
        {
            if (d.Cooldown == 0)
                return;

            ds.Slider.value = 1f + (float)((Time.ElapsedTime - d.NextUsableTime) / d.Cooldown);
        }).WithoutBurst().Run();
    }
}
