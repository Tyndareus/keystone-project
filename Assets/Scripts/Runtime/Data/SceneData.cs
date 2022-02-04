using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =  "Keystone/Scene Data")]
public class SceneData : ScriptableObject
{
    public SceneObject mainScene;
    public SceneObject selectionScene;
    public SceneObject finalScene;
    public List<SceneObject> sceneOrder;
    public static int currentScene;
}

[Serializable]
public class SceneObject
{
    [SceneSelector] public int scene;
}
