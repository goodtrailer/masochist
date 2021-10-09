// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class ImmunityUISystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ImmunityUIData iUI, in ImmunityData i) =>
        {
            if (i.LastDuration == 0f)
                iUI.Slider.value = 0f;
            else
                iUI.Slider.value = (float)((i.WearOffTime - Time.ElapsedTime) / i.LastDuration);
        }).WithoutBurst().Run();
    }
}
