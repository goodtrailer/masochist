// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

[GenerateAuthoringComponent]
public struct ImmunityData : IComponentData
{
    public double NormalDuration;
    public double LastDuration;
    public double WearOffTime;
}
