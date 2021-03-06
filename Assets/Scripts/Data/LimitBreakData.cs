// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct LimitBreakData : IComponentData
{
    public float DamageMultiplier;
    public float HealthMultiplier;
    public double Cooldown;

    [HideInInspector]
    public bool IsLimitBroken;
    [HideInInspector]
    public double NextUsableTime;
}
