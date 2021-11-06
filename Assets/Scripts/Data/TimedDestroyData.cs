// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

public struct TimedDestroyData : IComponentData
{
    public double AliveDuration;
    public double StartTime;
    public bool AutoStartTime;
}

public class TimedDestroyConversionSystem : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((TimedDestroyDataAuthoring a) =>
        {
            Entity e = GetPrimaryEntity(a);

            DstEntityManager.AddComponentData(e, new TimedDestroyData
            {
                AliveDuration = a.AliveDuration,
                StartTime = double.MaxValue - a.AliveDuration,
                AutoStartTime = a.AutoStartTime,
            });
        });
    }
}
