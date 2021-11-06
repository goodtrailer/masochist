using Unity.Entities;

public struct LimitBreakAttackPrefabData : IPrefabData, IComponentData
{
    public Entity Entity { get; set; }
}

public class LimitBreakAttackPrefabConversionSystem : EntityConversionSystem<LimitBreakAttackPrefabAuthoring, LimitBreakAttackPrefabData> { }

public class LimitBreakAttackPrefabDeclarationSystem : PrefabDeclarationSystem<LimitBreakAttackPrefabAuthoring> { }
