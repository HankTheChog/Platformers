using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {

    public static int current_level = 1;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(transform.gameObject);
        Application.LoadLevel(current_level);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
