// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

public interface IAnimData
{
    public bool AutoStartTime { get; set; }
    public double StartTime { get; set; }
    public double Duration { get; set; }
    public float StartValue { get; set; }
    public float EndValue { get; set; }
    public Easing Easing { get; set; }
}

public abstract class AnimConversionSystem<A, C> : GameObjectConversionSystem
    where A : AnimDataAuthoring
    where C : struct, IAnimData, IComponentData
{
    protected override void OnUpdate()
    {
        Entities.ForEach((A a) =>
        {
            Entity e = GetPrimaryEntity(a);

            DstEntityManager.AddComponentData(e, new C
            {
                AutoStartTime = a.AutoStartTime,
                StartTime = double.MaxValue,
                Duration = a.Duration,
                StartValue = a.StartValue,
                EndValue = a.EndValue,
                Easing = a.Easing,
            });
        });
    }
}
