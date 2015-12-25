using UnityEngine;
using UnityEditor;
using System.Collections;

//todo: turn into a custom inspector for GameParameters

public class MyCustomInspector : EditorWindow
{
    [MenuItem("Custom/GameParameters")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MyCustomInspector window = new MyCustomInspector();
        window.autoRepaintOnSceneChange = true;
        window.Show();
    }

    public void OnEnable()
    {
    }

    public void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label(" Set game parameters here");
        GUILayout.Label("Max walk speed");
        GameParameters.max_walk_speed = GUILayout.HorizontalSlider(GameParameters.max_walk_speed, 0, 100);
        GUILayout.EndVertical();
    }
}
