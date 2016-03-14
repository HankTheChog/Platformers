using UnityEngine;
using System.Collections;

public abstract class BasicPlayer : MonoBehaviour {


    public enum PlayerType { RED, BLUE, NUM_OF_PLAYERS };

    public LayerMask can_jump_off;

    [HideInInspector]
    public PlayerType WhoAmI;


    protected Rigidbody2D rb;
    protected Transform body;
    protected GameObject ground_check;
    protected Animator anim;

    // Jump related
    protected bool jump_button_pressed = false;
    protected bool jump_button_was_pressed = false;
    protected bool jumping = false;
    protected bool grounded = true;

    // Use this for initialization
    public void BasicPlayerStart() {
        rb = GetComponent<Rigidbody2D>();
        body = transform.GetChild(0);
        anim = body.GetComponent<Animator>();
        foreach(Transform childT in transform)
        {
            if (childT.gameObject.name == "GroundCheck")
                ground_check = childT.gameObject;
        }
    }

    public void handle_jumping(float v)
    {
        jump_button_pressed = (v == 1);

        if (jump_button_pressed && !jump_button_was_pressed && grounded && !jumping)
        {
            StartCoroutine(Jump());
        }
        jump_button_was_pressed = jump_button_pressed;
    }

    public void handle_walking(float h)
    {
        float accel, max_speed;

        grounded = ground_check.GetComponent<GroundCheckScript>().IsGrounded();

        if (grounded)
        {
            accel = GameParameters.walking_accel;
            max_speed = GameParameters.max_walk_speed;
        }
        else
        {
            accel = GameParameters.air_travel_accel;
            max_speed = GameParameters.max_air_horizontal_speed;
        }

        if (h * rb.velocity.x < max_speed)
            rb.AddForce(Vector2.right * h * accel);

        if (Mathf.Abs(rb.velocity.x) > max_speed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * max_speed, rb.velocity.y);
    }

    public void change_sprite_to_match_walk_direction(bool facing_right)
    {
        var scale = transform.localScale;
        scale.x = facing_right ? 1 : -1;
        transform.localScale = scale;
    }

    public void HitSpikes()
    {
        Destroy(this.gameObject);
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


    public abstract void EnteredAntiMagnet();

    public abstract void LeavingAntiMagnet();

    public virtual void TurnedIntoHuman() { }
    public virtual void TurnedIntoPlatform() { }

    public void OnDrawGizmos()
    {
        float max_jump_dist = GameParameters.max_jump_time * 2 * GameParameters.max_walk_speed;

        body = transform.GetChild(0);
        Vector3 origin_of_lines = transform.position;
        //origin_of_lines.y -= body.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y; // move origin to bottom of player -- BUG. sprite is too big

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin_of_lines, GameParameters.magnet_radius);

        Gizmos.color = Color.white;

        Gizmos.DrawLine(origin_of_lines, origin_of_lines + Vector3.right * max_jump_dist);
        Gizmos.DrawLine(origin_of_lines, origin_of_lines + Vector3.left * max_jump_dist);

        Gizmos.DrawLine(origin_of_lines, origin_of_lines + Vector3.up * GameParameters.max_jump_height);

        Vector3 half_point_right = origin_of_lines + Vector3.right * max_jump_dist / 2;
        Vector3 half_point_left = origin_of_lines + Vector3.left * max_jump_dist / 2;
        Gizmos.DrawLine(half_point_right, half_point_right + Vector3.up * GameParameters.max_jump_height);
        Gizmos.DrawLine(half_point_left, half_point_left + Vector3.up * GameParameters.max_jump_height);
    }

}
