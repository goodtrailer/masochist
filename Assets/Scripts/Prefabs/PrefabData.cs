// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

public interface IPrefabData
{
    public Entity Entity { get; set; }
}

[UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
public abstract class PrefabDeclarationSystem<A> : GameObjectConversionSystem
    where A : PrefabAuthoring
{
    protected override void OnUpdate()
    {
        Entities.ForEach((A a) =>
        {
            DeclareReferencedPrefab(a.Prefab);
        });
    }
}

public abstract class PrefabConversionSystem<A, C> : GameObjectConversionSystem
    where A : PrefabAuthoring
    where C : struct, IPrefabData, IComponentData
{
    protected override void OnUpdate()
    {
        Entities.ForEach((A a) =>
        {
            Entity e = GetPrimaryEntity(a);

            DstEntityManager.AddComponentData(e, new C
            {
                Entity = GetPrimaryEntity(a.Prefab),
            });
        });
    }
}

[UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
public abstract class PrefabBufferDeclarationSystem<A> : GameObjectConversionSystem
    where A : PrefabBufferAuthoring
{
    protected override void OnUpdate()
    {
        Entities.ForEach((A a) =>
        {
            foreach (var prefab in a.Prefabs)
                DeclareReferencedPrefab(prefab);
        });
    }
}

public abstract class PrefabBufferConversionSystem<A, C> : GameObjectConversionSystem
    where A : PrefabBufferAuthoring
    where C : struct, IPrefabData, IBufferElementData
{
    protected override void OnUpdate()
    {
        Entities.ForEach((A a) =>
        {
            Entity e = GetPrimaryEntity(a);
            
            DynamicBuffer<C> components = DstEntityManager.AddBuffer<C>(e);
            foreach (var prefab in a.Prefabs)
                components.Add(new C
                {
                    Entity = GetPrimaryEntity(prefab),
                });
        });
    }
}
