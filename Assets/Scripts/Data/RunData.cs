// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

public struct RunData : IComponentData
{
    public double StartTime;
    public double EndTime;
    public bool InProgress;
}
