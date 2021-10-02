// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

[GenerateAuthoringComponent]
public struct AttackerData : IComponentData
{
    public float AttackSpeed;
    public float AttackRate;
    public float DamageMultiplier;
    public float LastAttackTime;
}
