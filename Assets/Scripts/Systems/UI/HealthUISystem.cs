// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;

public class HealthUISystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((HealthUIData hUI, in HealthData h) =>
        {
            hUI.Slider.value = h.Health / h.HealthMax;
        }).WithoutBurst().Run();
    }
}
