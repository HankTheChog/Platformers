using UnityEngine;
using System.Collections;

public class Aura : MonoBehaviour {

    private Vector2 sprite_size;
    private Color original_color;
    private Color magnet_is_on_color;
    private string magnet_button;

	// Use this for initialization
	void Start () {
        sprite_size = this.gameObject.GetComponent<SpriteRenderer>().bounds.extents;
        original_color = transform.gameObject.GetComponent<SpriteRenderer>().color;
        magnet_is_on_color = original_color;
        magnet_is_on_color.a = 1.0f;

        float scale_factor = (GameParameters.magnet_radius / sprite_size.x);
        transform.localScale = new Vector3(scale_factor, scale_factor, scale_factor);
    }
	
    public void SetMagnetButton(string b)
    {
        magnet_button = b;
    }
    // Update is called once per frame
    void Update() {
        transform.gameObject.GetComponent<SpriteRenderer>().color = Input.GetButton(magnet_button) ? magnet_is_on_color : original_color;
	}
}
