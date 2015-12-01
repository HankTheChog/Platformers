﻿using UnityEngine;
using System.Collections;

public class TestPlayerScript : MonoBehaviour {

    private bool jumping = false;
    private bool grounded = true;
    private bool jump_button_pressed = false;
    private float max_jump_time = 0.175f;

    private float walk_force = 10f;
    private float max_walk_speed = 5f;
    public LayerMask collide_with;

    private float h_last_frame;
    private float time_for_full_running_speed = 1.0f;
    private float running_time = 0f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("RedHorizontal");
        float v = Input.GetAxisRaw("RedVertical");
        grounded = IsGrounded();

        if (grounded)
        {
            if (h == h_last_frame)
            {
                running_time += Time.deltaTime;
            }
            else
            {
                running_time = 0f;
            }

            // todo: I think movement is still jittery.
            // how to make it smoother ?
            // tweak the force I'm applying?
            // add counter-force instead of clamping max-velocity?
            // translate the rigidbody myself instead of using forces ? (won't take friction into account)
            if (Mathf.Abs(rb.velocity.x) < max_walk_speed)
            {
                float r = (Mathf.Min(rb.velocity.x, max_walk_speed) / max_walk_speed);
                rb.AddForce((1 - r) * Vector2.right * h * walk_force, ForceMode2D.Impulse);
            }
            if (Mathf.Abs(rb.velocity.x) > max_walk_speed)
            {
                Vector2 new_speed = rb.velocity;
                new_speed.x = Mathf.Clamp(rb.velocity.x, -max_walk_speed, max_walk_speed);
                rb.velocity = new_speed;
            }
        }
        h_last_frame = h;
    }

    void Update ()
    {
        float h = Input.GetAxisRaw("RedHorizontal");
        float v = Input.GetAxisRaw("RedVertical");
        jump_button_pressed = (v == 1);
        
        if (jump_button_pressed && grounded && !jumping)
        {
            StartCoroutine(Jump());
        }
	}

    bool IsGrounded()
    {
        Vector2 box_size = GetComponent<SpriteRenderer>().bounds.size * 0.5f;
        bool test = Physics2D.BoxCast(transform.position, box_size, 0, Vector2.down, box_size.y, collide_with.value);
        Debug.DrawLine(transform.position, transform.position + new Vector3(0, -box_size.y, 0), Color.red);
        return test;
        //return Physics2D.Raycast(transform.position, Vector2.down, 0.5f, LayerMask.GetMask("platform"));
    }

    IEnumerator Jump()
    {
        float timer = 0f;
        jumping = true;
        grounded = false;

        // since we zeroed the velocity, we need to restore sideways velocity to the player.
        // todo: the maximum force should be relevant to the walk velocity before the jump.
        // maybe I can also add it to the loop, adjusting in small steps...
        /*
                    rb.velocity = Vector2.zero;
        Vector2 horiz_force = Vector2.Lerp(Vector2.zero, Vector2.right, (seconds_running_in_same_direction / running_time_until_longest_jump));
        rb.AddForce(Input.GetAxisRaw(horizontal_axis) * horiz_force * 5, ForceMode2D.Impulse);
        */

        while (jump_button_pressed && timer < max_jump_time)
        {
            float proportion_completed = (timer / max_jump_time);
            Vector2 force_in_this_frame = Vector2.Lerp(Vector2.up*2, Vector2.zero, proportion_completed);
            rb.AddForce(force_in_this_frame, ForceMode2D.Impulse);
            timer += Time.deltaTime;
            yield return null;
        }
        jumping = false;
    }
}