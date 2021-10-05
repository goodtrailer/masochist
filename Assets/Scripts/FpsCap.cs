// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using UnityEngine;

public class FpsCap : MonoBehaviour
{
    [Min(0)]
    public int targetFps;
    [Min(0)]
    public int vSyncCount;

    private void OnValidate()
    {
        Application.targetFrameRate = targetFps;
        QualitySettings.vSyncCount = vSyncCount;
    }
}
