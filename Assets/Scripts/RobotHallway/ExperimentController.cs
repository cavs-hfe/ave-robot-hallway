using UnityEngine;
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

    Condition defaultCondition;

    //needed by hallway
    private GameObject robot;

    void Start()
    {
        defaultCondition = new Condition();
        defaultCondition.SetTargetType(Condition.TargetType.Robot);
        defaultCondition.SetBodyColor(ColorChangeBehavior.Maroon);
        defaultCondition.SetUnderglow(Color.red, Condition.UnderglowPattern.Off);
        defaultCondition.SetSpeed(Condition.SpeedSlow);
        defaultCondition.SetHallPosition(Condition.PositionA);
        defaultCondition.SetRobotPositions(Condition.Target.C, Condition.Target.B);
        defaultCondition.SetPlayerPositions(Condition.Target.A, Condition.Target.D);
        defaultCondition.SetTurretBehavior(TurretBehavior.TurretState.None);
        defaultCondition.SetOpenDoors(new Condition.Door[] { Condition.Door.C, Condition.Door.B, Condition.Door.D, Condition.Door.A });
        
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("ERROR: can't find player object!");
            return;
        }

        DontDestroyOnLoad(player);
        DontDestroyOnLoad(this);

        HeadTriggerEventScript ht = player.GetComponentInChildren<HeadTriggerEventScript>();
        if (ht != null)
        {
            ht.OnHeadTriggered += HeadTriggered;
        }

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        SetupConditions();

        SceneManager.LoadScene(1);
    }

    private void SetupConditions()
    {
        conditions = new Queue<Condition>();

        Condition c1 = new Condition();
        c1.SetTargetType(Condition.TargetType.Robot);
        c1.SetBodyColor(ColorChangeBehavior.Red);
        c1.SetUnderglow(Color.red, Condition.UnderglowPattern.Off);
        c1.SetSpeed(Condition.SpeedSlow);
        c1.SetHallPosition(Condition.PositionA);
        c1.SetRobotPositions(Condition.Target.D, Condition.Target.B);
        c1.SetPlayerPositions(Condition.Target.A, Condition.Target.D);
        c1.SetTurretBehavior(TurretBehavior.TurretState.None);
        c1.SetOpenDoors(new Condition.Door[] { Condition.Door.C, Condition.Door.B, Condition.Door.D, Condition.Door.A });
        conditions.Enqueue(c1);

        Condition c2 = new Condition();
        c2.SetTargetType(Condition.TargetType.Robot);
        c2.SetBodyColor(ColorChangeBehavior.Neutral);
        c2.SetUnderglow(Color.red, Condition.UnderglowPattern.Off);
        c2.SetSpeed(Condition.SpeedSlow);
        c2.SetHallPosition(Condition.PositionB);
        c2.SetRobotPositions(Condition.Target.B, Condition.Target.D);
        c2.SetPlayerPositions(Condition.Target.D, Condition.Target.A);
        c2.SetTurretBehavior(TurretBehavior.TurretState.None);
        c2.SetOpenDoors(new Condition.Door[] { Condition.Door.B, Condition.Door.D, Condition.Door.A });
        conditions.Enqueue(c2);

        Condition c3 = new Condition();
        c3.SetTargetType(Condition.TargetType.Robot);
        c3.SetBodyColor(ColorChangeBehavior.Black);
        c3.SetUnderglow(Color.red, Condition.UnderglowPattern.Off);
        c3.SetSpeed(Condition.SpeedSlow);
        c3.SetHallPosition(Condition.PositionC);
        c3.SetRobotPositions(Condition.Target.D, Condition.Target.B);
        c3.SetPlayerPositions(Condition.Target.A, Condition.Target.C);
        c3.SetTurretBehavior(TurretBehavior.TurretState.None);
        c3.SetOpenDoors(new Condition.Door[] { Condition.Door.D, Condition.Door.B, Condition.Door.C });
        conditions.Enqueue(c3);

        Condition c4 = new Condition();
        c4.SetTargetType(Condition.TargetType.Robot);
        c4.SetBodyColor(ColorChangeBehavior.Yellow);
        c4.SetUnderglow(Color.red, Condition.UnderglowPattern.Off);
        c4.SetSpeed(Condition.SpeedSlow);
        c4.SetHallPosition(Condition.PositionD);
        c4.SetRobotPositions(Condition.Target.A, Condition.Target.C);
        c4.SetPlayerPositions(Condition.Target.C, Condition.Target.A);
        c4.SetTurretBehavior(TurretBehavior.TurretState.None);
        c4.SetOpenDoors(new Condition.Door[] { Condition.Door.C, Condition.Door.A });
        conditions.Enqueue(c4);

        Condition c5 = new Condition();
        c5.SetTargetType(Condition.TargetType.Robot);
        c5.SetBodyColor(ColorChangeBehavior.Green);
        c5.SetUnderglow(Color.red, Condition.UnderglowPattern.Off);
        c5.SetSpeed(Condition.SpeedSlow);
        c5.SetHallPosition(Condition.PositionE);
        c5.SetRobotPositions(Condition.Target.D, Condition.Target.A);
        c5.SetPlayerPositions(Condition.Target.A, Condition.Target.D);
        c5.SetTurretBehavior(TurretBehavior.TurretState.None);
        c5.SetOpenDoors(new Condition.Door[] { Condition.Door.C, Condition.Door.D, Condition.Door.A });
        conditions.Enqueue(c5);
    }

    void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        Debug.Log("Scene loaded");
        switch (scene.buildIndex)
        {
            case 1: //hallway
                //Debug.Log("Hallway");
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

                HallwayController hallControl = GameObject.FindGameObjectWithTag("HallwayController").GetComponent<HallwayController>();
                if(conditions.Count > 0)
                {
                    hallControl.SetupCondition(conditions.Dequeue());
                } else
                {
                    //Debug.Log("Loading default condition!");
                    hallControl.SetupCondition(defaultCondition);
                }
      
                robot.GetComponent<TurretBehavior>().SetProximateObject(player.transform.GetChild(0).gameObject);

                break;
        }
    }

    void HeadTriggered(string tag)
    {
        //Debug.Log("head triggered:" + tag);
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1: //hallway
                if (tag.Equals("Target"))
                {
                    SceneManager.LoadScene(1);
                }
                break;
        }
    }

    public class Condition : UnityEngine.Object
    {
        public static float PositionA = -0.77f;
        public static float PositionB = -0.61f;
        public static float PositionC = 0;
        public static float PositionD = 0.61f;
        public static float PositionE = 0.77f;

        public enum Target
        {
            A, B, C, D, None
        }

        public enum Door
        {
            A, B, C, D, AB, CD
        }

        public enum UnderglowPattern
        {
            Flash, Breathe, On, Off
        }

        public enum TargetType
        {
            Robot, Human, Object
        }

        public static float SpeedFast = 1.4f;
        public static float SpeedMedium = 1.23f;
        public static float SpeedSlow = 0.5f;

        public TargetType targetType;
        public Color bodyColor;
        public Color underglowColor;
        public UnderglowPattern underglowPattern;
        public float speed;
        public Target robotStartTarget;
        public Target robotGoalTarget;
        public Target playerStartTarget;
        public Target playerGoalTarget;
        public float hallwayPosition;
        public TurretBehavior.TurretState turretBehavior;
        public Door[] openDoors;

        public Condition() : this(TargetType.Robot, ColorChangeBehavior.Black, Color.black, UnderglowPattern.Off, 0.0f, Target.A, Target.B, Target.C, Target.D, PositionC, TurretBehavior.TurretState.None, null)
        { }

        public Condition(TargetType targetType, Color bodyColor, Color underglowColor, UnderglowPattern pattern, float speed, Target robotStartPosition, Target robotGoalPosition, Target playerStartPosition, Target playerGoalPosition, float hallwayPosition, TurretBehavior.TurretState turretBehavior, Door[] openDoors)
        {
            this.targetType = targetType;
            this.bodyColor = bodyColor;
            this.underglowColor = underglowColor;
            this.underglowPattern = pattern;
            this.speed = speed;
            this.robotStartTarget = robotStartPosition;
            this.robotGoalTarget = robotGoalPosition;
            this.playerStartTarget = playerStartPosition;
            this.playerGoalTarget = playerGoalPosition;
            this.hallwayPosition = hallwayPosition;
            this.turretBehavior = turretBehavior;
            this.openDoors = openDoors;
        }

        public void SetTargetType(TargetType t)
        {
            this.targetType = t;
        }

        public void SetBodyColor(Color c)
        {
            this.bodyColor = c;
        }

        public void SetUnderglow(Color c, UnderglowPattern p)
        {
            this.underglowColor = c;
            this.underglowPattern = p;
        }

        public void SetSpeed(float s)
        {
            this.speed = s;
        }

        public void SetHallPosition(float p)
        {
            this.hallwayPosition = p;
        }

        public void SetRobotPositions(Target start, Target end)
        {
            this.robotStartTarget = start;
            this.robotGoalTarget = end;
        }

        public void SetPlayerPositions(Target start, Target end)
        {
            this.playerStartTarget = start;
            this.playerGoalTarget = end;
        }

        public void SetTurretBehavior(TurretBehavior.TurretState t)
        {
            this.turretBehavior = t;
        }

        public void SetOpenDoors(Door[] d)
        {
            this.openDoors = d;
        }
    }
}
