using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour {

    public string sceneName;

    private Button btn;
    private AudioSource source;

    // Use this for initialization
    void Start()
    {
        btn = gameObject.GetComponent<UnityEngine.UI.Button>();
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
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
