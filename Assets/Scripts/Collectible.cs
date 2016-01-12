using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour , IWinCondition
{

    public bool must_be_collected_for_win = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    bool IWinCondition.WinConditionSatisfied()
    {
        return !must_be_collected_for_win;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag=="Player")
        {
            Destroy(transform.GetComponent<CircleCollider2D>());
            // todo: add a cool effect here.
            Destroy(transform.gameObject, 0.2f);
        }
    }

}
