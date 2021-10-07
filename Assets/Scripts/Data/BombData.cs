// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

[GenerateAuthoringComponent]
public struct BombData : IComponentData
{
    public float Damage;
    public double Cooldown;

    public double NextUsableTime;
}
