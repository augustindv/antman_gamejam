using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Continue : MonoBehaviour {

    private AudioSource source;

    // Use this for initialization
    void Start () {
        UnityEngine.UI.Button btn = gameObject.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(TaskOnClick);
        source = GetComponent<AudioSource>();
    }

    void TaskOnClick()
    {
        source.Play();
        PauseMenu.isPaused = false;
    }
}
