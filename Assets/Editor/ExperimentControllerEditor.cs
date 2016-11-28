using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ExperimentController))]
public class ExperimentControllerEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.LabelField("Condition Parameters", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Offset from centerline:");
        ExperimentController.Condition.PositionA = EditorGUILayout.FloatField("PositionA", ExperimentController.Condition.PositionA);
        ExperimentController.Condition.PositionB = EditorGUILayout.FloatField("PositionB", ExperimentController.Condition.PositionB);
        ExperimentController.Condition.PositionC = EditorGUILayout.FloatField("PositionC", ExperimentController.Condition.PositionC);
        ExperimentController.Condition.PositionD = EditorGUILayout.FloatField("PositionD", ExperimentController.Condition.PositionD);
        ExperimentController.Condition.PositionE = EditorGUILayout.FloatField("PositionE", ExperimentController.Condition.PositionE);

        EditorGUILayout.LabelField("Speeds:");
        ExperimentController.Condition.SpeedFast = EditorGUILayout.FloatField("SpeedFast", ExperimentController.Condition.SpeedFast);
        ExperimentController.Condition.SpeedMedium = EditorGUILayout.FloatField("SpeedMedium", ExperimentController.Condition.SpeedMedium);
        ExperimentController.Condition.SpeedSlow = EditorGUILayout.FloatField("SpeedSlow", ExperimentController.Condition.SpeedSlow);
    }
}
