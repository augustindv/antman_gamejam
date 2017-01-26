using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StaticScreen : MonoBehaviour {


    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            StartCoroutine(LaunchGame());
        }
    }

    IEnumerator LaunchGame()
    {
        yield return new WaitForSeconds(0.8f);
        float fadeTime = GameObject.Find("General").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    }

}