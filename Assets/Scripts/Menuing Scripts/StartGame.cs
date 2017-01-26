using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	    if (Input.anyKeyDown)
        {
            StartCoroutine(LaunchGame());
        }
	}

    IEnumerator LaunchGame()
    {
        float fadeTime = GameObject.Find("General").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("Level 1", LoadSceneMode.Single);
    }

}
