// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

public struct AttackPrefabData : IPrefabData, IComponentData
{
    public Entity Entity { get; set; }
}

public class AttackPrefabDeclarationSystem : PrefabDeclarationSystem<AttackPrefabAuthoring> { }

public class AttackPrefabConversionSystem : PrefabConversionSystem<AttackPrefabAuthoring, AttackPrefabData> { }
