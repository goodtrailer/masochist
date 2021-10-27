// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using System.Collections.Generic;
using UnityEngine;

public class StagesDataAuthoring : MonoBehaviour
{
    [System.Serializable]
    public class Stage
    {
        public float HealthMin;
        public bool Reverses;
        public List<Vector2> Destinations;
    }

    public List<Stage> Stages;
}
