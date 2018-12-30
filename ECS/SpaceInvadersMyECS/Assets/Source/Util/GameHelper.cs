public static class GameHelper
{
    public static int getCurrentWeaponIndex(int enemiesDestroyed)
    {
        int currentPercent = -1;
        float percent = enemiesDestroyed / (float)Game.NUM_OF_ENEMIES;
        for (int i = 0; i < Game.neededPercentForNextWeapon.Length; i++)
        {
            if (percent >= Game.neededPercentForNextWeapon[i])
            {
                currentPercent = i;
            }
        }
        return currentPercent;
    }
}