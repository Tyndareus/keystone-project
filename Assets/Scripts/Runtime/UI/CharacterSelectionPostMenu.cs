using System;
using System.Collections;
using UnityEngine;

public class CharacterSelectionPostMenu : MonoBehaviour
{
    [SerializeField] private FadeManager fadeManager;

    private void Start()
    {
        fadeManager.FadeIn();
    }

    public void OnFadeIn()
    {
        foreach (var uiPlayer in FindObjectsOfType<UIPlayerController>())
        {
            uiPlayer.EnableInput();
        }
    }

    public void OnCharacterSelected()
    {
        fadeManager.FadeOut();
    }

    public void OnFadeOut()
    {
        LevelManager.Instance.LoadNextScene();
    }
}
