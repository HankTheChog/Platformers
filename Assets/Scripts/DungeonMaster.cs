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

    public string NextLevel;

    private GameObject red;
    private GameObject blue;
	// Use this for initialization
	void Start () {
        SceneManager.LoadScene("ui", LoadSceneMode.Additive);
        red = GameObject.Find("Red player");
        blue = GameObject.Find("Blue player");
    }

    // Update is called once per frame
    void Update () {
        if (red == null || blue == null)
        {
            // restart level
            // todo: pause the game, show some message/fade-out/graphics, then reload
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
