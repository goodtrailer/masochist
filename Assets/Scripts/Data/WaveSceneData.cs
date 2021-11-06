// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using UnityEditor;

public struct WaveSceneData : IBufferElementData
{
    public Hash128 GUID;
}

public class WaveSceneConversionSystem : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((WaveSceneDataAuthoring a) =>
        {
            Entity e = GetPrimaryEntity(a);

            var b = DstEntityManager.AddBuffer<WaveSceneData>(e);
            foreach (var guid in a.SceneGUIDs)
                b.Add(new WaveSceneData
                {
                    GUID = guid,
                });
        });
    }
}
