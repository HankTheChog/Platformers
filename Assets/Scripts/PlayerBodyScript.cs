using UnityEngine;
using System.Collections;

public class PlayerBodyScript : MonoBehaviour {

    private Player parent_script;
    private BoxCollider2D ground_check;

	// Use this for initialization
	void Start () {
        parent_script = transform.parent.GetComponent<Player>();
        ground_check = transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool IsGrounded()
    {
        return Physics2D.IsTouchingLayers(ground_check, parent_script.can_jump_off.value);
    }

    public void TransformAnimationIsOver()
    {
        parent_script.TransformAnimationOver();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "spike")
        {
            parent_script.HitSpikes();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "anti-magnet")
        {
            parent_script.EnteredAntiMagnet();
        }
        if (col.tag == "exit door")
        {
            col.gameObject.GetComponent<ExitDoorway>().PlayerTouching(parent_script.WhoAmI);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "anti-magnet")
        {
            parent_script.LeavingAntiMagnet();
        }
        if (col.tag == "exit door")
        {
            col.gameObject.GetComponent<ExitDoorway>().PlayerLeaving(parent_script.WhoAmI);
        }
    }
}
