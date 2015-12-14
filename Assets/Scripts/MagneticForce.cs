using UnityEngine;
using System.Collections;

public class MagneticForce : MonoBehaviour {

    [SerializeField]
    public float force_constant = 10.0f;

    public float reduce_player_friction_when_magnet_is_on = 1.0f; // 1 has effect, values larger than 1 reduce friction

    [SerializeField]
    private static bool active = false;

    [SerializeField]
    private GameObject red;

    [SerializeField]
    private GameObject blue;

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
        if (active)
        {
            Vector3 red_to_blue = (blue.transform.position - red.transform.position);
            float distance_sqr = red_to_blue.sqrMagnitude;
            if (distance_sqr > minimal_distance_to_active)
            {
                Vector3 force_on_red = force_constant * red_to_blue.normalized;
                //  / distance_sqr

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
        bool button_state = Input.GetButton("ToggleRope");
        if (button_state != active)
        {
            if (button_state)
            {
                // since both players share the physics material, reducing for one of them is enough
                red.GetComponent<PolygonCollider2D>().sharedMaterial.friction /= reduce_player_friction_when_magnet_is_on;
            }
            else
            {
                //TODO: save previous values and restore. repeated division and multiplication cause errors
                red.GetComponent<PolygonCollider2D>().sharedMaterial.friction *= reduce_player_friction_when_magnet_is_on;
            }
            active = button_state;
        }
    }

    public static bool IsActive()
    {
        return active;
    }
}
