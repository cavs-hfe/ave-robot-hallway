using UnityEngine;
using System;

/// <summary>
/// Class to control turret behavior. Assign a turret/gun base object and an object to use for proximity checking.
/// </summary>
public class TurretBehavior : MonoBehaviour
{
    public enum TurretState
    {
        Fixed,
        Sweep,
        Track,
        None
    };

    [SerializeField]
    [Tooltip("Turret Game Object")]
    private GameObject turret;

    [SerializeField]
    [Tooltip("Base of turret that rotates")]
    private GameObject gunMount;

    [SerializeField]
    [Tooltip("Current state of turret: fixed(doesn't move), sweep(back and forth, doesn't react to person), or track(follows person if they are close enough)")]
    private TurretState currentState = TurretState.Fixed;

    [SerializeField]
    [Tooltip("Degrees to sweep +/- from starting position (i.e. half of total sweep arc")]
    [Range(0, 180)]
    private float sweepDegrees = 30;

    //used internally by sweep state
    private float targetDegrees;
    private int sweepDirection = 1;

    [SerializeField]
    [Tooltip("Speed turret can rotate, in degrees per second")]
    private float degreesPerSecond = 10f;

    [SerializeField]
    [Tooltip("Radius of proximity checking, in meters")]
    private float trackProximity = 3;

    [SerializeField]
    private float trackDamping = 6f;

    [SerializeField]
    [Tooltip("Maximum number of degrees turret can swivel in either direction")]
    [Range(0, 180)]
    private float maxDegrees = 90;

    [SerializeField]
    [Tooltip("Object to use with proximity checking")]
    private GameObject proximateObject;

    [SerializeField]
    [Tooltip("If we are track state and we aren't tracking, what should our fallback behavior be?")]
    private TurretState trackDefaultState = TurretState.Sweep;

    private Quaternion initialTurretRotation;

    void Start()
    {
        //get initial transform to reset if needed
        initialTurretRotation = gunMount.transform.localRotation;
        
        //set initial target degrees for Sweep
        targetDegrees = RestrictAngleRange((float)Math.Round(initialTurretRotation.eulerAngles.y + sweepDegrees));        
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case TurretState.Fixed:
                //reset, in case turret was moved by other state and then switched to fixed
                Reset();
                break;
            case TurretState.Sweep:
                //move turret to next position
                Sweep();
                break;
            case TurretState.Track:
                //if close enough, track to person, otherwise default behavior (sweep or fixed?)
                if (Proximity())
                {
                    Track();
                }
                else
                {
                    if (trackDefaultState == TurretState.Fixed)
                    {
                        Reset();                            
                    }
                    else if (trackDefaultState == TurretState.Sweep)
                    {
                        Sweep();
                    }
                }
                break;
            case TurretState.None:
                turret.SetActive(false);
                break;
        }

        //get angle limits
        float leftAngle = RestrictAngleRange((float)Math.Round(initialTurretRotation.eulerAngles.y - maxDegrees));
        float rightAngle = RestrictAngleRange((float)Math.Round(initialTurretRotation.eulerAngles.y + maxDegrees));

        //restrict turret movement to max range
        float currentAngle = (float)Math.Round(gunMount.transform.localRotation.eulerAngles.y);

        if (currentAngle > rightAngle && currentAngle < 180)
        {
            gunMount.transform.localRotation = Quaternion.Euler(gunMount.transform.localRotation.eulerAngles.x, rightAngle, gunMount.transform.localRotation.eulerAngles.z);
        }
        else if (currentAngle < leftAngle && currentAngle > 180)
        {
            gunMount.transform.localRotation = Quaternion.Euler(gunMount.transform.localRotation.eulerAngles.x, leftAngle, gunMount.transform.localRotation.eulerAngles.z);
        }
    }

    /// <summary>
    /// Sweep the turret back and forth using the class variables.
    /// </summary>
    private void Sweep()
    {
        if (Math.Round(gunMount.transform.localRotation.eulerAngles.y) == targetDegrees)
        {
            sweepDirection = sweepDirection * -1;
            targetDegrees = RestrictAngleRange(targetDegrees + sweepDirection * 2 * sweepDegrees);            
        }
        gunMount.transform.localRotation = Quaternion.AngleAxis(gunMount.transform.localRotation.eulerAngles.y + (Time.deltaTime * degreesPerSecond * sweepDirection), Vector3.up);
    }

    /// <summary>
    /// Tracks the turret toward the assigned proximate object. Uses Slerp to smoothly rotate the turret toward the object. 
    /// </summary>
    private void Track()
    {
        Quaternion rotation = Quaternion.LookRotation(proximateObject.transform.position - gunMount.transform.position);
        Quaternion flatRotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        gunMount.transform.rotation = Quaternion.Slerp(gunMount.transform.rotation, flatRotation, Time.deltaTime * trackDamping);
    }

    /// <summary>
    /// Reset turret to inital position. Used when turret is in track state and default behavior is fixed to reset turret if tracked object is lost or goes out of range.
    /// </summary>
    private void Reset()
    {
        gunMount.transform.localRotation = Quaternion.Slerp(gunMount.transform.localRotation, initialTurretRotation, Time.deltaTime * trackDamping);
    }

    /// <summary>
    /// Check to see if the proximate object is within trackProximity meters of the robot.
    /// </summary>
    /// <returns>True if object is within proximity radius, false otherwise</returns>
    private bool Proximity()
    {
        double distance = Math.Pow(Math.Pow(transform.position.x - proximateObject.transform.position.x, 2) + Math.Pow(transform.position.z - proximateObject.transform.position.z, 2), 0.5);
        return distance < trackProximity;
    }

    /// <summary>
    /// Limit the angle values to between 0 and 360. If angle is greater than 360 or less than 0, it will be converted to the corresponding value in the range 0-360.
    /// </summary>
    /// <param name="angle">Angle to check</param>
    /// <returns>Angle value in range of 0-360.</returns>
    private float RestrictAngleRange(float angle)
    {
        if (angle < 0)
        {
            angle = 360 + angle;
        }
        else if (angle >= 360)
        {
            angle = angle - 360;
        }
        return angle;
    }

    public TurretState CurrentTurretState {
        get { return currentState; }
        set { currentState = value; }
    }

    public float SweepDegrees
    {
        get { return sweepDegrees; }
        set { sweepDegrees = value; }
    }

    public float TurretSpeed
    {
        get { return degreesPerSecond; }
        set { degreesPerSecond = value; }
    }

    public float ProximityDistance
    {
        get { return trackProximity; }
        set { trackProximity = value; }
    }

    public float MaxTurretAngle
    {
        get { return maxDegrees; }
        set { maxDegrees = value; }
    }

    public TurretState TrackDefaultState
    {
        get { return trackDefaultState; }
        set { trackDefaultState = value; }
    }

}