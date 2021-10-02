// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using UnityEngine;

public class PlayerDataAuthoring : MonoBehaviour
{
    [Header("Health")]
    [Min(0)]
    public float HealthRegen;
    [Min(0)]
    public float HealthDecay;

    [Header("Stamina")]
    [Min(0)]
    public float StaminaRegen;
    [Min(0)]
    public float StaminaDecay;

    [Header("Misc")]
    [Min(0)]
    public float SpeedWalk;
    [Min(0)]
    public float SpeedSprint;
    [Min(0)]
    public float LevelUpRatio;
}
