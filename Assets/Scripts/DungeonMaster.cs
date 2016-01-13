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

public class DungeonMaster : MonoBehaviour, IWinCondition
{
    public Transform UI_prefab;
    private Transform UI;
    public string NextLevel;

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
	}
    
    bool IWinCondition.WinConditionSatisfied()
    {
        var instances = from t in Assembly.GetExecutingAssembly().GetTypes()
                        where t.GetInterfaces().Contains(typeof(IWinCondition))
                                 && t.GetConstructor(System.Type.EmptyTypes) != null
                        select System.Activator.CreateInstance(t) as IWinCondition;

        foreach (var instance in instances)
        {
            if (instance.WinConditionSatisfied() == false)
            {
                return false;
            }
        }
        return true;
    }

}
