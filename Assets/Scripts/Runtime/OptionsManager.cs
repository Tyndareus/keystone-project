using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private OptionsData options;

    private void Awake()
    {
        options.SetVolume(1);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) return;

        foreach (var a in FindObjectsOfType<AudioSource>())
        {
            a.volume = options.volumeLevel;
        }

        foreach (var t in FindObjectsOfType<TMP_Text>())
        {
            t.fontSize = options.fontSize;
        }
    }
}
