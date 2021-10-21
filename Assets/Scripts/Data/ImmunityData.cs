// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct ImmunityData : IComponentData
{
    public double NormalDuration;

    [HideInInspector]
    public double LastDuration;
    [HideInInspector]
    public double WearOffTime;
}
