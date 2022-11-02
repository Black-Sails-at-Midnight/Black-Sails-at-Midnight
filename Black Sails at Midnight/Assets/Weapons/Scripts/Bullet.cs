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
    public Gun ShotBy { set { shotBy = value; } }

    private float creationTime;

    private void Start()
    {
        creationTime = Time.timeSinceLevelLoad;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("Ship"));
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad - creationTime > maxFlightTime)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "EnemyCrew")
        {
            other.gameObject.GetComponent<CrewHealth>().Hit(shotBy.gameObject, shotBy);
            Destroy(gameObject);
        }
    }
}
