using UnityEngine;
using System.Collections;

public class PlayerBodyScript : MonoBehaviour {

    private BasicPlayer parent_script;

	// Use this for initialization
	void Start () {
        parent_script = transform.parent.GetComponent<BasicPlayer>();
    }
	
    public void TurnedIntoHuman()
    {
        parent_script.TurnedIntoHuman();
    }
    public void TurnedIntoPlatform()
    {
        parent_script.TurnedIntoPlatform();
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
