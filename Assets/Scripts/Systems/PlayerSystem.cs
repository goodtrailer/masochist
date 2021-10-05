// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
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
        bool doDodge = Input.GetButtonDown("Dodge");
        bool doSprint = Input.GetButtonDown("Sprint");
        bool dontSprint = Input.GetButtonUp("Sprint");

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
        Entities.WithReadOnly(damages).WithReadOnly(velocities)
            .WithAll<PlayerTag>().ForEach((ref AttackerData a,
            in AttackPrefabComponent ap,
            in PlayerData pd,
            in Translation t) =>
        {
            bool attacking = pd.SemiAutomatic ? doAttackDown : doAttack;
            if (attacking && elapsedTime > a.LastAttackTime + 1 / a.AttackRate)
            {
                a.LastAttackTime = elapsedTime;

                Entity attack = ecb.Instantiate(0, ap.entity);
                ecb.SetComponent(0, attack, t);

                DamageData dAttack = damages[ap.entity];
                dAttack.Damage *= a.DamageMultiplier;
                ecb.SetComponent(0, attack, dAttack);

                VelocityData vAttack = velocities[ap.entity];
                vAttack.Speed *= a.SpeedMultiplier;
                vAttack.Direction = math.up();
                ecb.SetComponent(0, attack, vAttack);
            }
        }).Schedule();

        // Dodging
        Entities.WithAll<PlayerTag>().ForEach((ref DodgeData d,
            ref ImmunityData i) =>
        {
            // Dodging
            if (doDodge && d.NextUsableTime < elapsedTime)
            {
                i.LastDuration = d.Duration;
                i.WearOffTime = elapsedTime + d.Duration;
                d.NextUsableTime = elapsedTime + d.Cooldown;
            }
        }).Schedule();
    }
}
