using UnityEngine;
using System.Collections;

public class GameParameters : MonoBehaviour {

    static public float max_walk_speed = 5.0f;
    static public float max_air_horizontal_speed = 5.0f;

    static public float walking_accel = 10f;
    static public float air_travel_accel = 10f;

    static public float max_jump_height = 2f;
    static public float max_jump_time = 0.5f; // time to reach ground from top of jump - actual jump time is twice as long !!

    static public float min_jump_height = 3f;

    static public float magnet_radius = 4f;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
