using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitDoorway : MonoBehaviour {

    [SerializeField]
    public int win_condition = 2;

    private bool[] touching;
    public string target_level;

	// Use this for initialization
	void Start () {
        int n = (int)BasicPlayerScript.PlayerType.NUM_OF_PLAYERS;
        touching = new bool[n];
        for (int i = 0; i < n; i++)
            touching[i] = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (win_condition == 2 && touching[0] && touching[1])
            GoToNextLevel();

        if (win_condition == 1 && (touching[0] || touching[1]))
            GoToNextLevel();
	}

    void GoToNextLevel()
    {
        SceneManager.LoadScene(target_level);
    }

    public void PlayerTouching(BasicPlayerScript.PlayerType who)
    {
        touching[(int)who] = true;
    }

    public void PlayerLeaving(BasicPlayerScript.PlayerType who)
    {
        touching[(int)who] = false;

    }
}
