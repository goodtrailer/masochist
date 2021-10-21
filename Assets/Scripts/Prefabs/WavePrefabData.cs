// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

public struct WavePrefabData : IPrefabData, IBufferElementData
{
    public Entity Entity { get; set; }
}

public class WavePrefabDeclarationSystem : PrefabBufferDeclarationSystem<WavePrefabAuthoring> { }

public class WavePrefabConversionSystem : PrefabBufferConversionSystem<WavePrefabAuthoring, WavePrefabData> { }
