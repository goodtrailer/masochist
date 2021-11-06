﻿// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using TMPro;
using Unity.Entities;

[GenerateAuthoringComponent]
public class TimeUIData : IComponentData
{
    public TMP_Text TimeText;
}
