using UnityEngine;
using System.Collections;

public class GroundCheckScript : MonoBehaviour {

    private BasicPlayer parent_script;
    private BoxCollider2D ground_check;

    // Use this for initialization
    void Start () {
        parent_script = transform.parent.GetComponent<BasicPlayer>();
        ground_check = transform.GetComponent<BoxCollider2D>();
    }

    public bool IsGrounded()
    {
        return Physics2D.IsTouchingLayers(ground_check, parent_script.can_jump_off.value);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "spike")
        {
            Debug.Log("body touched spikes");
            parent_script.HitSpikes();
        }
    }

}
