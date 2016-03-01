using UnityEngine;
using System.Collections;

public class GameParameters : MonoBehaviour {

    static public float max_walk_speed = 5.0f;
    public float _max_walk_speed
    {
        get { return max_walk_speed; }
        set { max_walk_speed = value; } 
    }

    static public float max_air_horizontal_speed = 5.0f;
    public float _max_air_horizontal_speed
    {
        get { return max_air_horizontal_speed; }
        set { max_air_horizontal_speed = value; }
    }

    static public float walking_accel = 20f;
    public float _walking_accel
    {
        get { return walking_accel; }
        set { walking_accel = value; }
    }

    static public float air_travel_accel = 10f;
    public float _air_travel_accel
    {
        get { return air_travel_accel; }
        set { air_travel_accel = value; }
    }

    static public float max_jump_height = 2f;
    public float _max_jump_height
    {
        get { return max_jump_height; }
        set { max_jump_height = value; }
    }

    static public float max_jump_time = 0.5f; // time to reach ground from top of jump - actual jump time is twice as long !!
    public float _max_jump_time
    {
        get { return max_jump_time; }
        set { max_jump_time = value; }
    }

    static public float min_jump_height = 3f;
    public float _min_jump_height
    {
        get { return min_jump_height; }
        set { min_jump_height = value; }
    }

    static public float magnet_radius = 4f;
    public float _magnet_radius
    {
        get { return magnet_radius; }
        set { magnet_radius = value; }
    }

    static public float time_for_full_magnet_power = 4f;
    public float _time_for_full_magnet_power
    {
        get { return time_for_full_magnet_power; }
        set { time_for_full_magnet_power = value; }
    }

    static public float magnet_cooldown_time = 2f;
    public float _magnet_cooldown_time
    {
        get { return magnet_cooldown_time; }
        set { magnet_cooldown_time = value; }
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
