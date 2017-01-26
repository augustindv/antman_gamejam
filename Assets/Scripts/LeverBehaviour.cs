using UnityEngine;
using System.Collections;

public class LeverBehaviour : MonoBehaviour {

    [HideInInspector]
    public bool activated;
    public GameObject objectTriggered;
    public AudioClip leverSound;

    private AudioSource source;

    void Awake () {
        source = GetComponent<AudioSource>();
    }

	// Update is called once per frame
	void Update () {
	    if (activated && objectTriggered != null)
        {
            Destroy(objectTriggered);
            source.PlayOneShot(leverSound, 0.5f);
            activated = false;
        }
	}
}
