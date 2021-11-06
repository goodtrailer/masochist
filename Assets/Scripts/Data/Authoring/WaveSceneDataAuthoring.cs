// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Hash128 = Unity.Entities.Hash128;

public class WaveSceneDataAuthoring : MonoBehaviour
{
    [HideInInspector]
    public List<Hash128> SceneGUIDs;

#if UNITY_EDITOR
    public List<SceneAsset> Scenes;

    void OnValidate()
    {
        SceneGUIDs.Clear();
        foreach (var scene in Scenes)
            SceneGUIDs.Add(new GUID(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(scene))));
    }
#endif
}
