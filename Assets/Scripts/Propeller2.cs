using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Propeller2 : MonoBehaviour {

    public Animator playerAnim;

    private Animator animator;
    private BoxCollider2D boxCollider;
    private bool canGameOver = true;
    private AudioSource[] audioSources;
    private AudioSource gameOverSource;
    private AudioSource windSource;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSources = GetComponents<AudioSource>();
        gameOverSource = audioSources[0];
        windSource = audioSources[1];
    }

    // Update is called once per frame
    void Update () {
	    
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && canGameOver)
        {
            StartCoroutine(GameOver());
        }
        if (col.tag == "Grabbable")
        {
            canGameOver = false;
            windSource.Stop();
            animator.speed = 0;
            boxCollider.enabled = false;
        }
    }

    IEnumerator GameOver()
    {
        if (!Player.gameOver)
        {
            Player.gameOver = true;
            playerAnim.SetTrigger("gameOverPropeller");
            gameOverSource.Play();
            yield return new WaitForSeconds(2f);
            float fadeTime = GameObject.Find("General").GetComponent<Fading>().BeginFade(1);
            yield return new WaitForSeconds(fadeTime);
            SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
        }
    }
}
