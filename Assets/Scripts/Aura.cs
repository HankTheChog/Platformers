using UnityEngine;
using System.Collections;

public class Aura : MonoBehaviour {

    private Vector2 sprite_size;
    private Color original_color;
    private Color magnet_is_on_color;
    private string magnet_button;
    private bool magnet_is_on;

	// Use this for initialization
	void Start () {
        sprite_size = this.gameObject.GetComponent<SpriteRenderer>().bounds.extents;
        original_color = transform.gameObject.GetComponent<SpriteRenderer>().color;
        magnet_is_on_color = original_color;
        magnet_is_on_color.a = 0.9f;

        float scale_factor = (GameParameters.magnet_radius / sprite_size.x);
        transform.localScale = new Vector3(scale_factor, scale_factor, scale_factor);
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
            StartCoroutine(GlowEffect());
        }
        magnet_is_on = button_state_now;
    }

    IEnumerator GlowEffect()
    {
        float start_time = Time.time;

        while (magnet_is_on)
        {
            float time_since_start = Time.time - start_time;
            Color new_color;
            /*
            // slowly rise with magnet power, then pulse
            if (time_since_start < GameParameters.time_for_full_magnet_power)
            {
                new_color = Color.Lerp(original_color, magnet_is_on_color, (time_since_start / GameParameters.time_for_full_magnet_power));
            }
            else
            {
                new_color = magnet_is_on_color;
                float factor = 2 * Mathf.PI * freq * (time_since_start - GameParameters.time_for_full_magnet_power);
                new_color.a *= 1 + (0.05f * Mathf.Sin(factor));
            }
            */

            // pulse & raise opaque level at the same time.
            // pulse frequence also changes - starts fast, slows down (like it stabilizes)
            new_color = Color.Lerp(original_color, magnet_is_on_color, (time_since_start / GameParameters.time_for_full_magnet_power));
            float ff = Mathf.Lerp(5f, 1.5f, (time_since_start / GameParameters.time_for_full_magnet_power));
            float factor = 2 * Mathf.PI * ff * (time_since_start - GameParameters.time_for_full_magnet_power);

            new_color.a *= Mathf.Clamp(1 + (0.1f * Mathf.Sin(factor)), 0f, 1f);
            
            transform.gameObject.GetComponent<SpriteRenderer>().color = new_color;
            yield return null;
        }
        transform.gameObject.GetComponent<SpriteRenderer>().color = original_color;
    }
    
}
