// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;

public class HealthSliderSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((HealthSliderData hs, in HealthData h) =>
        {
            hs.Slider.value = h.Health / h.HealthMax;
        }).WithoutBurst().Run();
    }
}
