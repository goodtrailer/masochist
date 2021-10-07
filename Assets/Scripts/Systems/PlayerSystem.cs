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
        var ecb = endSimEcbSystem.CreateCommandBuffer().AsParallelWriter();
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

        // Moving
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
            v.Direction = math.normalize(input);
            bool3 dirNan = math.isnan(v.Direction);
            if (dirNan.x || dirNan.y || dirNan.z)
                v.Direction = float3.zero;
            velocities[e] = v;
        }).Schedule();

        // Attacking
        if (doAttack)
            Entities.WithAll<PlayerTag>()
                .WithReadOnly(damages).WithReadOnly(velocities)
                .ForEach((ref AttackerData a,
                in AttackPrefabComponent ap,
                in PlayerData pd,
                in Translation t) =>
            {
                bool attacking = pd.SemiAutomatic ? doAttackDown : true;
                if (!attacking || a.NextAttackableTime > elapsedTime)
                    return;

                a.NextAttackableTime = elapsedTime + 1 / a.AttackRate;

                Entity attack = ecb.Instantiate(0, ap.entity);
                ecb.SetComponent(0, attack, t);

                DamageData dAttack = damages[ap.entity];
                dAttack.Damage *= a.DamageMultiplier;
                ecb.SetComponent(0, attack, dAttack);

                VelocityData vAttack = velocities[ap.entity];
                vAttack.Speed *= a.SpeedMultiplier;
                vAttack.Direction = math.up();
                ecb.SetComponent(0, attack, vAttack);
            }).Schedule();

        // Dodging
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

        // Bombing
        if (doBomb)
            Entities.WithAll<PlayerTag>().ForEach((ref BombData b) =>
            {
                if (b.NextUsableTime > elapsedTime)
                    return;

                b.NextUsableTime = elapsedTime + b.Cooldown;

                EntityQuery enemies = GetEntityQuery(typeof(HealthData),
                    ComponentType.ReadOnly<EnemyTag>());
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

        // Limit breaking
        if (doLimitBreak)
            Entities.WithAll<PlayerTag>().ForEach((ref AttackerData a,
                ref LimitBreakData lb) =>
            {
                if (lb.NextUsableTime > elapsedTime)
                    return;

                lb.NextUsableTime = elapsedTime + lb.Cooldown;
                lb.IsLimitBroken = !lb.IsLimitBroken;
                if (lb.IsLimitBroken)
                    a.DamageMultiplier *= lb.DamageMultiplier;
                else
                    a.DamageMultiplier /= lb.DamageMultiplier;
            }).Schedule();
    }
}
