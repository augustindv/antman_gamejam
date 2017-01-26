using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {

    [HideInInspector]
    public static bool isPaused = false;

    private float sinceLastPause = 0;
    [SerializeField]
    private Transform pauseMenuParent = null;
    [SerializeField]
    private Transform eventSystem = null;
    private List<UnityEngine.UI.Button> buttonsList;
    private List<Image> imagesList;

    // Use this for initialization
    void Start ()
    {
        isPaused = false;
        imagesList = new List<Image>();
        buttonsList = new List<UnityEngine.UI.Button>();
        buttonsList.AddRange(pauseMenuParent.GetComponentsInChildren<UnityEngine.UI.Button>());
        imagesList.AddRange(pauseMenuParent.GetComponentsInChildren<Image>());
        HidePauseMenu();
    }

    // Update is called once per frame
    void Update () {
        bool pause = Input.GetButton("Pause");

        if (pause && sinceLastPause > 0.3)
        {
            isPaused = !isPaused;
            sinceLastPause = 0;
        }
        if (isPaused)
        {
            DisplayPauseMenu();
            Time.timeScale = 0f;
        }
        else if (!isPaused)
        {
            Time.timeScale = 1.0f;
            HidePauseMenu();
        }
        sinceLastPause += Time.unscaledDeltaTime;
    }

    void DisplayPauseMenu()
    {
        eventSystem.GetComponent<EventSystem>().enabled = true;
        foreach (Image image in imagesList)
        {
            image.CrossFadeAlpha(0.8f, 0.25f, true);
        }
    }

    void HidePauseMenu()
    {
        eventSystem.GetComponent<EventSystem>().enabled = false;
        foreach (Image image in imagesList)
        {
            image.CrossFadeAlpha(0f, 0.1f, true);
        }
    }
}
