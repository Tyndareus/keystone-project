using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject optionMenu;
    [SerializeField] private FadeManager fadeManager;

    public void OnPlayClick()
    {
        foreach (var button in mainMenuButtons.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }

        fadeManager.FadeOut();
    }

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

    public void OnFadeOut()
    {
        LevelManager.Instance.LoadFirstScene();
    }
}