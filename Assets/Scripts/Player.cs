using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


    public enum PlayerType { RED, BLUE, NUM_OF_PLAYERS };

    public PlayerType WhoAmI;

    public LayerMask can_walk_on;

    

    private Rigidbody2D rb;
    private Transform body;
    private PlayerBodyScript body_script;

    private float initial_jump_speed;
    private bool jump_button_pressed = false;
    private bool jump_button_was_pressed = false;
    private bool jumping = false;
    private bool grounded;
    private bool is_platform_mode = false;
    private string horizontal, vertical, platformize;
    private int original_layer;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        body = transform.GetChild(0);
        body_script = body.GetComponent<PlayerBodyScript>();
        original_layer = gameObject.layer;
    }

    // Use this for initialization
    void Start () {
        if (WhoAmI == PlayerType.RED)
        {
            horizontal = "RedHorizontal";
            vertical = "RedVertical";
            platformize = "RedTransform";
        }
        else
        {
            horizontal = "BlueHorizontal";
            vertical = "BlueVertical";
            platformize = "BlueTransform";
        }
        UpdateJumpParameters();
    }

    public void UpdateJumpParameters()
    {
        //todo: instead of changing global gravity, change local gravity scale
        //      or recalculate every jump (this way it's updated in game mode also)
        // this also changes friction, which is bad !
        Physics2D.gravity = Vector3.down * 2 * GameParameters.max_jump_height / (GameParameters.max_jump_time * GameParameters.max_jump_time);
        initial_jump_speed = -Physics2D.gravity.y * GameParameters.max_jump_time;
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
            if (h * rb.velocity.x < max_air_horizontal_speed)
                rb.AddForce(Vector2.right * h * GameParameters.air_travel_accel);

            if (Mathf.Abs(rb.velocity.x) > max_air_horizontal_speed)
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * max_air_horizontal_speed, rb.velocity.y);
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

        if (jump_button_pressed && !jump_button_was_pressed && grounded && !jumping && !is_platform_mode)
        {
            StartCoroutine(Jump());
        }

        if (Input.GetButtonDown(platformize))
        {
            TurnIntoPlatformOrBack();
        }

        jump_button_was_pressed = jump_button_pressed;
    }

    private void TurnIntoPlatformOrBack()
    {
        is_platform_mode = !is_platform_mode;

        Vector3 scale = body.localScale;
        if (is_platform_mode)
        {
            scale.x = 3; // enlarging the player horizontally
            rb.isKinematic = true; // gravity won't apply if we're kinematic
            gameObject.layer = 8; // this makes the player be in the "platform" layer, so the other player can jump off it
        }
        else
        {
            scale.x = 1;
            rb.isKinematic = false;
            gameObject.layer = original_layer;
        }
        body.localScale = scale;
    }

    public void EnteredAntiMagnet()
    {
        MagneticForce.deactivate();
        if (MagneticForce.IsActive())
        {
            rb.velocity = Vector2.zero;
        }
    }
    public void LeavingAntiMagnet()
    {
        MagneticForce.reactivate();
        MagneticForce.Cooldown();
    }
    public void HitSpikes()
    {
        Destroy(this.gameObject);
    }

    IEnumerator Jump()
    {
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
        Gizmos.DrawWireSphere(transform.position, GameParameters.magnet_radius[GameParameters.magnet_radius.Length-1]);

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
