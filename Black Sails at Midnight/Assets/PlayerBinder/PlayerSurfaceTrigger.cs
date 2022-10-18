using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSurfaceTrigger : MonoBehaviour
{
    [SerializeField]
    public LayerMask hitLayers;

    [SerializeField]
    public float castDistance = 5f;


    // Update is called once per frame
    void Update()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, Vector3.down, out raycastHit, castDistance, hitLayers.value, QueryTriggerInteraction.Ignore))
        {
            IBindingSurface bindingSurface = raycastHit.collider.GetComponentInChildren<IBindingSurface>();
            if (bindingSurface == null)
                return;
            
            bindingSurface.Bind(gameObject);
        }

        Debug.DrawRay(transform.position, Vector3.down, Color.red, castDistance);
    }
}
