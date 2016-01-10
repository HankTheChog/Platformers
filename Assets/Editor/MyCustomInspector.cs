using UnityEngine;
using UnityEditor;
using System.Collections;

//todo: turn into a custom inspector for P

[CustomEditor(typeof(GameParameters))]
public class MyCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        GameParameters P = (GameParameters)target;

        P._max_walk_speed = EditorGUILayout.FloatField("Max walking speed", P._max_walk_speed);
        P._walking_accel = EditorGUILayout.FloatField("Walking acceleration", P._walking_accel);

        P._max_air_horizontal_speed = EditorGUILayout.FloatField("Max Air Horizontal Speed", P._max_air_horizontal_speed);
        P._air_travel_accel = EditorGUILayout.FloatField("Air travel acceleration", P._air_travel_accel);

        EditorGUILayout.Space();

        P._max_jump_height = EditorGUILayout.FloatField("Jump Maximum Height", P._max_jump_height);
        P._max_jump_time = EditorGUILayout.FloatField("Time of highest jump", P._max_jump_time);
        P._min_jump_height = EditorGUILayout.FloatField("Time of shortest jump", P._min_jump_height);

        EditorGUILayout.Space();

        P._magnet_radius = EditorGUILayout.FloatField("Magnet radius", P._magnet_radius);
        P._magnet_cooldown_time = EditorGUILayout.FloatField("Magnet radius", P._magnet_cooldown_time);
        P._time_for_full_magnet_power = EditorGUILayout.FloatField("Magnet radius", P._time_for_full_magnet_power);
    }
}
