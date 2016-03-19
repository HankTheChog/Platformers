using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine.SceneManagement;


public class DungeonMaster : MonoBehaviour
{
    public Transform UI_prefab;
    private Transform UI;
    public int NextLevel;

    public static bool paused;
    private static bool level_finished;

    private GameObject red;
    private GameObject blue;
    private bool activated_transition;

    void OnLevelWasLoaded()
    {
        level_finished = false;
        paused = false;
    }

    // Use this for initialization
    void Start () {
        UI = (Transform)Instantiate(UI_prefab);
        red = GameObject.Find("Red player");
        blue = GameObject.Find("Blue player");
        activated_transition = false;
    }

    static public void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
    }

    static public void Resume()
    {
        paused = false;
        Time.timeScale = 1f;
    }

    static public void LevelFinished()
    {
        level_finished = true;
    }

    // Update is called once per frame
    void Update () {
        if (red == null || blue == null)
        {
            // restart level
            // todo: pause the game, show some message/fade-out/graphics, then reload
            UI.GetComponent<InGameMenu>().RespawnLevel();
        }
        if (level_finished && !activated_transition)
        {
            activated_transition = true;
            UI.GetComponent<InGameMenu>().MoveToLevel(NextLevel);
        }
	}
}
