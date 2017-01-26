using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Propeller : MonoBehaviour {

    public float timeTurning = 5;
    public float timeStopping = 2;
    public float delayTime = 0;
    public Animator playerAnim;

    private Animator animator;
    private BoxCollider2D col;
    private float time;
    private bool turning = true;
    private bool started = false;
    private AudioSource[] audioSources;
    private AudioSource gameOverSource;
    private AudioSource windSource;
    private AudioSource windSource2;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        audioSources = GetComponents<AudioSource>();
        gameOverSource = audioSources[0];
        windSource = audioSources[1];
        if (audioSources.Length > 2)
        {
            windSource2 = audioSources[2];
        }
        animator.speed = 3;
    }

    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;
        if (!started)
        {
            if (time > delayTime)
            {
                started = true;
                time = 0;
            }
        }
        if (started && turning && time > timeTurning)
        {
            if (windSource2 != null)
            {
                windSource2.Stop();
            }
            animator.speed = 0;
            time = 0;
            turning = false;
            windSource.Stop();
            col.enabled = false;
        }
        else if (started && !turning && time > timeStopping)
        {
            if (windSource2 != null)
            {
                windSource2.Stop();
            }
            animator.speed = 3;
            time = 0;
            turning = true;
            windSource.Play();
            col.enabled = true;
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            StartCoroutine(GameOver());
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
