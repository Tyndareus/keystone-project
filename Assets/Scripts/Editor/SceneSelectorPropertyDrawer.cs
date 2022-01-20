using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneSelector))]
public class SceneSelectorPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();

        int selection = EditorGUI.Popup(position, label.text, property.intValue, EditorBuildSettings.scenes.Select(scene => scene.path).ToArray());

        if (EditorGUI.EndChangeCheck())
        {
            property.intValue = selection;
        }
    }
}
