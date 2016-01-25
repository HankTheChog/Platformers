using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

public class screenshot : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if (Input.GetKeyDown(KeyCode.K))
        {
            Application.CaptureScreenshot("Screenshot_level_" + EditorSceneManager.GetActiveScene().buildIndex);
        }
	}
}
