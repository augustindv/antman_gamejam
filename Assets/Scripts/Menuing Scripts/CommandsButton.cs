using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CommandsButton : MonoBehaviour {

    private AudioSource source;

    // Use this for initialization
    void Start()
    {
        UnityEngine.UI.Button btn = gameObject.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(TaskOnClick);
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

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
        SceneManager.LoadScene("Commands Screen", LoadSceneMode.Single);
    }
}
