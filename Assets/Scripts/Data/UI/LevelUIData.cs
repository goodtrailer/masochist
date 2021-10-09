// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using System;
using Unity.Entities;
using TMPro;
using UnityEngine.UI;

[GenerateAuthoringComponent]
public class LevelUIData : IComponentData
{
    public Slider XpSlider;
    public TMP_Text NumberText;
}
