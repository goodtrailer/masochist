// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using UnityEngine;

[DisallowMultipleComponent]
public class TimedDestroyDataAuthoring : MonoBehaviour
{
    [Min(0)]
    public double AliveDuration;
    public bool AutoStartTime;
}
