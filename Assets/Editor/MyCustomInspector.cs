using UnityEngine;
using UnityEditor;
using System.Collections;

//todo: turn into a custom inspector for GameParameters

[CustomEditor(typeof(GameParameters))]
public class MyCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        GameParameters.max_walk_speed = EditorGUILayout.FloatField("Max walking speed", GameParameters.max_walk_speed);

        GameParameters.max_air_horizontal_speed = EditorGUILayout.FloatField("Max Air Horizontal Speed", GameParameters.max_air_horizontal_speed);
        GameParameters.walking_accel = EditorGUILayout.FloatField("Walking acceleration", GameParameters.walking_accel);
        GameParameters.air_travel_accel = EditorGUILayout.FloatField("Air travel acceleration", GameParameters.air_travel_accel);
        GameParameters.max_jump_height = EditorGUILayout.FloatField("Jump Maximum Height", GameParameters.max_jump_height);
        GameParameters.max_jump_time = EditorGUILayout.FloatField("Time of highest jump", GameParameters.max_jump_time);
        GameParameters.min_jump_height = EditorGUILayout.FloatField("Time of shortest jump", GameParameters.min_jump_height);
        GameParameters.magnet_radius = EditorGUILayout.FloatField("Magnet radius", GameParameters.magnet_radius);
    }
}
