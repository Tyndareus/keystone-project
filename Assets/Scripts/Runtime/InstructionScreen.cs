using UnityEngine;

public class InstructionScreen : MonoBehaviour
{
    [SerializeField] private FadeManager altFade, fadeManager;
    
    private void Awake() => altFade.FadeIn();
    public void OnPlayClicked() => fadeManager.FadeIn();
}
