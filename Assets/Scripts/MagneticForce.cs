using UnityEngine;
using System.Collections;

public class MagneticForce : MonoBehaviour {

    [SerializeField]
    public float force_constant = 10.0f;

    [SerializeField]
    public float cooldown_time = 2f;

    private static bool deactivated = false;
    private static bool on_cooldown = false;
    private static bool magnet_button_pressed = false;
    private static float start_cooldown_time;

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

            if (distance < GameParameters.magnet_radius)
            {
                Vector3 force_on_red = force_constant * red_to_blue.normalized;

                red.GetComponent<Rigidbody2D>().AddForce(force_on_red);
                blue.GetComponent<Rigidbody2D>().AddForce(-force_on_red);
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
        magnet_button_pressed = Input.GetButton("ToggleRope");
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
