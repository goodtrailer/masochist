// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using UnityEngine;
using UnityEngine.UI;

public class SkillSlider : Slider
{
    public Image ForegroundGraphic;
    public Color ForegroundPrimary;
    public Color ForegroundSecondary;
    public Color CooldownReady;
    public Color CooldownUnready;

    private bool useSecondaryColor;

    public bool UseSecondaryColor
    {
        get => useSecondaryColor;
        set
        {
            useSecondaryColor = value;
            ForegroundGraphic.color = value ? ForegroundSecondary : ForegroundPrimary;
        }
    }


    protected override void Start()
    {
        base.Start();

        if (!Application.isPlaying)
            ForegroundGraphic = transform.Find("Foreground").GetComponent<Image>();

        ForegroundGraphic.color = useSecondaryColor ? ForegroundSecondary : ForegroundPrimary;

        onValueChanged.AddListener(delegate {
            targetGraphic.color = value < 1 ? CooldownUnready : CooldownReady;
        });
    }
}
