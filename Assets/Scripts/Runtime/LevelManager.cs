using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private SceneData sceneData;

    private SceneObject currentScene;

    public void ReturnToLobby()
    {
        currentScene = sceneData.mainScene;
        SceneData.currentScene = 0;
        TimerManager.InPlay = false;
        
        SceneManager.LoadScene(currentScene.scene);
    }

    public void LoadNextScene()
    {
        if (SceneData.currentScene > sceneData.sceneOrder.Count - 1)
        {
            SceneManager.LoadScene(sceneData.finalScene.scene);
            return;
        }

        currentScene = sceneData.sceneOrder[SceneData.currentScene++];
        SceneManager.LoadScene(currentScene.scene);
    }
}
