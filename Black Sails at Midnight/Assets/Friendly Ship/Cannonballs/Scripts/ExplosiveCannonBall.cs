using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCannonBall : CannonBallProperties
{
    [SerializeField]
    [Range(0,100)]
    float Radius;
    [SerializeField]
    [Range(0, 100)]
    float SplashDamage;
    [SerializeField]
    GameObject explosionFX;
    public void CauseExplosion()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, Radius, transform.forward);
        foreach (var target in hits)
        {
            if (target.collider.tag == "Enemy")
            {
                target.collider.GetComponent<ShipHealth>().Hit(SplashDamage);
            }
        }
        //ShowVFX(); // Needs a VFX obj before we can actually use this.
    }

    private void ShowVFX()
    {
        GameObject obj = Instantiate(explosionFX, transform.position, Quaternion.identity);
        obj.GetComponent<ParticleSystem>().Play();
        Destroy(obj, 2f);
    }
}
