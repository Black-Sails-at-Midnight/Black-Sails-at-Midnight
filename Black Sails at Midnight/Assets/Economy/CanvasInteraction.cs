using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasInteraction : MonoBehaviour
{
    [SerializeField]
    Camera FPCamera;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    KeyCode keyCode = KeyCode.F;

    Vault vault;

    // Start is called before the first frame update
    void Start()
    {
        vault = new Vault();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyCode))
        {
            RaycastHit hit;
            if(Physics.Raycast(new Ray(transform.position, FPCamera.transform.forward), out hit, 20f, layerMask))
            {
                PurchaseOption option = hit.collider.GetComponent<PurchaseOption>();
                if (option == null)
                {
                    hit.collider.GetComponent<InteractableCanvasObject>().CanvasAction();
                }
                else
                {
                    if(vault.Withdraw(option.Cost))
                    {
                        hit.collider.GetComponent<InteractableCanvasObject>().CanvasAction();
                    }
                }
            }

        }
    }
}
