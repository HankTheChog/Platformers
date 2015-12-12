using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {

    public static int current_level = 0;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static int get_cur_level()
    {
        return current_level;
    }
    public static void load_level(int num)
    {
        current_level = num;
        Application.LoadLevel(num);
    }

}
