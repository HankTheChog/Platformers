using UnityEngine;
using System.Collections;

public class MagneticForce : MonoBehaviour {

    [SerializeField]
    public float force_constant = 10.0f;

    [SerializeField]
    public float cooldown_time = 2f;

    public float time_for_full_power = 4f;

    private static bool deactivated = false;
    private static bool on_cooldown = false;
    private static bool magnet_button_pressed = false;
    private static float start_cooldown_time;
    private static float activated_magnet_time;

    private GameObject red;
    private GameObject blue;

    // Use this for initialization
    void Start () {
        red = GameObject.Find("Red player");
        blue = GameObject.Find("Blue player");
    }

    void FixedUpdate()
    {
        if ((Time.time - start_cooldown_time) > cooldown_time)
        {
            on_cooldown = false;
        }
        if (magnet_button_pressed && !on_cooldown && !deactivated)
        {
            Vector3 red_to_blue = (blue.transform.position - red.transform.position);
            float distance = red_to_blue.magnitude;

            Vector3 start_force_on_red = Physics2D.gravity.magnitude * red_to_blue.normalized;
            Vector3 final_force_on_red = start_force_on_red * 1.4f;

            float time_since_activation = (Time.time - activated_magnet_time);
            Vector3 final_force = Vector3.Lerp(start_force_on_red, final_force_on_red, time_since_activation / time_for_full_power);
            // we add the magnet force X times, where X is the number of radius the players are in.
            /*
            
            int n = 0;
            foreach (float radius in GameParameters.magnet_radius)
            {
                if (distance < radius)
                {
                    n++;
                }
            }
            
            red.GetComponent<Rigidbody2D>().AddForce(force_on_red * n);
            blue.GetComponent<Rigidbody2D>().AddForce(-force_on_red * n);
            */
            if (distance < GameParameters.magnet_radius[1])
            {
                red.GetComponent<Rigidbody2D>().AddForce(final_force_on_red);
                blue.GetComponent<Rigidbody2D>().AddForce(-final_force_on_red);
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
        if (!magnet_button_pressed && button_state)
        {
            activated_magnet_time = Time.time;

            Vector3 red_to_blue = (blue.transform.position - red.transform.position);
            float distance = red_to_blue.magnitude;
            if (distance < GameParameters.magnet_radius[1])
            {
                red.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                blue.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        };
        magnet_button_pressed = button_state;
    }

    public static bool IsActive()
    {
        return magnet_button_pressed;
    }
    public static void deactivate()
    {
        deactivated = true;
    }
    public static void reactivate()
    {
        deactivated = false;
    }
    public static void Cooldown()
    {
        on_cooldown = true;
        start_cooldown_time = Time.time;
    }

}
