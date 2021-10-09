// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

public struct LevelData : IComponentData
{
    public float Level;
    public float LevelMax;
    public double NextLevelUpTime;
    public float LevelUpCoefficient;
    public float LevelUpExponent;
}

public class LevelConversionSystem : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((LevelDataAuthoring ld) =>
        {
            Entity e = GetPrimaryEntity(ld);
            DstEntityManager.AddComponentData(e, new LevelData
            {
                Level = 1f,
                LevelMax = ld.LevelMax,
                NextLevelUpTime = Time.ElapsedTime + ld.LevelUpCoefficient,
                LevelUpCoefficient = ld.LevelUpCoefficient,
                LevelUpExponent = ld.LevelUpExponent,
            });
        });
    }
}
