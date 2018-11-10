public interface IWeapon
{
    void updateWeapon(IPlayer player);
    void changeBullet(IBullet newBullet);
    void init();
    float getCooldown();
    float getLastShotTime();
    void destroy();
}