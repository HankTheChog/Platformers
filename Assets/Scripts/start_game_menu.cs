using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class start_game_menu : MonoBehaviour {

    public GameObject screen_canvas;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void button_start()
    {
        StartCoroutine(FadeToBlackAndRestart(1));
    }

    public void button_quit()
    {
        Application.Quit();
    }

    public IEnumerator FadeToBlackAndRestart(int level)
    {
        float t = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - t < 1)
        {
            screen_canvas.GetComponent<Image>().color = Color.Lerp(Color.white, Color.black, Time.realtimeSinceStartup - t);
            yield return null;
        }
        SceneManager.LoadScene(level);
    }

}
