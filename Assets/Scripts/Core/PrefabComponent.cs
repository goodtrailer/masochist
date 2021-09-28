// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

public interface IPrefabComponent : IComponentData
{
    public Entity entity { get; set; }
}

[UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
public abstract class PrefabDeclarationSystem<A> : GameObjectConversionSystem
    where A : PrefabAuthoring
{
    protected override void OnUpdate()
    {
        Entities.ForEach((A a) =>
        {
            DeclareReferencedPrefab(a.prefab);
        });
    }
}

public abstract class PrefabConversionSystem<A, C> : GameObjectConversionSystem
    where A : PrefabAuthoring
    where C : struct, IPrefabComponent
{
    protected override void OnUpdate()
    {
        Entities.ForEach((A a) =>
        {
            Entity e = GetPrimaryEntity(a);

            DstEntityManager.AddComponentData(e, new C
            {
                entity = GetPrimaryEntity(a.prefab),
            });
        });
    }
}
