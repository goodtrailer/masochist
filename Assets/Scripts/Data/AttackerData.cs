// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct AttackerData : IComponentData
{
    public float AttackRate;
    public float DamageMultiplier;
    public float SpeedMultiplier;

    [HideInInspector]
    public double NextAttackableTime;
}
