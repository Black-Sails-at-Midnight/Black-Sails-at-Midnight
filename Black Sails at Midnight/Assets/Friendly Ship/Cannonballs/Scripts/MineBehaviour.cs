using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBehaviour : MonoBehaviour
{
    [SerializeField]
    string TagToDetonateOn;
    [SerializeField]
    float Radius;
    [SerializeField]
    float Damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagToDetonateOn && other.GetType() == typeof(MeshCollider))
        {
            RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, Radius, Vector3.up);

            foreach (RaycastHit hit in hits)
            {            
                if(hit.rigidbody != null && hit.rigidbody.tag == TagToDetonateOn)
                {
                    hit.rigidbody.GetComponent<ShipHealth>().Hit(Damage);
                }
            }
            Destroy(gameObject);
        }
    }
}
