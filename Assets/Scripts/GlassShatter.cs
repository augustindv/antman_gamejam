using UnityEngine;
using System.Collections;

public class GlassShatter : MonoBehaviour {

    [HideInInspector]
    public bool isDestroyed;
    public Material mat;

    private Animator animator;
    private Collider2D polygonCollider;
    private AudioSource source;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        polygonCollider = GetComponent<Collider2D>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
	    if (isDestroyed)
        {
            GetComponent<SpriteRenderer>().material = mat;
            polygonCollider.enabled = false;
            animator.SetTrigger("destroy");
            isDestroyed = false;
            StartCoroutine(DestroyGlass());
        }
    }

    IEnumerator DestroyGlass()
    {
        polygonCollider.enabled = false;
        source.Play();
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
