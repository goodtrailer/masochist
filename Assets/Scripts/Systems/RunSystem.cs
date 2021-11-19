// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;
using UnityEngine;

public class RunSystem : SystemBase
{
    bool prevEmpty = true;

    protected override void OnCreate()
    {
        base.OnCreate();

        EntityManager.CreateEntity(typeof(RunData));
        SetSingleton(new RunData
        {
            StartTime = 0.0,
            InProgress = false,
        });
    }

    protected override void OnUpdate()
    {
        EntityQueryDesc query = new EntityQueryDesc
        {
            Any = new ComponentType[]
            {
                ComponentType.ReadOnly<SpawnerData>(),
                ComponentType.ReadOnly<EnemyTag>(),
            },
        };

        bool empty = GetEntityQuery(query).IsEmpty;
        if (prevEmpty == empty)
            return;
        prevEmpty = empty;

        RunData r = GetSingleton<RunData>();
        if (empty)
        {
            r.EndTime = Time.ElapsedTime;
            r.InProgress = false;
        }
        else
        {
            r.StartTime = Time.ElapsedTime;
            r.InProgress = true;
        }
        SetSingleton(r);
    }
}
