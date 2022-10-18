using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class SpyglassBehaviour : MonoBehaviour
{
    [Header("Scope Settings")]
    [SerializeField]
    public float zoomFOV = 30;
    
    [SerializeField]
    public bool isZoomedIn;

    [Header("Keybindings")]
    [SerializeField]
    public KeyCode zoomKey = KeyCode.G;

    [SerializeField]
    public KeyCode focusPreviousKey = KeyCode.A;

    [SerializeField]
    public KeyCode focusNextKey = KeyCode.D;

    [SerializeField]
    public KeyCode teleportKey = KeyCode.Space;

    [SerializeField]
    public bool teleportOnRelease = true;

    [Header("Teleportation Settings")]
    [SerializeField]
    public float teleportRange = 1000f;

    [SerializeField]
    public LayerMask teleportLayerMask;

    private GameObject lastFocusedObject;
    private FirstPersonController FPController;
    private Camera FPCamera;
    private PlayerLookBehaviour playerLook;
    private WeaponManager weaponManager;

    private Vector3 defaultCameraPosition;
    private float defaultFov;
    private float defaultWalkSpeed;
    private float defaultRunSpeed;
    

    private void Start() {
        FPController = FindObjectOfType<FirstPersonController>();
        FPCamera = Camera.main;
        playerLook = FindObjectOfType<PlayerLookBehaviour>();
        weaponManager = FindObjectOfType<WeaponManager>();

        defaultCameraPosition = FPController.m_OriginalCameraPosition;
        defaultFov = FPCamera.fieldOfView;
    }

    private void Update() {
        if (Input.GetKeyDown(zoomKey) && !isZoomedIn)
        {
            ActivateSpyglass();
        }

        if ((Input.GetKeyUp(zoomKey) && !teleportOnRelease) && isZoomedIn)
        {
            DeactivateSpyglass();
        }

        if (isZoomedIn)
        {
            if (Input.GetKeyDown(focusPreviousKey))
            {
                FocusOnObject(Direction.Previous);
            }

            if (Input.GetKeyDown(focusNextKey))
            {
                FocusOnObject(Direction.Next);
            }

            if ((Input.GetKeyDown(teleportKey) && !teleportOnRelease) || (Input.GetKeyUp(zoomKey) && teleportOnRelease))
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out raycastHit, teleportRange, teleportLayerMask, QueryTriggerInteraction.Ignore))
                {
                    TeleportHandler teleportHandler = raycastHit.collider.gameObject.GetComponentInChildren<TeleportHandler>();
                    if (teleportHandler != null)
                    {
                        Transform targetLocation = teleportHandler.teleportPoint.transform;
                        PlayerRelocator relocator = FindObjectOfType<PlayerRelocator>();
                        relocator.MovePlayer(targetLocation);
                        teleportHandler.GetComponent<PlayerBinder>().Bind(FindObjectOfType<FirstPersonController>().gameObject);
                    }
                }

                if (Input.GetKeyUp(zoomKey) && teleportOnRelease)
                {
                    DeactivateSpyglass();
                }
            }
        }
    }

    private void ActivateSpyglass()
    {
        FPController.m_OriginalCameraPosition = FPController.m_OriginalCameraPosition + new Vector3(0, 100, 0);
        Debug.Log(FPController.m_OriginalCameraPosition);
        FPCamera.fieldOfView = zoomFOV;

        ToggleMovement(false);
        weaponManager.ToggleWeaponFire(false);

        isZoomedIn = true;
    }

    private void DeactivateSpyglass()
    {
        FPController.m_OriginalCameraPosition = defaultCameraPosition;
        Debug.Log(FPController.m_OriginalCameraPosition);
        FPCamera.fieldOfView = defaultFov;
        
        ToggleMovement(true);
        weaponManager.ToggleWeaponFire(true);

        isZoomedIn = false;
    }

    private void ToggleMovement(bool active)
    {
        if (active)
        {
            FPController.m_WalkSpeed = defaultWalkSpeed;
            FPController.m_RunSpeed = defaultRunSpeed;
            FPController.m_JumpEnabled = true;
        } else {
            defaultWalkSpeed = FPController.m_WalkSpeed;
            defaultRunSpeed = FPController.m_RunSpeed;
            FPController.m_JumpEnabled = false;

            FPController.m_WalkSpeed = 0;
            FPController.m_RunSpeed = 0;
        }
    }

    private void FocusOnObject(Direction direction)
    {
        List<GameObject> FocusObject = BuildFocusObjectList();
        if (FocusObject.Count == 0)
            return;

        int lastFocusedIndex = FocusObject.IndexOf(lastFocusedObject);
        int indexToFocusOn = lastFocusedIndex + ((int)direction);

        if (indexToFocusOn < 0)
            indexToFocusOn = FocusObject.Count - 1;
        
        if (indexToFocusOn > FocusObject.Count - 1)
            indexToFocusOn = 0;

        lastFocusedObject = FocusObject[indexToFocusOn];

        playerLook.LookAtTarget(FocusObject[indexToFocusOn]);
    }

    private List<GameObject> BuildFocusObjectList()
    {
        List<GameObject> FocusObjects = new();
        
        List<ShipNavigationAI> ships = new (FindObjectsOfType<ShipNavigationAI>());
        foreach(ShipNavigationAI ship in ships)
        {
            FocusObjects.Add(ship.gameObject);
        }

        FocusObjects.Sort(SortOnDistance);
        return FocusObjects;
    }

    private int SortOnDistance(GameObject a, GameObject b)
    {
        float distanceA = Vector3.Distance(transform.position, a.transform.position);
        float distanceB = Vector3.Distance(transform.position, b.transform.position);

        if (distanceA > distanceB)
            return 1;

        if (distanceB > distanceA)
            return -1;
        
        return 0;
    }
}


public enum Direction {
    Previous = -1,
    Next = 1
}
