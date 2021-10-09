// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class LevelDataAuthoring : MonoBehaviour
{
    [Min(1)]
    public float LevelMax;
    [Min(0)]
    public float LevelUpCoefficient;
    [Min(0)]
    public float LevelUpExponent;
}
