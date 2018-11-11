using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager
{
    Image expBar;

    public void init()
    {
        expBar = GameObject.Find("Canvas/ExpBar/Bar").GetComponent<Image>();
        expBar.fillAmount = 0;
    }

    public void update()
    {
        int currentPercent = -1;
        int neededPercent = 0;
        float percent = Game.instance.enemyDestroyed / (float)Game.NUM_OF_ENEMIES;
        for (int i = 0; i < Game.instance.neededPercentForNextWeapon.Length - 1; i++)
        {
            if (percent >= Game.instance.neededPercentForNextWeapon[i])
            {
                currentPercent = i;
                neededPercent = i + 1;
            }
        }
        float fill = 0;
        if(currentPercent == -1)
        {
            fill = Math.getPercentBetween(percent, 0, Game.instance.neededPercentForNextWeapon[0]);
        }
        else
        {
            fill = Math.getPercentBetween(percent, Game.instance.neededPercentForNextWeapon[currentPercent], Game.instance.neededPercentForNextWeapon[neededPercent]);
        }
        Debug.Log(neededPercent + " " + percent + " " + Game.instance.neededPercentForNextWeapon[0]);

        expBar.fillAmount = fill;
    }
}