using UnityEngine;
using System.Collections;

public class CameraTrigger : MonoBehaviour
{

    public Transform player;
    public float smooth = 3.0f;
    [HideInInspector]
    public Vector3 reset = new Vector3(0, 20);

    private bool moveCamera = true;
    private Transform mainCamera;
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main.transform;
        offset = mainCamera.position - player.position;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        GameObject tempObject = col.gameObject;
        if (tempObject.tag == "CameraTrigger")
        {
            moveCamera = false;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        GameObject tempObject = col.gameObject;
        if (tempObject.tag == "CameraTrigger")
        {
            moveCamera = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveCamera)
        {
            Vector3 targetPosition = player.position + offset;
            transform.position = mainCamera.position - reset;

            mainCamera.position = Vector3.Lerp(mainCamera.position, targetPosition, smooth * Time.deltaTime);

        }
    }


}
