// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class ImmunitySliderSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ImmunitySliderData @is, in ImmunityData i) =>
        {
            if (i.LastDuration == 0f)
                @is.Slider.value = 0f;
            else
                @is.Slider.value = (float)((i.WearOffTime - Time.ElapsedTime) / i.LastDuration);
        }).WithoutBurst().Run();
    }
}
