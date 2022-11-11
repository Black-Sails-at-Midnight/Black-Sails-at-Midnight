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

    [SerializeField]
    KeyCode RespawnKey;


    public bool isLaunched {get; private set;} = false;

    private Vector3 startPosition;
    private int defaultLayer;

    // Monobehaviour Methods
    private void Start() {
        defaultLayer = ObjectToLauch.gameObject.layer;
    }

    private void Update()
    {
        if (Input.GetKeyDown(RespawnKey))
        {
            FindObjectOfType<PlayerRelocator>().MovePlayer(RespawnPoint);
        }

        if (isLaunched && Vector3.Distance(ObjectToLauch.transform.position, RespawnPoint.position) < DistaceToLandingPoint)
        {
            PrepareForLanding();
            ObjectToLauch.transform.position = RespawnPoint.transform.position;
            ObjectToLauch.velocity = Vector3.zero;
            isLaunched = false;
        }
    }

    // Public Methods
    public void RespawnPlayer()
    {
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
        startPosition = ObjectToLauch.GetComponentInChildren<PlayerMovement>().transform.position;
        ObjectToLauch.gameObject.layer = Mathf.RoundToInt(Mathf.Log(inFlightLayer.value, 2));

        // Disable Player Elements
        ObjectToLauch.GetComponentInChildren<WeaponManager>().ToggleWeaponRenderers(false);
        ObjectToLauch.GetComponent<PlayerMovement>().disableMovement = true;
        ObjectToLauch.velocity = Vector3.zero;
    }

    private void PrepareForLanding()
    {
        ObjectToLauch.gameObject.layer = defaultLayer;

        // Enable Player Elements
        ObjectToLauch.GetComponentInChildren<WeaponManager>().ToggleWeaponRenderers(true);
        ObjectToLauch.GetComponent<PlayerMovement>().disableMovement = false;  
    }
}
