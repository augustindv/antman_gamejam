using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour {

    private AudioSource source;

    // Use this for initialization
    void Start () {
        UnityEngine.UI.Button btn = gameObject.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(TaskOnClick);
        source = GetComponent<AudioSource>();
    }


    void TaskOnClick()
    {
        StartCoroutine(Quit());
    }

    IEnumerator Quit()
    {
        source.Play();
        PauseMenu.isPaused = false;
        float fadeTime = GameObject.Find("General").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Application.Quit();
    }
}
