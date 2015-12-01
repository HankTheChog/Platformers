using UnityEngine;
using System.Collections;

public class globals : MonoBehaviour {

    public static int current_level;
    public static string[] levels = { "testing_ground" };

	// Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
	void Start () {
        current_level = 1;
        Application.LoadLevel(levels[current_level-1]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
