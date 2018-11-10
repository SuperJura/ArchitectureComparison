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
        expBar.fillAmount = Game.instance.enemyDestroyed / (float)Game.instance.needToDestroyForNextWeapon;
    }
}