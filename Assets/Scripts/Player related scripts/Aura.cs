using UnityEngine;
using System.Collections;

public class Aura : MonoBehaviour {

    private Vector2 sprite_size;

    private string magnet_button;
    private bool magnet_is_on;

    private float scale_full_magnet;
    private float scale_no_magnet;
    private float scale_current;
    private Animator anim;



	// Use this for initialization
	void Start () {
        sprite_size = this.gameObject.GetComponent<SpriteRenderer>().bounds.extents;
        anim = this.gameObject.GetComponent<Animator>();

        scale_full_magnet = (GameParameters.magnet_radius / sprite_size.x);
        scale_no_magnet = 0.0f;
        scale_current = scale_no_magnet;
    }
	
    public void SetMagnetButton(string b)
    {
        magnet_button = b;
    }

    // Update is called once per frame
    void Update()
    {
        bool button_state_now = Input.GetButton(magnet_button);
        if (!magnet_is_on && button_state_now == true)
        {
            //magnet button just pressed, start the glowing effect
            magnet_is_on = true; // must set this before starting the coroutine
            anim.Play("Blue halo animation");
            StartCoroutine(GlowEffect());
        }
        magnet_is_on = button_state_now;

        
        //float s = scale_current * (1 + 0.01f * Mathf.Sin(10f * Time.time)); // this causes the pulsing effect of the aura.
        float s = scale_current;
        transform.localScale = new Vector3(s, s, 1.0f);
    }

    IEnumerator GlowEffect()
    {
        float start_time = Time.time;

        GetComponent<AudioSource>().Play();
        while (magnet_is_on)
        {
            float time_since_start = Time.time - start_time;
            // uncomment the next line for color-pulsing (looks kinda ugly!)
            /*
            Color new_color;
            // pulse frequence also changes - starts fast, slows down (as if it stabilizes)
            new_color = Color.Lerp(original_color, magnet_is_on_color, (time_since_start / GameParameters.time_for_full_magnet_power));
            float ff = Mathf.Lerp(5f, 1.5f, (time_since_start / GameParameters.time_for_full_magnet_power));
            float factor = 2 * Mathf.PI * ff * (time_since_start - GameParameters.time_for_full_magnet_power);
            float sin_f = Mathf.Sin(factor);
            new_color.a *= Mathf.Clamp(1 + (0.15f * sin_f), 0f, 1f);
            transform.gameObject.GetComponent<SpriteRenderer>().color = new_color;
            */
             
            if (time_since_start < GameParameters.time_for_full_magnet_power)
            {
                // exponential interpolation - rise in 100ms to 95% of full radius
                scale_current = scale_full_magnet*(1 - (1 / Mathf.Exp(2.9957322f * time_since_start)));
            }
            else
            {
                scale_current = scale_full_magnet;
            }
            yield return null;
        }

        // make it return slowly to normal
        //todo: make the sound fade away slowly
        start_time = Time.time;
        float s = scale_current;
        while (!magnet_is_on && (Time.time - start_time) < 0.1f)
        {
            scale_current = Mathf.Lerp(s, scale_no_magnet, (Time.time - start_time) / 0.1f);
            yield return null;
        }
        anim.Play("idle");
        GetComponent<AudioSource>().Stop();
    }
    
}
