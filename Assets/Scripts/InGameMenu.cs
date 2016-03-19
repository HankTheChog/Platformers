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
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play(); // why doesn't this work inside the coroutine?

        float start_t = Time.realtimeSinceStartup;
        float end_t = GetComponent<AudioSource>().clip.length + start_t;
        while (Time.realtimeSinceStartup < end_t)
        {
            screen_canvas.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.black, Time.realtimeSinceStartup - start_t);
            yield return null;
        }
        DungeonMaster.Resume();
        SceneManager.LoadScene(level);
    }

}
