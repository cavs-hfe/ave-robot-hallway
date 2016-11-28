using UnityEngine;
using System.Collections;

public class HallwayController : MonoBehaviour
{

    public GameObject robot;

    public GameObject ABStopper;
    public GameObject CDStopper;

    public GameObject targetA;
    public GameObject targetB;
    public GameObject targetC;
    public GameObject targetD;

    public GameObject doorA;
    public GameObject doorB;
    public GameObject doorC;
    public GameObject doorD;

    public GameObject doorAB;
    public GameObject doorCD;

    public void SetupCondition(ExperimentController.Condition c)
    {
        //set robot behaviors
        robot.GetComponent<RobotDrivingBehavior>().SetSpeed(c.speed);
        robot.GetComponent<RobotDrivingBehavior>().SetTargets(c.playerStartTarget, c.playerGoalTarget);
        robot.GetComponent<RobotDrivingBehavior>().setHallwayPosition(c.hallwayPosition);

        robot.GetComponent<ColorChangeBehavior>().SetBodyColor(c.bodyColor);

        robot.GetComponent<TurretBehavior>().CurrentTurretState = c.turretBehavior;

        //set robot start position/rotation
        if (c.robotStartTarget == ExperimentController.Condition.Target.A)
        {
            robot.transform.position = targetA.transform.position;
        }
        else if (c.robotStartTarget == ExperimentController.Condition.Target.B)
        {
            robot.transform.position = targetB.transform.position;
        }
        else if (c.robotStartTarget == ExperimentController.Condition.Target.C)
        {
            robot.transform.position = targetC.transform.position;
        }
        else if (c.robotStartTarget == ExperimentController.Condition.Target.D)
        {
            robot.transform.position = targetD.transform.position;
        }

        if (c.robotStartTarget == ExperimentController.Condition.Target.A || c.robotStartTarget == ExperimentController.Condition.Target.D)
        {
            robot.transform.rotation = Quaternion.Euler(0, 90, 0);
            Debug.Log("Setting rotation to 90");
        }
        else
        {
            robot.transform.rotation = Quaternion.Euler(0, 270, 0);
            Debug.Log("Setting rotation to 270");
        }

        //set up stopper
        if (c.robotStartTarget == ExperimentController.Condition.Target.A || c.robotStartTarget == ExperimentController.Condition.Target.B)
        {
            ABStopper.SetActive(false);
        }
        else
        {
            CDStopper.SetActive(false);
        }

        //setup player goal
        if (c.playerGoalTarget.Equals(ExperimentController.Condition.Target.A))
        {
            targetA.GetComponent<CapsuleCollider>().enabled = true;
        }
        else if (c.playerGoalTarget.Equals(ExperimentController.Condition.Target.B))
        {
            targetB.GetComponent<CapsuleCollider>().enabled = true;
        }
        else if (c.playerGoalTarget.Equals(ExperimentController.Condition.Target.C))
        {
            targetC.GetComponent<CapsuleCollider>().enabled = true;
        }
        else if (c.playerGoalTarget.Equals(ExperimentController.Condition.Target.D))
        {
            targetD.GetComponent<CapsuleCollider>().enabled = true;
        }

        //set up doors
        if (c.openDoors != null)
        {
            foreach (ExperimentController.Condition.Door d in c.openDoors)
            {
                if (d == ExperimentController.Condition.Door.A)
                {
                    doorA.SetActive(false);
                }
                else if (d == ExperimentController.Condition.Door.B)
                {
                    doorB.SetActive(false);
                }
                else if (d == ExperimentController.Condition.Door.C)
                {
                    doorC.SetActive(false);
                }
                else if (d == ExperimentController.Condition.Door.D)
                {
                    doorD.SetActive(false);
                }
                else if (d == ExperimentController.Condition.Door.AB)
                {
                    doorAB.SetActive(false);
                }
                else if (d == ExperimentController.Condition.Door.CD)
                {
                    doorCD.SetActive(false);
                }
            }
        }
    }
}
