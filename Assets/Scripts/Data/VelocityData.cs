// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct VelocityData : IComponentData
{
    public float Speed;
    public float3 Direction;
}
