using UnityEditor;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject optionMenu;

    public void OnPlayClick() => levelManager.LoadFirstScene();

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