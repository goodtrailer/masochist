// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

[CustomEditor(typeof(SkillSlider))]
public class SkillSliderEditor : SliderEditor
{
    public override void OnInspectorGUI()
    {
        SkillSlider ss = (SkillSlider)target;

        EditorGUILayout.LabelField("Skill Slider Fields", EditorStyles.boldLabel);

        EditorGUI.indentLevel++;
        ss.ForegroundGraphic = EditorGUILayout.ObjectField("Foreground Graphic", ss.ForegroundGraphic, typeof(Image), true) as Image;
        ss.ForegroundPrimary = EditorGUILayout.ColorField("Foreground Primary", ss.ForegroundPrimary);
        ss.ForegroundSecondary = EditorGUILayout.ColorField("Foreground Secondary", ss.ForegroundSecondary);
        ss.CooldownReady = EditorGUILayout.ColorField("Cooldown Ready", ss.CooldownReady);
        ss.CooldownUnready = EditorGUILayout.ColorField("Cooldown Unready", ss.CooldownUnready);
        EditorGUI.indentLevel--;

        EditorGUILayout.Space(4f);

        EditorGUILayout.LabelField("Slider Fields", EditorStyles.boldLabel);
        base.OnInspectorGUI();
    }
}
