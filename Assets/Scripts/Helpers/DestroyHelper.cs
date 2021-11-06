// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using Unity.Transforms;

public static class DestroyHelper
{
    public static void DestroyHierarchy(in Entity entity,
            in EntityCommandBuffer.ParallelWriter entityCommandBuffer,
            in BufferFromEntity<Child> childrens)
    {
        entityCommandBuffer.DestroyEntity(0, entity);

        if (!childrens.HasComponent(entity))
            return;

        foreach (Child c in childrens[entity])
            DestroyHierarchy(c.Value, entityCommandBuffer, childrens);
    }

    public static void DestroyHierarchy(in Entity entity,
            in EntityCommandBuffer entityCommandBuffer,
            in BufferFromEntity<Child> childrens)
    {
        entityCommandBuffer.DestroyEntity(entity);

        if (!childrens.HasComponent(entity))
            return;

        foreach (Child c in childrens[entity])
            DestroyHierarchy(c.Value, entityCommandBuffer, childrens);
    }
}
