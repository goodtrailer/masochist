// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

public class RunSystem : SystemBase
{
    protected override void OnStartRunning()
    {
        base.OnStartRunning();

        EntityManager.CreateEntity(typeof(RunData));

        SetSingleton(new RunData
        {
            StartTime = Time.ElapsedTime,
            InProgress = true,
        });
    }

    protected override void OnUpdate()
    {
        var r = GetSingleton<RunData>();
        if (!r.InProgress)
            return;
        EntityQuery spawners = GetEntityQuery(ComponentType.ReadOnly<SpawnerData>());
        if (!spawners.IsEmpty)
            return;
        EntityQuery enemies = GetEntityQuery(ComponentType.ReadOnly<EnemyTag>());
        if (!enemies.IsEmpty)
            return;

        r.EndTime = Time.ElapsedTime;
        r.InProgress = false;
        SetSingleton(r);

        Entities.ForEach((EndScreenUIData esUI) =>
        {
            int time = (int)(r.EndTime - r.StartTime);
            esUI.Screen.Show(string.Format("{0:D2}:{1:D2}", time / 60, time % 60));
        }).WithoutBurst().Run();
    }
}
