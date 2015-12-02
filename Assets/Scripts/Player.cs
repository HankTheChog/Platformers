using System.Collections;
using UnityEngine;

// I would like that after running for a while, jumps are longer, and after running for short distance jumps are shorter.
//   this means track time in which horizontal axis is the same, and adjust jump accordingly.
//   right now, it doesn't work right.
//   also, when landing, do we keep the momentum for the next jump ?

// todo: forces should be multiplied by mass (in case I ever want to change the mass)

// IsGrounded possibilities:
// cast capsule/box downwards (as wide as the player)
// Use a trigger collider as the feet of the player, and update in OnTriggerEnter2D OnTriggerExit2D 
//  grounded = Physics2D.OverlapArea(top_left.position, bottom_right.position, ground_layers);
// 2D character controller

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float jump_force;

        [SerializeField]
        private float walk_force;

        [SerializeField]
        private float rope_swing_force;

        [SerializeField]
        private float max_walk_speed;

        [SerializeField]
        private float climbing_speed_factor;

        [SerializeField]
        private GameObject other_player;


        protected string horizontal_axis;
        protected string vertical_axis;
        protected string transform_button;

        protected bool is_platform_mode;

        protected Rigidbody2D rb;

        // everything about jumps
        protected bool jump_button_pressed;
        protected bool jumping = false;
        protected bool grounded;
        protected float max_jump_time = 0.4f;

        public void Initialize(string horizontal, string vertical, string trans)
        {
            grounded = true;
            is_platform_mode = false;
            rb = GetComponent<Rigidbody2D>();
            this.horizontal_axis = horizontal;
            this.vertical_axis = vertical;
            this.transform_button = trans;
        }

        private void Update()
        {
            jump_button_pressed = Input.GetAxisRaw(vertical_axis) == 1;

            if (jump_button_pressed && !jumping && grounded)
            {
                StartCoroutine(Jump());
            }

            if (Input.GetButtonDown(transform_button))
            {
                is_platform_mode = !is_platform_mode;
                TurnIntoPlatform();
            }
        }


        private void TurnIntoPlatform()
        {
            Vector3 scale = transform.localScale;
            if (is_platform_mode)
            {
                scale.x = 12;
                rb.isKinematic = true;
                gameObject.layer = 8;
            }
            else
            {
                scale.x = 3;
                rb.isKinematic = false;
                gameObject.layer = 9;
            }
            transform.localScale = scale;
        }

        private void FixedUpdate()
        {
            float h = Input.GetAxisRaw(horizontal_axis);
            float v = Input.GetAxisRaw(vertical_axis);

            if (grounded)
            {
                rb.AddForce(Vector2.right * h * walk_force, ForceMode2D.Impulse);

                if (Mathf.Abs(rb.velocity.x) > max_walk_speed)
                {
                    Vector2 new_speed = rb.velocity;
                    new_speed.x = Mathf.Clamp(rb.velocity.x, -max_walk_speed, max_walk_speed);
                    rb.velocity = new_speed;
                }
            }

            if (Rope.is_active)
            {
                // swinging from rope
                if (h != 0)
                {
                    Vector2 to_other = (other_player.transform.position - transform.position).normalized;
                    Vector2 clockwise_rotate = new Vector2(to_other.y, -to_other.x);
                    rb.AddForce(h * rope_swing_force * clockwise_rotate);
                }
                // climbing up and down
                if (!grounded && v != 0)
                {
                    Rope.rope_length = Mathf.Clamp(Rope.rope_length - climbing_speed_factor * v, 1, 5);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag=="platform")
            {
                grounded = true;
            }
        }


        // I copied this from : http://gamasutra.com/blogs/DanielFineberg/20150825/244650/Designing_a_Jump_in_Unity.php
        IEnumerator Jump()
        {
            float timer = 0f;
            jumping = true;
            grounded = false;

            /*
            rb.velocity = Vector2.zero;
            // since we zeroed the velocity, we need to restore sideways velocity to the player.
            // todo: the maximum force should be relevant to the walk velocity before the jump.
            // maybe I can also add it to the loop, adjusting in small steps...
            Vector2 horiz_force = Vector2.Lerp(Vector2.zero, Vector2.right, (seconds_running_in_same_direction / running_time_until_longest_jump));
            rb.AddForce(Input.GetAxisRaw(horizontal_axis) * horiz_force * 5, ForceMode2D.Impulse);
            */

            while (jump_button_pressed && timer < max_jump_time)
            {
                float proportion_completed = (timer / max_jump_time);
                Vector2 force_in_this_frame = Vector2.Lerp(Vector2.up, Vector2.zero, proportion_completed);
                rb.AddForce(force_in_this_frame, ForceMode2D.Impulse);
                timer += Time.deltaTime;
                yield return null;
            }
            jumping = false;
        }

    }
}
