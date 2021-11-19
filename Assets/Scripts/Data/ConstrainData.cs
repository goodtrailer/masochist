// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ConstrainData : IComponentData
{
    public float3 Min;
    public float3 Max;
}
