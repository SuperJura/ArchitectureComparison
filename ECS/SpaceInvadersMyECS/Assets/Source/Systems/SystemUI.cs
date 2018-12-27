using UnityEngine;
using UnityEngine.UI;
using static EntityManager;

public class SystemUI : ISystem
{
    Component[] filter = new Component[]
    {
        new ComponentUI()
    };

    Component[] globalStatsFilter = new Component[]
    {
        new ComponentGlobalPlayerStats()
    };

    public void update()
    {
        var globalStats = getFirstComponent<ComponentGlobalPlayerStats>(globalStatsFilter);
        var entities = getEntities(filter);
        for (int j = 0; j < entities.Count; j++)
        {
            var components = getComponents(entities[j]);
            var ui = getComponent<ComponentUI>(components);
            
            int currentPercent = -1;
            int neededPercent = 0;
            float percent = globalStats.numOfEnemiesDestroyed / (float)Game.NUM_OF_ENEMIES;
            for (int i = 0; i < Game.neededPercentForNextWeapon.Length - 1; i++)
            {
                if (percent >= Game.neededPercentForNextWeapon[i])
                {
                    currentPercent = i;
                    neededPercent = i + 1;
                }
            }
            float fill = 0;
            if(currentPercent == -1)
            {
                fill = Math.getPercentBetween(percent, 0, Game.neededPercentForNextWeapon[0]);
            }
            else
            {
                fill = Math.getPercentBetween(percent, Game.neededPercentForNextWeapon[currentPercent], Game.neededPercentForNextWeapon[neededPercent]);
            }
            ui.expBar.fillAmount = fill;
        }
    }
}