// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
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

        float3 input = new float3(Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"), 0);
        bool doSprint = Input.GetButtonDown("Sprint");
        bool dontSprint = Input.GetButtonUp("Sprint");
        bool doAttack = Input.GetButton("Attack");

        float time = UnityEngine.Time.time;
        float dt = Time.DeltaTime;
        Entities.ForEach((ref AttackerData a,
            ref PlayerData p,
            ref StaminaData s,
            ref VelocityData v,
            in AttackPrefabComponent ap,
            in Translation t) =>
        {
            // Moving
            if (doSprint)
                p.IsSprinting = true;
            if (dontSprint || s.Stamina < 0)
                p.IsSprinting = false;

            if (p.IsSprinting)
                s.Stamina -= p.StaminaDecay * dt;
            else
                s.Stamina = math.min(s.StaminaMax, s.Stamina + p.StaminaRegen * dt);

            v.Speed = p.IsSprinting ? p.SpeedSprint : p.SpeedWalk;
            v.Direction = math.normalize(input);
            bool3 dirNan = math.isnan(v.Direction);
            if (dirNan.x || dirNan.y || dirNan.z)
                v.Direction = float3.zero;

            // Attacking
            if (doAttack && time > a.LastAttackTime + 1 / a.AttackRate)
            {
                a.LastAttackTime = time;

                Entity attack = ecb.Instantiate(0, ap.entity);
                ecb.SetComponent(0, attack, t);
                ecb.SetComponent(0, attack, new VelocityData
                {
                    Speed = a.AttackSpeed,
                    Direction = math.up(),
                });
            }
        }).Schedule();

        endSimEcbSystem.AddJobHandleForProducer(Dependency);
    }
}
