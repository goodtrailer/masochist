// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using System.Collections.Generic;
using UnityEngine;

public abstract class EntityAuthoring : MonoBehaviour
{
    public GameObject GameObject;
}

public abstract class EntityBufferAuthoring : MonoBehaviour
{
    public List<GameObject> GameObjects;
}

public abstract class PrefabAuthoring : EntityAuthoring { }

public abstract class PrefabBufferAuthoring : EntityBufferAuthoring { }
