﻿// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;

public class StaminaSliderSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((StaminaSliderData ss, in StaminaData s) =>
        {
            ss.Slider.value = s.Stamina / s.StaminaMax;
        }).WithoutBurst().Run();
    }
}
