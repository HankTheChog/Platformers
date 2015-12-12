using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {

    public GameObject menu;
    public static bool paused;

	// Use this for initialization
	void Start () {
        paused = false;
        menu.SetActive(false);
    }
	
    void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
        menu.SetActive(true);
    }
    void Resume()
    {
        paused = false;
        Time.timeScale = 1f;
        menu.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
	}

    public void Button_ResumeGame()
    {
        Resume();
    }
    public void Button_RestartLevel()
    {
        Resume();
        Application.LoadLevel(Application.loadedLevelName);
    }
    public void Button_Quit()
    {
        Application.Quit();
    }

}
