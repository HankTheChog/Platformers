using UnityEngine;
using System.Collections;

public class Aura : MonoBehaviour {

    private Color original_color;
    private Color magnet_is_on_color;
	// Use this for initialization
	void Start () {
        original_color = transform.gameObject.GetComponent<SpriteRenderer>().color;
        magnet_is_on_color = original_color;
        magnet_is_on_color.a = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.gameObject.GetComponent<SpriteRenderer>().color = Input.GetButton("ToggleRope") ? magnet_is_on_color : original_color;
	}
}
