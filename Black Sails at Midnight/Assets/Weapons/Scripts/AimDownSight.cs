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

    private Dictionary<string, float> defaultValues;
    private FirstPersonController FPController;
    private Camera FPCamera;
    private Animator animator;

    // Monobehaviour Methods
    public void Start() {
        FPController = FindObjectOfType<FirstPersonController>();
        animator = GetComponent<Animator>();
        FPCamera = Camera.main;
        FOV = FPCamera.fieldOfView;

        defaultValues = new();
        defaultValues.Add("Sensitivity", FPController.m_MouseLook.XSensitivity);
    }

    public void Update() 
    {
        if (Input.GetButtonDown("Fire2"))
        {
            ScopeIn();
        }

        if (Input.GetButtonUp("Fire2"))
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
        animator.SetTrigger("ScopeIn");

        if (aimReticle != null)
            aimReticle.enabled = false;

        FPController.m_MouseLook.XSensitivity = FPController.m_MouseLook.YSensitivity = defaultValues["Sensitivity"] * sensitivityModifier;
        scopedIn = true;
    }

    private void ScopeOut()
    {
        if (!scopedIn)
            return;

        animator.StopPlayback();
        animator.SetTrigger("ScopeOut");

        if (aimReticle != null)
            aimReticle.enabled = true;

        FPController.m_MouseLook.XSensitivity = FPController.m_MouseLook.YSensitivity = defaultValues["Sensitivity"];
        scopedIn = false;
    }
}
