// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class GoonSystem : SystemBase
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

        double elapsedTime = Time.ElapsedTime;

        Entities.WithAll<GoonTag>()
            .WithReadOnly(damages).WithReadOnly(velocities)
            .ForEach((ref AttackerData a,
            in AttackPrefabData ap,
            in Rotation r,
            in Translation t) =>
        {
            if (a.NextAttackableTime > elapsedTime || a.AttackRate == 0f)
                return;

            a.NextAttackableTime = elapsedTime + 1 / a.AttackRate;

            Entity attack = ecb.Instantiate(0, ap.Entity);
            ecb.SetComponent(0, attack, t);
            ecb.SetComponent(0, attack, r);

            DamageData dAttack = damages[ap.Entity];
            dAttack.Damage *= a.DamageMultiplier;
            ecb.SetComponent(0, attack, dAttack);

            VelocityData vAttack = velocities[ap.Entity];
            vAttack.Speed *= a.SpeedMultiplier;
            vAttack.Direction = math.rotate(r.Value, math.up());
            ecb.SetComponent(0, attack, vAttack);
        }).ScheduleParallel();

        endSimEcbSystem.AddJobHandleForProducer(Dependency);
    }
}
