using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {

    private Player player;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag != "CameraTrigger" && col.gameObject.tag != "Wall" && col.gameObject.tag != "Breakable" && col.gameObject.tag != "Trigger")
        {
            player.grounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "CameraTrigger" && col.gameObject.tag != "Wall" && col.gameObject.tag != "Breakable" && col.gameObject.tag != "Trigger")
        {
            player.grounded = false;
        }
    }
}
