// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

public struct ScaleAnimData : IAnimData, IComponentData
{
    public bool AutoStartTime { get; set; }
    public double StartTime { get; set; }
    public double Duration { get; set; }
    public float StartValue { get; set; }
    public float EndValue { get; set; }
    public Easing Easing { get; set; }
}

public class ScaleAnimConversionSystem : AnimConversionSystem<ScaleAnimDataAuthoring, ScaleAnimData> { }
