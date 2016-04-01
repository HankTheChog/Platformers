using UnityEngine;
using System.Collections;



// This script should always be attached to an object with collider.
// It's meant to catch collision with triggers, such a anti-magnet and
// the exit gate.
// I never put the colliders on the root object of the player, so I need a 
// separate script for these kind of collisions
public class PlayerBodyScript : MonoBehaviour {

    private BasicPlayer parent_script;

	// Use this for initialization
	void Start () {
        parent_script = transform.parent.GetComponent<BasicPlayer>();
    }
	
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "anti-magnet")
        {
            parent_script.EnteredAntiMagnet();
        }
        if (col.tag == "exit door")
        {
            col.gameObject.GetComponent<ExitGate>().PlayerTouching(parent_script.WhoAmI);
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
            col.gameObject.GetComponent<ExitGate>().PlayerLeaving(parent_script.WhoAmI);
        }
    }
}
