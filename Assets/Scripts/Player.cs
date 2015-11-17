using UnityEngine;
using System.Collections;

// problems with movement:
// when landing from jump the character stops. if the player pushes the buttons, we still get a momentary stop. it should be smooth.

// I would like that after running for a while, jumps are longer, and after running for short distance jumps are shorter.
// - I get this effect now, but it's very very weak.



public class Player : MonoBehaviour {

    public float jump_force = 8f;
    public float move_force = 300f;
    public float max_speed = 5f;


    protected string jump_button;
    protected string horizontal_axis;
    protected string vertical_axis;
    protected string transform_button; // transform to platform

    protected bool jump;
    protected bool grounded;
    protected Rigidbody2D rb; // can I just use rigidbody2D instead of this?

    protected static bool rope_active = false;
    protected static float rope_max_distance = 0.0f;
	
    
    // Use this for initialization
    void Start()
    {
    }
    
    // I use this because I can pass parameters (can't with Start)
	public void initialize(string jump_button, string horizontal, string vertical) {
        jump = false;
        grounded = true;
        rb = GetComponent<Rigidbody2D>();
        this.jump_button = jump_button;
        this.horizontal_axis = horizontal;
        this.vertical_axis = vertical;
    }
	
    public void FixedUpdate()
    {
        if (jump)
        {
            rb.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);
            jump = false;
            grounded = false;
        }

        float h = Input.GetAxisRaw(horizontal_axis);
        float v = Input.GetAxisRaw(vertical_axis);
        // movement, while on ground only (not in mid-jump)
        if (grounded)
        {
            float vx = Mathf.Clamp(rb.velocity.x + h * move_force, -max_speed, max_speed);
            rb.velocity = new Vector2(vx, rb.velocity.y);
        }
        if (Rope.is_active)
        {
            // swinging from rope
            if (h != 0)
            {
                rb.AddForce(h * move_force * Vector2.right);
            }
            // climbing up and down
            if (!grounded && v != 0)
            {
                Rope.activate_pull_force_distance -= 0.1f * v;
                Rope.activate_pull_force_distance = Mathf.Clamp(Rope.activate_pull_force_distance - 0.1f * v, 1, Rope.max_allowed_rope_length);
            }
        }
    }

	// Update is called once per frame
	public void Update ()
    {
    	if (Input.GetButtonDown(jump_button) && grounded)
        {
                jump = true;
        }
	}

    public void OnCollisionEnter2D(Collision2D col)
    {
        grounded = true;
    }
    public void OnCollisionExit2D(Collision2D col)
    {
        grounded = false;
    }
}
