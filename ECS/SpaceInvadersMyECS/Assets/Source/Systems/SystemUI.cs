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
            
            int currentIndex = GameHelper.getCurrentWeaponIndex(globalStats.numOfEnemiesDestroyed);
            float percent = globalStats.numOfEnemiesDestroyed / (float)Game.NUM_OF_ENEMIES;
            float fill = 0;
            if(currentIndex == -1)
            {
                fill = Math.getPercentBetween(percent, 0, Game.neededPercentForNextWeapon[0]);
            }
            else if (currentIndex < 2)
            {
                fill = Math.getPercentBetween(percent, Game.neededPercentForNextWeapon[currentIndex], Game.neededPercentForNextWeapon[currentIndex + 1]);
            }
            else
            {
                fill = 1;
            }
            ui.expBar.fillAmount = fill;
        }
    }
}