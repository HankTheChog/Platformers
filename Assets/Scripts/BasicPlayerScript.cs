using UnityEngine;
using System.Collections;

public class BasicPlayerScript : MonoBehaviour {


    public enum PlayerType { RED, BLUE };

    public PlayerType InputSource;

    public LayerMask can_walk_on;

    private bool jumping = false;
    private bool grounded = true;
    private bool jump_button_pressed = false;

    private Rigidbody2D rb;

    protected bool is_platform_mode;
    protected string horizontal, vertical, platformize;

    protected Vector2 size_self;
    protected float dist_to_ground;
    protected float initial_jump_speed;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        size_self = GetComponent<Renderer>().bounds.size;
        dist_to_ground = GetComponent<PolygonCollider2D>().bounds.extents.y;
    }

    public void Start()
    {
        if (InputSource == PlayerType.RED)
        {
            horizontal  = "RedHorizontal";
            vertical    = "RedVertical";
            platformize = "RedTransform";
        }
        else
        {
            horizontal  = "BlueHorizontal";
            vertical    = "BlueVertical";
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
        grounded = IsGrounded();

        if (grounded)
        {
            //todo: deceleration should be faster, to allow quick direction changes.
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

    void Update ()
    {
        if (InGameMenu.paused)
        {
            return; // we don't process any input if game is paused
        }

        float v = Input.GetAxisRaw(vertical);
        jump_button_pressed = (v == 1);
        
        if (jump_button_pressed && grounded && !jumping)
        {
            StartCoroutine(Jump());
        }

        if (Input.GetButtonDown(platformize))
        {
            TurnIntoPlatformOrBack();
        }
    }

    private void TurnIntoPlatformOrBack()
    {
        is_platform_mode = !is_platform_mode;

        Vector3 scale = transform.localScale;
        if (is_platform_mode)
        {
            scale.x = 3;
            rb.isKinematic = true;
            gameObject.layer = 8;
        }
        else
        {
            scale.x = 1;
            rb.isKinematic = false;
            gameObject.layer = 9;
        }
        transform.localScale = scale;
    }

    bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, size_self, 0, Vector2.down, dist_to_ground, can_walk_on.value);
    }

    public void OnDrawGizmos()
    {
        float max_jump_dist = GameParameters.max_jump_time * 2 * GameParameters.max_walk_speed;
        
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * max_jump_dist);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left  * max_jump_dist);

        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * GameParameters.max_jump_height);

        Vector3 half_point_right = transform.position + Vector3.right * max_jump_dist / 2;
        Vector3 half_point_left = transform.position + Vector3.left* max_jump_dist / 2;
        Gizmos.DrawLine(half_point_right, half_point_right + Vector3.up * GameParameters.max_jump_height);
        Gizmos.DrawLine(half_point_left, half_point_left + Vector3.up * GameParameters.max_jump_height);
    }

    IEnumerator Jump()
    {
        float timer = 0f;
        jumping = true;
        //rb.AddForce(Vector2.up * initial_jump_speed, ForceMode2D.Impulse);
        Vector2 v = rb.velocity;
        v.y += initial_jump_speed;
        rb.velocity = v;

        while (jump_button_pressed && timer < GameParameters.max_jump_time)
        {
            // todo: should suppress gravity for some time, 
            /*
            float proportion_completed = (timer / max_jump_time);
            Vector2 force_in_this_frame = Vector2.Lerp(Vector2.up*2, Vector2.zero, proportion_completed);
            rb.AddForce(force_in_this_frame, ForceMode2D.Impulse);
            */
            timer += Time.deltaTime;
            yield return null;
        }
        // restore normal gravity
        jumping = false;
    }
}
