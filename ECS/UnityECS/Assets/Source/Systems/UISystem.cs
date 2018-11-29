using Unity.Entities;
using UnityEngine.UI;

public class UISystem : ComponentSystem
{
    ComponentGroup group;
    ComponentGroup enemyGroup;
    ComponentGroup playerGroup;

    protected override void OnUpdate()
    {
        if(group == null)
        {
            group = GetComponentGroup(Bootstraper.uiTypes);
            enemyGroup = GetComponentGroup(Bootstraper.enemyTypes);
            playerGroup = GetComponentGroup(Bootstraper.playerTypes);
        }

        var uiEntity = group.GetEntityArray()[0];
        var ui = group.GetSharedComponentDataArray<UI>()[0];
        var numOfEnemies = enemyGroup.CalculateLength();
        var playerStats = playerGroup.GetComponentDataArray<PlayerStats>()[0];


        int currentPercent = -1;
        int neededPercent = 0;
        float percent = playerStats.enemiesDestroyed / (float)Bootstraper.NUM_OF_ENEMIES;
        for (int i = 0; i < Bootstraper.neededPercentForNextWeapon.Length - 1; i++)
        {
            if (percent >= Bootstraper.neededPercentForNextWeapon[i])
            {
                currentPercent = i;
                neededPercent = i + 1;
            }
        }
        float fill = 0;
        if(currentPercent == -1)
        {
            fill = Math.getPercentBetween(percent, 0, Bootstraper.neededPercentForNextWeapon[0]);
        }
        else
        {
            fill = Math.getPercentBetween(percent, Bootstraper.neededPercentForNextWeapon[currentPercent], Bootstraper.neededPercentForNextWeapon[neededPercent]);
        }
        ui.bar.fillAmount = fill;
    }
}