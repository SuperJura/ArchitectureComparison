using System.Collections.Generic;
using UnityEngine;

public class Rifle : IWeapon
{
    readonly IBullet DEFAULT_BULLET = new LaserBullet();
    IBullet bullet;
    float lastShotTime = -1;
    float cooldown;

    List<IBullet> bullets;

    public Rifle(float cooldown)
    {
        this.cooldown = cooldown;
        changeBullet(DEFAULT_BULLET);
        bullets = new List<IBullet>();
    }

    public void changeBullet(IBullet newBullet)
    {
        bullet = newBullet;
    }

    public void updateWeapon(IPlayer player)
    {
        if(player.getDevice().getFire()) shoot(player.getPlayerObject());
        
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            IBullet bullet = bullets[i];
            bullet.move();
            if(bullet.getLifeTime() <= Time.time - bullet.getSpawnTime())
            {
                bullet.destroy();
                bullets.RemoveAt(i);
            }   
        }
    }
    
    public void shoot(GameObject player)
    {
        if(getLastShotTime() >= Time.time - getCooldown()) return;
        IBullet[] newBullets = new IBullet[] { bullet.copy(), bullet.copy() };
        for (int i = 0; i < newBullets.Length; i++)
        {
            int offset = i % 2 == 0 ? 10 : -10;
            Vector3 pos = player.transform.position;
            pos += player.transform.right * offset;
            IBullet bullet = newBullets[i];
            bullet.init(pos, player.transform.rotation);
            bullets.Add(bullet);
        }
        lastShotTime = Time.time;
    }

    public float getCooldown()
    {
        return cooldown;
    }

    public float getLastShotTime()
    {
        return lastShotTime;
    }

    public void init()
    {
        bullet = DEFAULT_BULLET;
    }

    public void destroy()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            bullets[i].destroy();
        }
        bullets.Clear();
    }
}