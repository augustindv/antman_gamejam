using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EventSound : MonoBehaviour {

    private AudioSource source;
    private EventSystem eventSystem;
    private GameObject localObject;

    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
        eventSystem = GetComponent<EventSystem>();
        localObject = null;
    }

    // Update is called once per frame
    void Update () {
        GameObject tempObject = eventSystem.currentSelectedGameObject;
        if (localObject != tempObject && localObject != null && tempObject != null)
        {
            source.Play();
        }
        localObject = tempObject;
	}
}
