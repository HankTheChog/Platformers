using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGameButton()
    {
        Globals.load_level(1);
    }
    public void StartTutorialButton()
    {
        Globals.load_level(1);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
