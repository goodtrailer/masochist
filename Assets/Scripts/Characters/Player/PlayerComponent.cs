// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Physics;

public struct PlayerComponent : IComponentData
{
    public float health;
    public float healthMax;

    public float stamina;
    public float staminaMax;

    public float damage;

    public float speed;
    public float speedSprint;

    public bool normalizeSpeed;
    public float accuracy;

    public CollisionFilter collisionFilter;
}

public class PlayerConversionSystem : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((PlayerAuthoring p) =>
        {
            Entity e = GetPrimaryEntity(p);

            DstEntityManager.AddComponentData(e, new PlayerComponent
            {
                health = p.health,
                healthMax = p.health,
                stamina = p.stamina,
                staminaMax = p.stamina,
                damage = p.damage,
                speed = p.speed,
                speedSprint = p.speedSprint,

                normalizeSpeed = p.normalizeSpeed,
                accuracy = p.accuracy,

                collisionFilter = p.collisionFilter,
            });
        });
    }
}
