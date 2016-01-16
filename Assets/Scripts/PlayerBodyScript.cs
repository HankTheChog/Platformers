using UnityEngine;
using System.Collections;

public class PlayerBodyScript : MonoBehaviour {

    private Player parent_script;

    private float dist_to_ground;
    private Vector2 size_self;

	// Use this for initialization
	void Start () {
        parent_script = transform.parent.GetComponent<Player>();
        dist_to_ground = GetComponent<PolygonCollider2D>().bounds.extents.y;
        size_self = GetComponent<PolygonCollider2D>().bounds.extents;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, size_self, 0, Vector2.down, 0.75f * dist_to_ground, parent_script.can_jump_off.value);
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
