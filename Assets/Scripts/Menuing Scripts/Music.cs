using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour {

    private static GameObject instance;
    private static AudioSource source;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        source = GetComponent<AudioSource>();
    }
	
	void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "StartMenu" && sceneName != "Commands Screen" && sceneName != "Credits")
        {
            source.Stop();
        }
        if (SavedInfos.savedScene != null && SavedInfos.savedScene != "StartMenu")
        {
            source.Play();
        }
    }


}
