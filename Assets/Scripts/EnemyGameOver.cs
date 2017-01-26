using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyGameOver : MonoBehaviour {

    public Animator animator;

    private AudioSource firingSource;

    // Use this for initialization
    void Start () {
        firingSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	
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
        Player.gameOver = true;
        animator.SetTrigger("gameOver");
        yield return new WaitForSeconds(0.8f);
        firingSource.Play();
        yield return new WaitForSeconds(1f);
        float fadeTime = GameObject.Find("General").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
    }
}
