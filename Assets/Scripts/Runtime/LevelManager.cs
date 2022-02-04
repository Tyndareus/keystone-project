using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private SceneData sceneData;

    private SceneObject currentScene;

    public void LoadFirstScene()
    {
        //Ensure the current scene "is" the main scene, even by number this should be fine.
        SceneData.currentScene = 0;
        SceneManager.LoadScene(sceneData.selectionScene.scene);
        /*loadOperation.allowSceneActivation = false;
        loadOperation.completed += LoadSceneAsync;*/
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
