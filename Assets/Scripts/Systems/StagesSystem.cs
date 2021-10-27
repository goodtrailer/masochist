// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class StagesSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((DynamicBuffer<DestinationData> bd,
            DynamicBuffer<StageData> bs,
            ref StagesData s,
            ref VelocityData v,
            in HealthData h,
            in Translation t) =>
        {
            float greatestHealthMin = float.MinValue ;
            for (int i = 0; i < bs.Length; i++)
                if (bs[i].HealthMin > greatestHealthMin && h.Health >= bs[i].HealthMin)
                {
                    greatestHealthMin = bs[i].HealthMin;
                    s.StageIndex = i;
                }

            float distance = math.distance(t.Value, bd[bs[s.StageIndex].DestinationIndex0 + s.DestinationIndex].Destination);
            float destinationEpsilon = math.abs(v.Speed) * 0.05f;
            if (distance < destinationEpsilon)
            {
                int index1 = s.StageIndex < bs.Length - 1 ? bs[s.StageIndex + 1].DestinationIndex0 : bd.Length;
                int destinationsCount = index1 - bs[s.StageIndex].DestinationIndex0;

                s.DestinationIndex += s.DeltaDestinationIndex;

                if (s.DestinationIndex >= destinationsCount)
                {
                    if (bs[s.StageIndex].Reverses)
                    {
                        s.DestinationIndex = destinationsCount - 1;
                        s.DeltaDestinationIndex = -1;
                    }
                    else
                        s.DestinationIndex = 0;
                }
                
                if (s.DestinationIndex == 0)
                    s.DeltaDestinationIndex = 1;

                v.Direction = float3.zero;
            }
            else
                v.Direction = bd[bs[s.StageIndex].DestinationIndex0 + s.DestinationIndex].Destination - t.Value;
        }).WithoutBurst().Schedule();
    }
}
