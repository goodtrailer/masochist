// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class TimeUISystem : SystemBase
{
    protected override void OnUpdate()
    {
        double elapsedTime = Time.ElapsedTime;
        RunData r = GetSingleton<RunData>();
        Entities.ForEach((TimeUIData tUI) =>
        {
            Debug.Log("bruh");
            int time = (int)(elapsedTime - r.StartTime);
            tUI.TimeText.text = string.Format("{0:D2}:{1:D2}", time / 60, time % 60);
        }).WithoutBurst().Run();
    }
}
