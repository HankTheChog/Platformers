using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour , IWinCondition
{
    public bool must_be_collected_for_win = false;
    public GameObject image_to_activate_upon_collection;
    public GameObject image_to_activate_upon_collection2;
    public GameObject image_to_activate_upon_collection3;
    private bool collected = false;


	// Use this for initialization
	void Start () {
        GlobalWinCondition.add(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool WinConditionSatisfied()
    {
        return (must_be_collected_for_win ? collected : true);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag=="Player")
        {
            Destroy(transform.GetComponent<CircleCollider2D>());
            // todo: add a cool effect here.
            Destroy(transform.gameObject, 0.1f);
            collected = true;
            image_to_activate_upon_collection.SetActive(true);
            image_to_activate_upon_collection2.SetActive(true);
            image_to_activate_upon_collection3.SetActive(true);
        }
    }

}
