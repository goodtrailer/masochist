// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Physics;

public struct BulletComponent : IComponentData
{
    public float damage;
    public float speed;

    public CollisionFilter collisionFilter;
}

public class BulletConversionSystem : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((BulletAuthoring b) =>
        {
            Entity e = GetPrimaryEntity(b);

            DstEntityManager.AddComponentData(e, new BulletComponent
            {
                damage = b.damage,
                speed = b.speed,
                collisionFilter = b.collisionFilter,
            });
        });
    }
}
