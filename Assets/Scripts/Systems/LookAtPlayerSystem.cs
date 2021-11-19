// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class LookAtPlayerSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem endSimEcbSystem;

    protected override void OnCreate()
    {
        endSimEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        EntityQuery players = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>(),
            ComponentType.ReadOnly<Translation>());

        var playerTranslations = players.ToComponentDataArray<Translation>(Allocator.Temp);
        float3 playerTranslation = playerTranslations.Length > 0 ? playerTranslations[0].Value : float3.zero;

        Entities.WithAll<LookAtPlayerTag>().ForEach((ref Rotation r,
            in Translation t) =>
        {
            float3 dir = math.normalize(playerTranslation - t.Value);
            r.Value = quaternion.AxisAngle(math.forward(),  math.acos(math.dot(math.up(), dir)) * (dir.x > 0 ? -1f : 1f));
        }).ScheduleParallel();
    }
}
