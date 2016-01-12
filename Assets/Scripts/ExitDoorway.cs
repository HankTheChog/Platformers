using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitDoorway : MonoBehaviour, IWinCondition
{

    public enum IntendedFor { JUST_RED, JUST_BLUE, EITHER_ONE_BUT_NOT_BOTH, EITHER_ONE_OR_BOTH, BOTH};

    [SerializeField]
    public IntendedFor who_is_this_for = IntendedFor.BOTH;

    private bool[] touching;
    private bool win_condition;
    public string target_level;

	// Use this for initialization
	void Start () {
        int n = (int)Player.PlayerType.NUM_OF_PLAYERS;
        touching = new bool[n];
        for (int i = 0; i < n; i++)
            touching[i] = false;
	}
	
	// Update is called once per frame
	void Update () {
        bool red_touching = touching[(int)Player.PlayerType.RED];
        bool blue_touching = touching[(int)Player.PlayerType.BLUE];
        bool both_touching = red_touching && blue_touching;
        bool only_one_touching = red_touching ^ blue_touching;
        bool one_or_both_touching = red_touching || blue_touching;

        win_condition = false;
        switch (who_is_this_for)
        {
            case IntendedFor.JUST_RED:
                win_condition = red_touching;
                break;
            case IntendedFor.JUST_BLUE:
                win_condition = blue_touching;
                break;
            case IntendedFor.EITHER_ONE_BUT_NOT_BOTH:
                win_condition = only_one_touching;
                break;
            case IntendedFor.EITHER_ONE_OR_BOTH:
                win_condition = one_or_both_touching;
                break;
            case IntendedFor.BOTH:
                win_condition = both_touching;
                break;
        }
	}

    public bool WinConditionSatisfied()
    {
        return win_condition;
    }

    void GoToNextLevel()
    {
        SceneManager.LoadScene(target_level);
    }

    public void PlayerTouching(Player.PlayerType who)
    {
        touching[(int)who] = true;
    }

    public void PlayerLeaving(Player.PlayerType who)
    {
        touching[(int)who] = false;

    }
}
