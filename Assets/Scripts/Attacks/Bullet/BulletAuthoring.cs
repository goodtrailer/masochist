// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Physics.Authoring;
using UnityEngine;

[AddComponentMenu("masochist/Bullet"),
    RequireComponent(typeof(PhysicsShapeAuthoring)),
    RequireComponent(typeof(PhysicsBodyAuthoring))]
public class BulletAuthoring : MonoBehaviour
{
    [Header("Stats")]
    [Min(0)]
    public float damage;
    [Min(0)]
    public float speed;

    [Header("Misc")]
    public SerializableCollisionFilter collisionFilter;
}
