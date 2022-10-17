using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class AimDownSight : MonoBehaviour
{
    [SerializeField] 
    public RawImage aimReticle;

    [SerializeField]
    [Range(0, 1)] 
    public float sensitivityModifier = 0.5f;

    [SerializeField] 
    public float FOV = 60;

    public bool scopedIn {get; private set;} = false;

    private float defaultSensitivity;
    private FirstPersonController FPController;
    private Camera FPCamera;
    private Animator animator;
    private Gun gun;

    // Monobehaviour Methods
    public void Start() {
        FPController = FindObjectOfType<FirstPersonController>();
        animator = GetComponent<Animator>();
        FPCamera = Camera.main;
        FOV = FPCamera.fieldOfView;
        gun = GetComponent<Gun>();

        defaultSensitivity = FPController.m_MouseLook.XSensitivity;
    }

    public void Update() 
    {
        if (Input.GetButtonDown("Fire2") && gun.attackSettings.canAttack)
        {
            ScopeIn();
        }

        if (Input.GetButtonUp("Fire2") || (gun.attackSettings.canAttack == false && scopedIn))
        {
            ScopeOut();
        }

        FPCamera.fieldOfView = FOV;
    }

    // Public Methods

    // Private Methods
    private void ScopeIn()
    {
        if (scopedIn)
            return;

        animator.StopPlayback();
        animator.SetBool("IsScopedIn",true);

        if (aimReticle != null)
            aimReticle.enabled = false;

        scopedIn = true;
        FPController.m_MouseLook.XSensitivity = FPController.m_MouseLook.YSensitivity = defaultSensitivity * sensitivityModifier;
    }

    private void ScopeOut()
    {
        if (!scopedIn)
            return;

        animator.StopPlayback();
        animator.SetBool("IsScopedIn",false);

        if (aimReticle != null)
            aimReticle.enabled = true;

        scopedIn = false;
        FPController.m_MouseLook.XSensitivity = FPController.m_MouseLook.YSensitivity = defaultSensitivity;
    }
}
