using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField, SceneSelector] private int sceneIndex;
    
    public void OnPlayClick() => SceneManager.LoadScene(sceneIndex);

    public void OnQuitClick()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
