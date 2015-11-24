using UnityEngine;

// problems with movement:
// when landing from jump the character stops. if the player pushes the buttons, we still get a momentary stop. it should be smooth.

// I would like that after running for a while, jumps are longer, and after running for short distance jumps are shorter.
// - I get this effect now, but it's very very weak.

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float jump_force = 8f;
        [SerializeField] private float move_force = 300f;
        [SerializeField] private float max_speed = 5f;
        [SerializeField] private float climbing_speed_factor = 0.01f;

        protected string jump_button;
        protected string horizontal_axis;
        protected string vertical_axis;
        protected string transform_button; // transform to platform

        protected bool jump;
        protected bool grounded;
        protected Rigidbody2D rb; // can I just use rigidbody2D instead of this?

        protected static bool rope_active = false;
        protected static float rope_max_distance = 0.0f;
    
        // I use this because I can pass parameters (can't with Start)
        public void Initialize(string jump_button, string horizontal, string vertical)
        {
            jump = false;
            grounded = true;
            rb = GetComponent<Rigidbody2D>();
            this.jump_button = jump_button;
            this.horizontal_axis = horizontal;
            this.vertical_axis = vertical;
        }
	
        private void Update()
        {
            if (Input.GetButtonDown(jump_button) && grounded)
            {
                jump = true;
            }
        }
        private void FixedUpdate()
        {
            if (jump)
            {
                rb.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);
                jump = false;
                grounded = false;
            }

            grounded = OnGround();

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
                    rb.AddForce(h * move_force * Vector2.right); //todo: force should be perpendicular to rope direction
                }
                // climbing up and down
                if (!grounded && v != 0)
                {
                    Rope.activate_pull_force_distance = Mathf.Clamp(Rope.activate_pull_force_distance - climbing_speed_factor * v, 1, Rope.max_allowed_rope_length);
                }
            }
        }

        private bool OnGround()
        {
            return Physics2D.Raycast(transform.position, Vector2.down, 0.5f, LayerMask.GetMask("platform"));
        }

        /*private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.contacts[0].normal == Vector2.up)
                grounded = true;

        }
        private void OnCollisionExit2D(Collision2D col)
        {
            grounded = false;
        }*/
    }
}
