using UnityEngine;

public class Beam : IWeapon
{
    readonly IBullet DEFAULT_BULLET = new BeamBullet();
    IBullet bullet;
    float lastShotTime = -1;
    float cooldown;

    IBullet currentBeam;

    public Beam()
    {
        changeBullet(DEFAULT_BULLET);
    }

    public void changeBullet(IBullet newBullet)
    {
        bullet = newBullet;
    }

    public void updateWeapon(IPlayer player)
    {
        lastShotTime = Time.time;
        if(player.getDevice().getFire())
        {
            if(currentBeam == null)
            {
                currentBeam = bullet.copy();
                currentBeam.init(player.getPlayerObject().transform.position, player.getPlayerObject().transform.rotation);
                currentBeam.getObject().transform.SetParent(player.getPlayerObject().transform);
                currentBeam.getObject().transform.localPosition += new Vector3(0, 0, 15);
            }
            else
            {
                float newScale = Mathf.Clamp(currentBeam.getObject().transform.localScale.x + Time.deltaTime, 1, 4f);
                currentBeam.getObject().transform.localScale = new Vector3(newScale, newScale, 1);
            }
        }
        else
        {
            if(currentBeam != null)
            {
                currentBeam.destroy();
                currentBeam = null;
            }
        }
    }

    public float getCooldown()
    {
        return 20;
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
        currentBeam.destroy();
        currentBeam = null;
    }
}