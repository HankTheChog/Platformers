using UnityEngine;
using System.Collections;

public class MagneticForce : MonoBehaviour {

    [SerializeField]
    public float force_constant = 10.0f;


    [SerializeField]
    private static bool on_cooldown = false;

    [SerializeField]
    public static float cooldown_time = 2f;

    [SerializeField]
    private GameObject red;

    [SerializeField]
    private GameObject blue;

    private static bool active = false;
    private static float start_cooldown_time;

    private Vector3 red_size, blue_size;
    private float minimal_distance_to_active;

    // Use this for initialization
    void Start () {
        red_size = red.gameObject.GetComponent<Renderer>().bounds.size;
        blue_size = red.gameObject.GetComponent<Renderer>().bounds.size;
        minimal_distance_to_active = (red_size.x / 2 + blue_size.x / 2);
    }

    void FixedUpdate()
    {
        if ((Time.time - start_cooldown_time) > cooldown_time)
        {
            on_cooldown = false;
        }
        if (active && !on_cooldown)
        {
            Vector3 red_to_blue = (blue.transform.position - red.transform.position);
            float distance = red_to_blue.magnitude;
            if (distance < GameParameters.magnet_radius)
            {
                //if (distance > minimal_distance_to_active)
                {
                    Vector3 force_on_red = force_constant * red_to_blue.normalized;
                    //  / distance_sqr

                    red.GetComponent<Rigidbody2D>().AddForce(force_on_red);
                    blue.GetComponent<Rigidbody2D>().AddForce(-force_on_red);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InGameMenu.paused)
        {
            return; // we don't process any input if game is paused
        }
        bool button_state = Input.GetButton("ToggleRope");
        if (button_state != active)
        {
            active = button_state;
        }
    }

    public static bool IsActive()
    {
        return active;
    }

    public static void Cooldown()
    {
        on_cooldown = true;
        start_cooldown_time = Time.time;
    }

}
