using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SavedInfos : MonoBehaviour {

    public static string savedScene;
    private static GameObject instance;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = gameObject;
        } else
        {
            Destroy(gameObject);
        }

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Game Over" || scene.name != "StartMenu")
        {
            savedScene = scene.name;
        }
	}
	
	// Update is called once per frame
	void Update () {
	}
}
