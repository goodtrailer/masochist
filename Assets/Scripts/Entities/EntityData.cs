// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

public interface IEntityData
{
    public Entity Entity { get; set; }
}

public interface IPrefabData : IEntityData { }

public abstract class EntityConversionSystem<A, C> : GameObjectConversionSystem
    where A : EntityAuthoring
    where C : struct, IEntityData, IComponentData
{
    protected override void OnUpdate()
    {
        Entities.ForEach((A a) =>
        {
            Entity e = GetPrimaryEntity(a);

            DstEntityManager.AddComponentData(e, new C
            {
                Entity = GetPrimaryEntity(a.GameObject),
            });
        });
    }
}

public abstract class EntityBufferConversionSystem<A, C> : GameObjectConversionSystem
    where A : EntityBufferAuthoring
    where C : struct, IEntityData, IBufferElementData
{
    protected override void OnUpdate()
    {
        Entities.ForEach((A a) =>
        {
            Entity e = GetPrimaryEntity(a);

            DynamicBuffer<C> components = DstEntityManager.AddBuffer<C>(e);
            foreach (var gameObject in a.GameObjects)
                components.Add(new C
                {
                    Entity = GetPrimaryEntity(gameObject),
                });
        });
    }
}

[UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
public abstract class PrefabDeclarationSystem<A> : GameObjectConversionSystem
    where A : PrefabAuthoring
{
    protected override void OnUpdate()
    {
        Entities.ForEach((A a) =>
        {
            DeclareReferencedPrefab(a.GameObject);
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
            foreach (var gameObject in a.GameObjects)
                DeclareReferencedPrefab(gameObject);
        });
    }
}
