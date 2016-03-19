using UnityEngine;
using System.Collections;

public class ActivateExitGate : MonoBehaviour {

    private GameObject gate;


	// Use this for initialization
	void Start () {
        gate = transform.GetChild(0).gameObject;
	}
	
	void Update ()
    {
	    if (Collectible.total_number == Collectible.number_touched)
        {
            gate.SetActive(true);
        }
    }
}
