using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ui_loader : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneManager.LoadScene("ui", LoadSceneMode.Additive);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
