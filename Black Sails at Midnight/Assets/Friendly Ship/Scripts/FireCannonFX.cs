using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannonFX : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject LeftCannons;

    [SerializeField]
    GameObject RightCannons;

    public void FireRightCannons()
    {
        ParticleSystem[] system = RightCannons.GetComponentsInChildren<ParticleSystem>();

        foreach (var item in system)
        {
            item.Play();
        }
    }

    public void FireLeftCannons()
    {
        ParticleSystem[] system = LeftCannons.GetComponentsInChildren<ParticleSystem>();

        foreach (var item in system)
        {
            item.Play();
        }
    }
}
