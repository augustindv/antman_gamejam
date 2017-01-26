using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

    public AudioClip fellSound;

    private AudioSource audioSource;
    private float waitTime = 0;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        waitTime += Time.deltaTime;
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground" && !audioSource.isPlaying && waitTime > 0.5 && transform.parent.name == "Crates")
        {
            audioSource.PlayOneShot(fellSound, 0.3f);
            waitTime = 0;
        }
    }

}
