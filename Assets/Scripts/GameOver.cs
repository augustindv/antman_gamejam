using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    private string oldScene;
    private AudioSource source;

    void Start()
    {
        oldScene = SavedInfos.savedScene;
        UnityEngine.UI.Button btn = gameObject.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(TaskOnClick);
        source = GetComponent<AudioSource>();
    }

    void TaskOnClick()
    {
        StartCoroutine(LaunchGame());
    }

    IEnumerator LaunchGame()
    {
        source.Play();
        float fadeTime = GameObject.Find("General").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(oldScene, LoadSceneMode.Single);
    }

}
