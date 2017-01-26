using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

    public Transform player;

	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update() {
	    if (player.position.y + 2 > transform.position.y)
        {
            Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), false);
        } else
        {
            Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>(), true);
        }
    }
}
