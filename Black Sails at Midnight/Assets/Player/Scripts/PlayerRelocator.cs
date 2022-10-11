using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerRelocator : MonoBehaviour
{
    [SerializeField]
    LayerMask inFlightLayer;

    private int defaultLayer;
    public void Move(Transform targetPosition)
    {
        FirstPersonController FPController = FindObjectOfType<FirstPersonController>();
        defaultLayer = FPController.gameObject.layer;
        FPController.gameObject.layer = Mathf.RoundToInt(Mathf.Log(inFlightLayer.value, 2));

        FPController.m_CharacterController.Move(targetPosition.position - FPController.transform.position);
        FPController.gameObject.layer = defaultLayer;
    }
}
