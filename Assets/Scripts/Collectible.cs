using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
    public GameObject image_to_activate_upon_collection;
    public GameObject image_to_activate_upon_collection2;

    static public int total_number;
    static public int number_touched;

    void OnLevelWasLoaded()
    {
        total_number = 0;
        number_touched = 0;
    }

    void Start () {
        total_number++;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag=="Player")
        {
            GetComponent<AudioSource>().Play();
            number_touched++;
            Destroy(transform.GetComponent<CircleCollider2D>());

            if (image_to_activate_upon_collection)
                image_to_activate_upon_collection.SetActive(true);
            if (image_to_activate_upon_collection2)
                image_to_activate_upon_collection2.SetActive(true);
        }
    }

}
