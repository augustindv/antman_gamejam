using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TriggerEnd : MonoBehaviour {

    public Animator doctorAnim;
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        doctorAnim.SetTrigger("move");
        yield return new WaitForSeconds(2.2f);
        float fadeTime = GameObject.Find("General").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("End Screen", LoadSceneMode.Single);
    }
}
