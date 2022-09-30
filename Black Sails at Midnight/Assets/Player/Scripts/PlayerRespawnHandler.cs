using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerRespawnHandler : MonoBehaviour
{
    [SerializeField]
    Rigidbody ObjectToLauch;
    [SerializeField]
    Transform RespawnPoint;
    [SerializeField]
    float DistaceToLandingPoint = 1f;
    [SerializeField]
    float Angle = 45f;
    [SerializeField]
    LayerMask inFlightLayer;


    public bool isLaunched {get; private set;} = false;

    private Vector3 startPosition;
    private int defaultLayer;

    // Toglable Component References
    WeaponManager weaponManager;

    // Monobehaviour Methods
    private void Start() {
        weaponManager = ObjectToLauch.GetComponentInChildren<WeaponManager>(); 
    }

    private void Update()
    {
        if (isLaunched && Vector3.Distance(ObjectToLauch.transform.position, RespawnPoint.position) < DistaceToLandingPoint)
        {
            PrepareForLanding();
            isLaunched = false;
        }
    }

    // Public Methods
    public void RespawnPlayer()
    {
        Debug.Log("Respawning");
        PrepareForLaunch();
        ObjectToLauch.velocity = CalculateTrajectory();
        isLaunched = true;
    }


    // Private Methods
    private Vector3 CalculateTrajectory()
    {
        Vector3 Direction = RespawnPoint.position - ObjectToLauch.transform.position;
        float deltaHeight = Direction.y;
        Direction.y = 0; // We calculate the required vertical direction.
        float Distance = Direction.magnitude;
        float angleInRadians = Angle * Mathf.Deg2Rad;
        Direction.y = Distance * Mathf.Tan(angleInRadians); // Set it to elevation angle.
        Distance += deltaHeight / Mathf.Tan(angleInRadians); // N
        float velocity  = Mathf.Sqrt(Distance * Physics.gravity.magnitude / Mathf.Sin(2 * angleInRadians)); // TLDR: Gets the speed based on the angle and distance, while accounting for gravity.

        return velocity * Direction.normalized;
    }

    private void PrepareForLaunch()
    {
        ObjectToLauch.velocity = Vector3.zero;
        ObjectToLauch.GetComponentInChildren<FirstPersonController>().enabled = false;
        
        startPosition = ObjectToLauch.GetComponentInChildren<FirstPersonController>().transform.position;
        defaultLayer = ObjectToLauch.gameObject.layer;
        ObjectToLauch.gameObject.layer = Mathf.RoundToInt(Mathf.Log(inFlightLayer.value, 2));

        // Disable Player Weapons
        weaponManager.ToggleWeaponRenderers(false);
    }

    private void PrepareForLanding()
    {
        ObjectToLauch.gameObject.layer = defaultLayer;

        FirstPersonController FPController = ObjectToLauch.GetComponentInChildren<FirstPersonController>();
        FPController.enabled = true;
        FPController.m_CharacterController.Move(RespawnPoint.position - startPosition);
        FPController.m_CharacterController.Move(RespawnPoint.position - FPController.transform.position);

        // Enable Player Weapons
        weaponManager.ToggleWeaponRenderers(true);        
    }
}
