// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct DestinationData : IBufferElementData
{
    public float3 Destination;
}

public class DestinationConversionSystem : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((StagesDataAuthoring a) =>
        {
            Entity e = GetPrimaryEntity(a);
            DynamicBuffer<DestinationData> destinations = DstEntityManager.AddBuffer<DestinationData>(e);
            foreach (var stage in a.Stages)
                foreach (var destination in stage.Destinations)
                    destinations.Add(new DestinationData
                    {
                        Destination = (Vector3)destination,
                    });
        });
    }
}
