// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

public class RunSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();

        EntityManager.CreateEntity(typeof(RunData));

        SetSingleton(new RunData
        {
            StartTime = Time.ElapsedTime,
        });
    }

    protected override void OnUpdate()
    {
    }
}
