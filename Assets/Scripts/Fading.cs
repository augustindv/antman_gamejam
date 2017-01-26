using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour {

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.2f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1; // in = -1, out = 1

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return fadeSpeed;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        BeginFade(-1);
    }
}
