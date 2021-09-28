// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;

[System.Serializable]
public struct SerializableCollisionFilter
{
    public PhysicsCategoryTags belongsTo;
    public PhysicsCategoryTags collidesWith;
    public int groupIndex;

    public static implicit operator CollisionFilter(SerializableCollisionFilter scf)
    {
        return new CollisionFilter
        {
            BelongsTo = scf.belongsTo.Value,
            CollidesWith = scf.collidesWith.Value,
            GroupIndex = scf.groupIndex,
        };
    }

    public static implicit operator SerializableCollisionFilter(CollisionFilter cf)
    {
        var scf = new SerializableCollisionFilter
        {
            groupIndex = cf.GroupIndex,
        };
        scf.belongsTo.Value = cf.BelongsTo;
        scf.collidesWith.Value = cf.CollidesWith;
        return scf;
    }
}
