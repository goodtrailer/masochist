// Copyright (c) Alden Wu <aldenwu0@gmail.com>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence text.

using Unity.Entities;

public struct PlayerData : IComponentData
{
    public float SpeedWalk;
    public float SpeedSprint;
    public bool IsSprinting;

    public float StaminaRegen;
    public float StaminaDecay;

    public float HealthRegen;
    public float HealthDecay;

    public bool SemiAutomatic;
}

public class PlayerConversionSystem : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((PlayerDataAuthoring pd) =>
        {
            Entity e = GetPrimaryEntity(pd);
            DstEntityManager.AddComponentData(e, new PlayerData
            {
                SpeedWalk = pd.SpeedWalk,
                SpeedSprint = pd.SpeedSprint,
                IsSprinting = false,

                StaminaRegen = pd.StaminaRegen,
                StaminaDecay = pd.StaminaDecay,

                HealthRegen = pd.HealthRegen,
                HealthDecay = pd.HealthDecay,

                SemiAutomatic = pd.SemiAutomatic,
            });
        });
    }
}
