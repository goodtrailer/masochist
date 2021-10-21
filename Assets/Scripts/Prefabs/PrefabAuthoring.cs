// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using System.Collections.Generic;
using UnityEngine;

public abstract class PrefabAuthoring : MonoBehaviour
{
    public GameObject Prefab;
}

public abstract class PrefabBufferAuthoring : MonoBehaviour
{
    public List<GameObject> Prefabs;
}
