using UnityEngine;
using System.Collections;

public class ExitDoor : MonoBehaviour {

    private bool red = false;
    private bool blue = false;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "RedPlayer")
            red = true;
        if (collider.gameObject.name == "BluePlayer")
            blue = true;
        if (red && blue)
        {
            globals.current_level++;
            if (globals.current_level < globals.levels.Length)
            {
                Application.LoadLevel(globals.levels[globals.current_level]);
            }
            else
            {
                // show game over screen
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "RedPlayer")
            red = false;
        if (collider.gameObject.name == "BluePlayer")
            blue = false;
    }
}
