// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    [Header("Stats")]
    [Min(0)]
    public float health;
    [Min(0)]
    public float stamina;
    [Min(0)]
    public float damage;
    [Min(0)]
    public float speed;
    [Min(0)]
    public float speedSprint;

    [Header("Skills")]
    public bool normalizeSpeed;

    [Header("Collision")]
    public PhysicsCategoryTags belongsTo;
    public PhysicsCategoryTags collidesWith;
}
