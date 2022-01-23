using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField, SceneSelector] private int sceneIndex;

    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject optionMenu;
    
    public void OnPlayClick() => SceneManager.LoadScene(sceneIndex);

    public void OnQuitClick()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnOptionsClicked()
    {
        mainMenuButtons.SetActive(false);
        optionMenu.SetActive(true);
    }
    
    public void OnBackClicked()
    {
        optionMenu.SetActive(false);
        mainMenuButtons.SetActive(true);
    }
}
