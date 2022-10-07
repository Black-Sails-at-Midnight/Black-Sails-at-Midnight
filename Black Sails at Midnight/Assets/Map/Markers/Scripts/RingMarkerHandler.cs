using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMarkerHandler : MonoBehaviour
{
    [SerializeField]
    public float radius = 200;

    public float Radius { set 
        {
            radius = value;
            transform.localScale = new Vector3((radius * 2) / 150000, 1, (radius * 2) / 150000);
        }
    }
}
