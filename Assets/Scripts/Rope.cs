using UnityEngine;
using System.Collections;

public class Rope : MonoBehaviour {

    public GameObject red_player;
    public GameObject blue_player;


    private bool is_active = false;
    private LineRenderer line;
	// Use this for initialization
	void Start () {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	if (Input.GetButtonDown("ToggleRope"))
        {
            is_active = !is_active;
            line.enabled = is_active;
        }
        if (is_active)
        {
            line.SetPosition(0, red_player.transform.position);
            line.SetPosition(1, blue_player.transform.position);
        }
	}
}
