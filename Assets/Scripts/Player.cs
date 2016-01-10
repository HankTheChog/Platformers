using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


    public enum PlayerType { RED, BLUE, NUM_OF_PLAYERS };

    public PlayerType WhoAmI;

    public LayerMask can_jump_off;
    

    private Rigidbody2D rb;
    private Transform body;
    private PlayerBodyScript body_script;
    private GameObject other_player;
    private Player other_player_script;
    private Rigidbody2D other_player_rb;

    //   Magnet related
    private float time_for_full_magnet_power = 4f;
    private bool I_am_being_pulled_by_magnet;
    private bool I_am_pulling_with_magnet;
    private bool in_anti_magnet_field;
    private float start_time_for_magnet;
    private bool on_cooldown = false;
    private float magnet_cooldown_time = 2f;

    // Jump related
    private bool jump_button_pressed = false;
    private bool jump_button_was_pressed = false;
    private bool jumping = false;
    private bool grounded;

    // Platform mode
    private bool in_platform_mode = false;

    // Input buttons
    private string horizontal, vertical, platformize, magnet_button;

    // Use this for initialization
    void Start () {

        rb = GetComponent<Rigidbody2D>();
        body = transform.GetChild(0);
        body_script = body.GetComponent<PlayerBodyScript>();

        if (WhoAmI == PlayerType.RED)
        {
            horizontal = "RedHorizontal";
            vertical = "RedVertical";
            platformize = "RedTransform";
            magnet_button = "RedMagnet";
            other_player = GameObject.Find("Blue player");
        }
        else
        {
            horizontal = "BlueHorizontal";
            vertical = "BlueVertical";
            platformize = "BlueTransform";
            magnet_button = "BlueMagnet";
            other_player = GameObject.Find("Red player");
        }
        transform.GetChild(1).GetComponent<Aura>().SetMagnetButton(magnet_button);
        other_player_script = other_player.GetComponent<Player>();
        other_player_rb = other_player.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float max_walk_speed = GameParameters.max_walk_speed;
        float max_air_horizontal_speed = GameParameters.max_air_horizontal_speed;

        float h = Input.GetAxisRaw(horizontal);
        grounded = body_script.IsGrounded();

        if (grounded)
        {
            if (h * rb.velocity.x < max_walk_speed)
                rb.AddForce(Vector2.right * h * GameParameters.walking_accel);

            if (Mathf.Abs(rb.velocity.x) > max_walk_speed)
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * max_walk_speed, rb.velocity.y);
        }
        else
        {
            if (!I_am_being_pulled_by_magnet)
            {
                if (h * rb.velocity.x < max_air_horizontal_speed)
                    rb.AddForce(Vector2.right * h * GameParameters.air_travel_accel);

                if (Mathf.Abs(rb.velocity.x) > max_air_horizontal_speed)
                    rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * max_air_horizontal_speed, rb.velocity.y);
            }
        }

        if (I_am_pulling_with_magnet && MagnetAllowed())
        {
            Debug.Log("Applying force");
            Vector3 from_him_to_me = (transform.position - other_player.transform.position);
            float distance = from_him_to_me.magnitude;

            float start_force = Physics2D.gravity.magnitude;
            float final_force = start_force * 1.4f;

            float time_since_activation = (Time.time - start_time_for_magnet);
            float force_now = Mathf.Lerp(start_force, final_force, time_since_activation / time_for_full_magnet_power);

            other_player_rb.AddForce(from_him_to_me.normalized * force_now);
        }
    }

    // Update is called once per frame
    void Update () {
        if (InGameMenu.paused)
        {
            return; // we don't process any input if game is paused
        }

        float v = Input.GetAxisRaw(vertical);
        jump_button_pressed = (v == 1);

        if (jump_button_pressed && !jump_button_was_pressed && grounded && !jumping && !in_platform_mode)
        {
            StartCoroutine(Jump());
        }

        bool magnet_button_state = Input.GetButton(magnet_button);
        // If starting to pull

        if (I_am_pulling_with_magnet == false && magnet_button_state && MagnetAllowed())
        {
            I_am_pulling_with_magnet = true;
            other_player_script.NotifyAboutMagnet(true);
            start_time_for_magnet = Time.time;
        }
        // If pulling, and letting go of the key (or have to cancel magnet for some reason)
        if (I_am_pulling_with_magnet && (magnet_button_state == false || MagnetAllowed()==false))
        {
            I_am_pulling_with_magnet = false;
            other_player_script.NotifyAboutMagnet(false);
        }

        if (Input.GetButtonDown(platformize))
        {
            TurnIntoPlatformOrBack();
        }

        jump_button_was_pressed = jump_button_pressed;
    }

    private void TurnIntoPlatformOrBack()
    {
        in_platform_mode = !in_platform_mode;

        Vector3 scale = body.localScale;
        if (in_platform_mode)
        {
            scale.x = 3; // enlarging the player horizontally
            rb.isKinematic = true; // gravity won't apply if we're kinematic
        }
        else
        {
            scale.x = 1;
            rb.isKinematic = false;
        }
        body.localScale = scale;
    }

    public void EnteredAntiMagnet()
    {
        in_anti_magnet_field = true;
        I_am_pulling_with_magnet = false;
        if (I_am_being_pulled_by_magnet)
        {
            rb.velocity = Vector2.zero;
        }
        I_am_being_pulled_by_magnet = false;
    }
    public void LeavingAntiMagnet()
    {
        in_anti_magnet_field = false;
        StartCoroutine(CooldownMagnet());
    }
    public bool CanBeAffectedByMagnet()
    {
        // I can also return true based on WhoAmI
        return !in_anti_magnet_field && !in_platform_mode;
    }
    public bool CanTriggerMagnet()
    {
        return !in_anti_magnet_field && !on_cooldown;
    }
    public bool MagnetAllowed()
    {
        if (other_player)
        {
            float distance = (transform.position - other_player.transform.position).magnitude;
            return (CanTriggerMagnet() && other_player_script.CanBeAffectedByMagnet() && distance < GameParameters.magnet_radius);
        }
        // else, other player is dead... :-(
        return false;
    }
    public void NotifyAboutMagnet(bool state)
    {
        I_am_being_pulled_by_magnet = state;
    }




    public void HitSpikes()
    {
        Destroy(this.gameObject);
    }

    IEnumerator CooldownMagnet()
    {
        float end_time = Time.time + magnet_cooldown_time;
        on_cooldown = true;
        while (Time.time < end_time)
        {
            yield return null;
        }
        on_cooldown = false;
    }


    /*
    To adjust for variable height jumps:
    a variable height jump is 2 parts
    1) we give player initial speed V1, keep it constant for T1 time (as long as jump key is pressed)
    2) we stop pushing the player, letting gravity take over.

    V1 is calculated to get us to minimum desired jump height.
    Gravity is calculated to get us from H' (smallest jump) to H'' (highest jump).
    
    In this format we don't get to set the fall time.

    How to do phase 1.
    easiest solution, keep the speed constant.
    more fluid solution, apply initial impulse force (enough to get us to H'), and while we're not in H' (and jump key is pressed)
    we keep applying force (which should diminish, use Lerp).
    In this case, the initial impulse force can easily be calculated assuming it should get us to H'.
    from physics:
        Total_Force * Total_time = m * delta_Velocity

    total_force = integral (T1) { initial_force * t'/T1 dt } = initial_force * T1 / 2
    initial_force = enough to get us to H'
    delta_velocity = velocity we need in H' to carry us to H'' under given gravity.
    total_time = the time for applying the force can be calculated from above.
    */
    IEnumerator Jump()
    {
        //todo: instead of changing global gravity, change local gravity scale
        //      or recalculate every jump (this way it's updated in game mode also)
        // this also changes friction, which is bad !
        Physics2D.gravity = Vector3.down * 2 * GameParameters.max_jump_height / (GameParameters.max_jump_time * GameParameters.max_jump_time);
        float initial_jump_speed = -Physics2D.gravity.y * GameParameters.max_jump_time; // this is the speed needed to get to max_jump_height

        float timer = 0f;
        jumping = true;
        Vector2 v = rb.velocity;
        v.y += initial_jump_speed;
        rb.velocity = v;

        while (jump_button_pressed && timer < GameParameters.max_jump_time)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        jumping = false;
    }



    public void OnDrawGizmos()
    {
        float max_jump_dist = GameParameters.max_jump_time * 2 * GameParameters.max_walk_speed;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, GameParameters.magnet_radius);

        Gizmos.color = Color.white;

        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * max_jump_dist);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * max_jump_dist);

        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * GameParameters.max_jump_height);

        Vector3 half_point_right = transform.position + Vector3.right * max_jump_dist / 2;
        Vector3 half_point_left = transform.position + Vector3.left * max_jump_dist / 2;
        Gizmos.DrawLine(half_point_right, half_point_right + Vector3.up * GameParameters.max_jump_height);
        Gizmos.DrawLine(half_point_left, half_point_left + Vector3.up * GameParameters.max_jump_height);
    }

}
