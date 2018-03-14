using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RobotDrivingBehavior : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Speed of robot movement, in m/s")]
    private float speed;

    [SerializeField]
    [Tooltip("Distance from centerline of hallway to drive robot, use ExperimentController.Condition values (ex: ExperimentController.Condition.PositionA)")]
    private float hallwayPosition;

    private ExperimentController.Condition.Target startTarget;
    private ExperimentController.Condition.Target endTarget;

    //How close does the robot need to get to it's target position before it begins the turn?
    private float tolerance = 0.1f;

    private bool hasTurned = false;
    private bool turning = false;
    private Quaternion goalRotation;

    [SerializeField]
    [Tooltip("Rate to turn robot, in degrees per second")]
    private float turnSpeed = 20;

    private void Update()
    {
        // debug shortcut to end level
        if (Input.GetButtonUp("Fire1"))
        {
            Debug.Log("Button Down");
            SceneManager.LoadScene(1);
        }
    }

    void FixedUpdate()
    {
        

        if (!hasTurned && !turning)
        {
            //Debug.Log("Time to turn?");
            //check to see how close we are to the target position
            float distance = Mathf.Abs(this.transform.position.x - hallwayPosition);
            if (distance < tolerance)
            {
                //close enough, start turning
                turning = true;

                //set goal angle/direction
                //Debug.Log("Start Target: " + startTarget);
                if (startTarget == ExperimentController.Condition.Target.A || startTarget == ExperimentController.Condition.Target.C)
                {
                    //turn left (subtract 90 from start angle)
                    //goalRotation *= Quaternion.AngleAxis(270, Vector3.up);
                    //float angle = (transform.rotation.eulerAngles.y + 90) % 360;
                    //Debug.Log("Turn Left from " + transform.rotation.eulerAngles.y + " to " + angle);
                    
                    goalRotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, (transform.rotation.eulerAngles.y - 90) % 360, transform.rotation.eulerAngles.z));
                }
                else
                {
                    //float angle = (transform.rotation.eulerAngles.y + 90) % 360;
                    //Debug.Log("Turn Right from " + transform.rotation.eulerAngles.y + " to " + angle);
                    //turn right (add 90 to start angle)
                    goalRotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, (transform.rotation.eulerAngles.y + 90) % 360, transform.rotation.eulerAngles.z));
                }
            }
            else
            {
                //move forward
                this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
        else if (turning)
        {
            //Debug.Log("Turning, goal: " + goalRotation.ToString());

            this.transform.rotation = Quaternion.Lerp(transform.rotation, goalRotation, turnSpeed * Time.time);

            if (this.transform.rotation.eulerAngles.y == goalRotation.eulerAngles.y)
            {
                turning = false;
                hasTurned = true;
            }
        }
        else
        {
            //Debug.Log("Done turning, driving");
            //move forward
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Stopper"))
        {
            this.speed = 0;
        }
    }

    /// <summary>
    /// Set the speed of the robot, in meters per second.
    /// </summary>
    /// <param name="speed">speed of robot as float, in meters per second</param>
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    /// <summary>
    /// Set the distance from centerline of hallway for robot to drive, in meters. Constant variables stored in ExperimentController.Condition (ex. ExperimentController.Condition.PositionA)
    /// </summary>
    /// <param name="hallwayPosition">Float, preferably from static variables in ExperimentController.Condition, offset from centerline in meters</param>
    public void setHallwayPosition(float hallwayPosition)
    {
        this.hallwayPosition = hallwayPosition;
    }

    public void SetTargets(ExperimentController.Condition.Target startTarget, ExperimentController.Condition.Target endTarget)
    {
        this.startTarget = startTarget;
        this.endTarget = endTarget;
    }
}
