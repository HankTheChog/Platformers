using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour {

    public GameObject menu;
    public GameObject screen_canvas;


    private bool menu_is_shown;

	// Use this for initialization
	void Start () {
        EnableOrDisableMenu(false);
        screen_canvas.GetComponent<Image>().color = Color.clear;
    }

    void EnableOrDisableMenu(bool menu_is_on)
    {
        menu_is_shown = menu_is_on;
        menu.SetActive(menu_is_shown);
        if (menu_is_shown)
            DungeonMaster.Pause();
        else
            DungeonMaster.Resume();
    }
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnableOrDisableMenu(!menu_is_shown);
        }
	}

    public void Button_ResumeGame()
    {
        EnableOrDisableMenu(false);
    }

    public void Button_RestartLevel()
    {
        EnableOrDisableMenu(false);
        StartCoroutine(FadeToBlackAndRestart(SceneManager.GetActiveScene().buildIndex));
   //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Button_Quit()
    {
        Application.Quit();
    }

    public void RespawnLevel()
    {
        StartCoroutine(FadeToBlackAndRestart(SceneManager.GetActiveScene().buildIndex));
        DungeonMaster.Pause();
    }

    public void MoveToLevel(int level)
    {
        StartCoroutine(FadeToBlackAndRestart(level));
        DungeonMaster.Pause();
    }

    public IEnumerator FadeToBlackAndRestart(int level)
    {
        float t = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - t < 1)
        {
            screen_canvas.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.black, Time.realtimeSinceStartup - t);
            yield return null;
        }
        DungeonMaster.Resume();
        SceneManager.LoadScene(level);
    }

}
