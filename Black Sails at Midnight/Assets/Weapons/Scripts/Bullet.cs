using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    public float maxFlightTime = 5f;

    [SerializeField]
    public ParticleSystem hitEffect;

    private Gun shotBy;
    public Gun ShotBy {set {shotBy = value;}}

    private float creationTime;

    private void Start() {
        creationTime = Time.timeSinceLevelLoad;
    }

    private void Update() 
    {
        if (Time.timeSinceLevelLoad - creationTime > maxFlightTime)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "Projectile")
            return;
        
        IAttackable attackable = other.gameObject.GetComponentInChildren<IAttackable>();
        
        if (attackable != null)
        {
            attackable.Hit(shotBy.gameObject, shotBy);
        }

        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.LookRotation(other.GetContact(0).normal));
        
        Destroy(this.gameObject);
    }
    
}
