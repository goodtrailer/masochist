// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

public struct StagesData : IComponentData
{
    public int DestinationIndex;
    public int DeltaDestinationIndex;
    public int StageIndex;
}

public struct StageData : IBufferElementData
{
    public float HealthMin;
    public bool Reverses;
    public int DestinationIndex0;
}


public class StageConversionSystem : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((StagesDataAuthoring a) =>
        {
            Entity e = GetPrimaryEntity(a);
            DynamicBuffer<StageData> stages = DstEntityManager.AddBuffer<StageData>(e);
            int index0 = 0;
            foreach (var stage in a.Stages)
            {
                stages.Add(new StageData
                {
                    HealthMin = stage.HealthMin,
                    Reverses = stage.Reverses,
                    DestinationIndex0 = index0,
                });
                index0 += stage.Destinations.Count;
            }
            DstEntityManager.AddComponentData(e, new StagesData
            {
                DestinationIndex = 0,
                DeltaDestinationIndex = 1,
                StageIndex = 0,
            });
        });
    }
}

