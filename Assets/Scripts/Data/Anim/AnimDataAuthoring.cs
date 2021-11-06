// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using UnityEngine;

public abstract class AnimDataAuthoring : MonoBehaviour
{
    public float StartValue;
    public float EndValue;

    [Min(0)]
    public double Duration;
    public Easing Easing;
    public bool AutoStartTime;
}
