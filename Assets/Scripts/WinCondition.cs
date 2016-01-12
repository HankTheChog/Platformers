using UnityEngine;
using System.Collections;

public interface IWinCondition
{
    bool WinConditionSatisfied();
}

public class WinCondition : MonoBehaviour, IWinCondition
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (WinConditionSatisfied())
        {
            // do something here, move to next level, show message
        }
	}

    public bool WinConditionSatisfied()
    {
        foreach(Transform child in transform)
        {
            if (!child.GetComponent<IWinCondition>().WinConditionSatisfied())
            {
                return false;
            }
        }
        return true;
    }
}
