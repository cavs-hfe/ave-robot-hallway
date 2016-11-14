using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ExperimentController : MonoBehaviour
{

    private enum State
    {
        VRFamiliarization,
        PostVRFamilSSQ,
        TaskFamiliarization,
        PostTaskFamilSSQ,
        Lobby,
        Trial
    }


    //needed by all
    private GameObject player;

    private int participantNumber = 0;
    private int runNumber = 0;
    private Queue<Condition> conditions;

    //needed by hallway
    private GameObject robot;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("ERROR: can't find player object!");
            return;
        }

        DontDestroyOnLoad(player);
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        SetupConditions();

        SceneManager.LoadScene(1);
    }

    private void SetupConditions()
    {
        conditions = new Queue<Condition>();

        conditions.Enqueue(new Condition(ColorChangeBehavior.Red, Color.red, Condition.UnderglowPattern.Off, Condition.SpeedFast, Condition.Target.C, Condition.Target.B, Condition.PositionA, TurretBehavior.TurretState.None));
        conditions.Enqueue(new Condition(ColorChangeBehavior.Yellow, Color.red, Condition.UnderglowPattern.Off, Condition.SpeedMedium, Condition.Target.B, Condition.Target.D, Condition.PositionB, TurretBehavior.TurretState.Sweep));
        conditions.Enqueue(new Condition(ColorChangeBehavior.Black, Color.red, Condition.UnderglowPattern.Off, Condition.SpeedSlow, Condition.Target.D, Condition.Target.A, Condition.PositionC, TurretBehavior.TurretState.None));
        conditions.Enqueue(new Condition(ColorChangeBehavior.Neutral, Color.red, Condition.UnderglowPattern.Off, Condition.SpeedFast, Condition.Target.A, Condition.Target.C, Condition.PositionD, TurretBehavior.TurretState.Fixed));
        conditions.Enqueue(new Condition(ColorChangeBehavior.Green, Color.red, Condition.UnderglowPattern.Off, Condition.SpeedMedium, Condition.Target.C, Condition.Target.A, Condition.PositionE, TurretBehavior.TurretState.Track));
    }

    void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded");
        switch (scene.buildIndex)
        {
            case 1: //hallway
                Debug.Log("Hallway");
                //setup robot initial conditions
                robot = GameObject.FindGameObjectWithTag("Robot");

                RobotDrivingBehavior rdb;
                ColorChangeBehavior ccb;
                TurretBehavior tb;

                if (robot == null)
                {
                    Debug.Log("ERROR: Couldn't find robot!");
                    return;
                }
                else
                {
                    rdb = robot.GetComponent<RobotDrivingBehavior>();
                    ccb = robot.GetComponent<ColorChangeBehavior>();
                    tb = robot.GetComponent<TurretBehavior>();

                    if (rdb == null || ccb == null || tb == null)
                    {
                        Debug.Log("ERROR: robot missing required component (RobotDrivingBehavior, ColorChangeBehavior, or TurretBehavior)");
                        return;
                    }
                }

                Debug.Log("Setting condition");

                Condition c = conditions.Dequeue();

                //set robot behaviors
                rdb.SetSpeed(c.speed);
                rdb.SetTargets(c.startTarget, c.goalTarget);
                rdb.setHallwayPosition(c.hallwayPosition);

                ccb.SetBodyColor(c.bodyColor);

                tb.CurrentTurretState = c.turretBehavior;

                //set robot start position/rotation
                GameObject target;
                if (c.startTarget == Condition.Target.A)
                {
                    target = GameObject.Find("TargetA");
                }
                else if (c.startTarget == Condition.Target.B)
                {
                    target = GameObject.Find("TargetB");
                }
                else if (c.startTarget == Condition.Target.C)
                {
                    target = GameObject.Find("TargetC");
                }
                else
                {
                    target = GameObject.Find("TargetD");
                }

                Debug.Log("Setting position to " + target.name + ": " + target.transform.position.ToString());
                robot.transform.position = target.transform.position;

                if (c.startTarget == Condition.Target.A || c.startTarget == Condition.Target.D)
                {
                    robot.transform.rotation = Quaternion.Euler(0, 90, 0);
                    Debug.Log("Setting rotation to 90");
                }
                else
                {
                    robot.transform.rotation = Quaternion.Euler(0, 270, 0);
                    Debug.Log("Setting rotation to 270");
                }

                Debug.Log("Robot rotation: " + target.transform.rotation.eulerAngles.ToString());

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public class Condition
    {
        public static float PositionA = -0.77f;
        public static float PositionB = -0.61f;
        public static float PositionC = 0;
        public static float PositionD = 0.61f;
        public static float PositionE = 0.77f;

        public enum Target
        {
            A, B, C, D
        }

        public enum UnderglowPattern
        {
            Flash, Breathe, On, Off
        }

        public static float SpeedFast = 1.4f;
        public static float SpeedMedium = 1.23f;
        public static float SpeedSlow = 0.9f;

        public Color bodyColor;
        public Color underglowColor;
        public UnderglowPattern underglowPattern;
        public float speed;
        public Target startTarget;
        public Target goalTarget;
        public float hallwayPosition;
        public TurretBehavior.TurretState turretBehavior;

        public Condition(Color bodyColor, Color underglowColor, UnderglowPattern pattern, float speed, Target startPosition, Target goalPosition, float hallwayPosition, TurretBehavior.TurretState turretBehavior)
        {
            this.bodyColor = bodyColor;
            this.underglowColor = underglowColor;
            this.underglowPattern = pattern;
            this.speed = speed;
            this.startTarget = startPosition;
            this.goalTarget = goalPosition;
            this.hallwayPosition = hallwayPosition;
            this.turretBehavior = turretBehavior;
        }


    }
}
