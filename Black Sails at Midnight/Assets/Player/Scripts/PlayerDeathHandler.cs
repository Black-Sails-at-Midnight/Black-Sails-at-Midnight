using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Vehicles.Ball;

public class PlayerDeathHandler : MonoBehaviour
{
    public void Die(GameObject player)
    {
        // Do whatever needs to happen when the player dies
    }

    [SerializeField]
    Rigidbody ObjectToLauch;
    [SerializeField]
    Transform Origin;
    [SerializeField]
    Transform Target;
    [SerializeField]
    float angle;

    bool isLaunched = false;

    private void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            PrepareForLaunch();
            ObjectToLauch.velocity = CalculateTrajectory();
            isLaunched = true;
        }

        if (isLaunched && Vector3.Distance(Origin.position, Target.position) < 1)
        {
            PrepareForLanding();
            isLaunched = false;
        }
    }

    private Vector3 CalculateTrajectory()
    {
        Vector3 Direction = Target.position - Origin.position;
        float deltaHeight = Direction.y;
        Direction.y = 0; // We calculate the required vertical direction.
        float Distance = Direction.magnitude;
        float angleInRadians = angle * Mathf.Deg2Rad;
        Direction.y = Distance * Mathf.Tan(angleInRadians); // Set it to elevation angle.
        Distance += deltaHeight / Mathf.Tan(angleInRadians); // N
        float velocity  = Mathf.Sqrt(Distance * Physics.gravity.magnitude / Mathf.Sin(2 * angleInRadians)); // TLDR: Gets the speed based on the angle and distance, while accounting for gravity.

        return velocity * Direction.normalized;
    }

    private void PrepareForLaunch()
    {
        ObjectToLauch.GetComponentInChildren<FirstPersonController>().enabled = false;
        ObjectToLauch.GetComponentInChildren<Rigidbody>().isKinematic = false;
    }

    private void PrepareForLanding()
    {
        ObjectToLauch.GetComponentInChildren<FirstPersonController>().enabled = true;
        ObjectToLauch.GetComponentInChildren<Rigidbody>().isKinematic = true;
    }
}
