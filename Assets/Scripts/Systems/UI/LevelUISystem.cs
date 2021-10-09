// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Mathematics;

public class LevelUISystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((LevelUIData lUI, in LevelData l) =>
        {
            double duration = l.LevelUpCoefficient * math.pow(l.Level, l.LevelUpExponent);
            double lastLevelUp = l.NextLevelUpTime - duration;
            lUI.XpSlider.value = (float)((Time.ElapsedTime - lastLevelUp) / duration);
            lUI.NumberText.text = math.round(l.Level).ToString("#");
        }).WithoutBurst().Run();
    }
}
