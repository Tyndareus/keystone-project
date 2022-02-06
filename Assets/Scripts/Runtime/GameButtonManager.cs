using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameButtonManager : MonoBehaviour
{
    [SerializeField] private OptionsData options;

    private bool currentlyMuted = false;

    public void OnExitClicked()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnLobbyClicked() => LevelManager.Instance.ReturnToLobby();

    public void OnMuteClicked()
    {
        currentlyMuted = !currentlyMuted;
        
        foreach (var a in FindObjectsOfType<AudioSource>())
        {
            a.volume = currentlyMuted ? 0.0f : options.volumeLevel;
        }
    }
}
