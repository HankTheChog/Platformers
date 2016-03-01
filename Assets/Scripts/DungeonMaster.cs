using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine.SceneManagement;

public interface IWinCondition
{
    bool WinConditionSatisfied();
}

static public class GlobalWinCondition
{
    static List<IWinCondition> objects;
    static GlobalWinCondition()
    {
        objects = new List<IWinCondition>();
    }
    static public void add(IWinCondition obj)
    {
        objects.Add(obj);
    }
    static public bool IsWinning()
    {
        foreach (var o in objects)
        {
            if (o.WinConditionSatisfied() == false)
            {
                return false;
            }   
        }
        return true;
    }

}

public class DungeonMaster : MonoBehaviour
{
    public Transform UI_prefab;
    private Transform UI;
    public int NextLevel;

    public static bool paused;

    private GameObject red;
    private GameObject blue;

	// Use this for initialization
	void Start () {
        UI = (Transform)Instantiate(UI_prefab);
        red = GameObject.Find("Red player");
        blue = GameObject.Find("Blue player");
        paused = false;
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
        // Update is called once per frame
    void Update () {
        if (red == null || blue == null)
        {
            // restart level
            // todo: pause the game, show some message/fade-out/graphics, then reload
            UI.GetComponent<InGameMenu>().RespawnLevel();
        }

        if (GlobalWinCondition.IsWinning())
        {
            UI.GetComponent<InGameMenu>().MoveToLevel(NextLevel);
        }
	}
}
