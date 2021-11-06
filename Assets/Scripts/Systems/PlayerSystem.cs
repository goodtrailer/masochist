// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem endSimEcbSystem;

    protected override void OnCreate()
    {
        endSimEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    [BurstCompile]
    public struct BombJob : IJobParallelFor
    {
        [ReadOnly]
        public float Damage;

        public NativeArray<HealthData> Healths;
        public NativeArray<Entity> Entities;

        public EntityCommandBuffer.ParallelWriter Ecb;

        public void Execute(int index)
        {
            HealthData h = Healths[index];
            h.Health -= Damage;
            Ecb.SetComponent(0, Entities[index], h);
        }
    }

    protected override void OnUpdate()
    {
        var ecbSeq = endSimEcbSystem.CreateCommandBuffer();
        var ecb = ecbSeq.AsParallelWriter();
        var damages = GetComponentDataFromEntity<DamageData>();
        var velocities = GetComponentDataFromEntity<VelocityData>();

        float3 input = new float3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical"),
            z = 0
        };
        bool doAttack = Input.GetButton("Attack");
        bool doAttackDown = Input.GetButtonDown("Attack");

        bool doSprint = Input.GetButtonDown("Sprint");
        bool dontSprint = Input.GetButtonUp("Sprint");

        bool doDodge = Input.GetButtonDown("Dodge");
        bool doBomb = Input.GetButtonDown("Bomb");
        bool doLimitBreak = Input.GetButtonDown("Limit Break");

        float deltaTime = Time.DeltaTime;
        double elapsedTime = Time.ElapsedTime;

        // Walk/Sprint
        Entities.WithAll<PlayerTag>().ForEach((Entity e,
            ref PlayerData p,
            ref StaminaData s) =>
        {
            if (doSprint)
                p.IsSprinting = true;
            if (dontSprint || s.Stamina < 0)
                p.IsSprinting = false;

            if (p.IsSprinting)
                s.Stamina -= p.StaminaDecay * deltaTime;
            else
                s.Stamina = math.min(s.StaminaMax, s.Stamina + p.StaminaRegen * deltaTime);

            VelocityData v = velocities[e];     // what the fuck is this shit, theres no way youre gonna make me use a job for some bs like this
            v.Speed = p.IsSprinting ? p.SpeedSprint : p.SpeedWalk;
            v.Direction = input;
            bool3 dirNan = math.isnan(v.Direction);
            if (dirNan.x || dirNan.y || dirNan.z)
                v.Direction = float3.zero;
            velocities[e] = v;
        }).Schedule();

        // Attack
        Entities.WithAll<PlayerTag>()
            .WithReadOnly(damages).WithReadOnly(velocities)
            .ForEach((ref AttackerData a,
            ref HealthData h,
            ref LevelData l,
            in AttackPrefabData ap,
            in LimitBreakData lb,
            in LimitBreakAttackPrefabData lbap,
            in PlayerData p,
            in Translation t) =>
        {
            bool attacking = p.SemiAutomatic ? doAttackDown : doAttack;
            if (!attacking)
            {
                // Level down
                float oldLevelMultiplier = (l.Level - 1f) * l.LevelCoefficient + 1f;
                l.Level = 1f;
                float newLevelMultiplier = (l.Level - 1f) * l.LevelCoefficient + 1f;

                a.AttackRate *= newLevelMultiplier / oldLevelMultiplier;
                l.NextLevelUpTime = elapsedTime + l.XpCoefficient;

                // Health regen
                h.Health = math.min(h.HealthMax, h.Health + p.HealthRegen * deltaTime);

                return;
            }

            // Level up
            if (l.NextLevelUpTime < elapsedTime && l.Level < l.LevelMax)
            {
                float oldLevelMultiplier = (l.Level - 1f) * l.LevelCoefficient + 1f;
                l.Level++;
                float newLevelMultiplier = (l.Level - 1f) * l.LevelCoefficient + 1f;

                a.AttackRate *= newLevelMultiplier / oldLevelMultiplier;
                if (l.Level < l.LevelMax)
                    l.NextLevelUpTime = elapsedTime + l.XpCoefficient
                        * math.pow(l.Level, l.XpExponent);
            }

            // Health decay
            if (h.Health > h.HealthMax / 2)
                h.Health = math.max(0, h.Health - p.HealthDecay * deltaTime);

            // Spawning attack
            if (a.NextAttackableTime > elapsedTime)
                return;

            a.NextAttackableTime = elapsedTime + 1 / a.AttackRate;

            Entity original = lb.IsLimitBroken ? lbap.Entity : ap.Entity;
            Entity attack = ecb.Instantiate(0, original);
            ecb.SetComponent(0, attack, t);

            DamageData dAttack = damages[original];
            dAttack.Damage *= a.DamageMultiplier;
            ecb.SetComponent(0, attack, dAttack);

            VelocityData vAttack = velocities[original];
            vAttack.Speed *= a.SpeedMultiplier;
            vAttack.Direction = math.up();
            ecb.SetComponent(0, attack, vAttack);
        }).Schedule();

        // Dodge
        if (doDodge)
            Entities.WithAll<PlayerTag>().ForEach((ref DodgeData d,
                ref ImmunityData i) =>
            {
                if (d.NextUsableTime > elapsedTime)
                    return;
                
                i.LastDuration = d.Duration;
                i.WearOffTime = elapsedTime + d.Duration;
                d.NextUsableTime = elapsedTime + d.Cooldown;
            }).Schedule();

        // Bomb
        if (doBomb)
        {
            EntityQuery enemies = GetEntityQuery(typeof(HealthData),
                    ComponentType.ReadOnly<EnemyTag>());
            Entities.WithAll<PlayerTag>().ForEach((ref BombData b,
                in BombPrefabData bp) =>
            {
                if (b.NextUsableTime > elapsedTime)
                    return;

                b.NextUsableTime = elapsedTime + b.Cooldown;

                ecbSeq.Instantiate(bp.Entity);

                var enemyEntities = enemies.ToEntityArray(Allocator.TempJob);
                var enemyHealths = enemies.ToComponentDataArray<HealthData>(Allocator.TempJob);
                new BombJob
                {
                    Damage = b.Damage,
                    Entities = enemyEntities,
                    Healths = enemyHealths,
                    Ecb = ecb,
                }.Schedule(enemyEntities.Length, 50).Complete();
                enemyEntities.Dispose();
                enemyHealths.Dispose();
            }).WithoutBurst().Run();
        }   

        // Limit break
        if (doLimitBreak)
            Entities.WithAll<PlayerTag>().ForEach((ref AttackerData a,
                ref HealthData h,
                ref PlayerData p,
                ref LimitBreakData lb) =>
            {
                if (lb.NextUsableTime > elapsedTime)
                    return;

                lb.NextUsableTime = elapsedTime + lb.Cooldown;
                lb.IsLimitBroken = !lb.IsLimitBroken;
                if (lb.IsLimitBroken)
                {
                    a.DamageMultiplier *= lb.DamageMultiplier;
                    h.Health *= lb.HealthMultiplier;
                    h.HealthMax *= lb.HealthMultiplier;
                }
                else
                {
                    a.DamageMultiplier /= lb.DamageMultiplier;
                    h.Health /= lb.HealthMultiplier;
                    h.HealthMax /= lb.HealthMultiplier;
                }
            }).Schedule();
    }
}
