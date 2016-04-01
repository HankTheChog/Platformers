using UnityEngine;
using System.Collections;

public class ExitGate : MonoBehaviour
{
    public enum IntendedFor { JUST_RED, JUST_BLUE, EITHER_ONE_BUT_NOT_BOTH, EITHER_ONE_OR_BOTH, BOTH };

    public IntendedFor who_is_this_for = IntendedFor.BOTH;

    private bool[] touching;


    // Use this for initialization
    void Start () {
        int n = (int)BasicPlayer.PlayerType.NUM_OF_PLAYERS;
        touching = new bool[n];
        for (int i = 0; i < n; i++)
            touching[i] = false;
    }
	
	// Update is called once per frame
	void Update () {
        bool red_touching = touching[(int)BasicPlayer.PlayerType.RED];
        bool blue_touching = touching[(int)BasicPlayer.PlayerType.BLUE];
        bool both_touching = red_touching && blue_touching;
        bool only_one_touching = red_touching ^ blue_touching;
        bool one_or_both_touching = red_touching || blue_touching;

        bool win_condition = false;
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

        if (win_condition)
        {
            DungeonMaster.LevelFinished();
        }
    }

    public void PlayerTouching(BasicPlayer.PlayerType who)
    {
        touching[(int)who] = true;
    }

    public void PlayerLeaving(BasicPlayer.PlayerType who)
    {
        touching[(int)who] = false;
    }
}
