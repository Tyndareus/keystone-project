using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu(menuName = "Keystone/Options")]
public class OptionsData : ScriptableObject
{
    public float volumeLevel
    {
        get => volume;
        private set
        {
            foreach (var audio in FindObjectsOfType<AudioSource>())
            {
                audio.volume = value;
            }
            
            volume = value;
        }
    }
    
    public int fontSize;

    public int colorMode
    {
        get => blindSetting.mode.value;
        set
        {
            if (blindSetting == null)
            {
                blindSetting = postProcess.GetSetting<ColorBlindCorrection>();
            }

            blindSetting.enabled.value = value != 0;

            if (value > 0)
            {
                blindSetting.mode.value = value - 1;
            }
        }
    }

    [SerializeField] private PostProcessProfile postProcess;
    private ColorBlindCorrection blindSetting;
    private float volume;

    private void Awake() => blindSetting = postProcess.GetSetting<ColorBlindCorrection>();
    public void SetVolume(float value) => volumeLevel = Mathf.Clamp(volumeLevel + value, 0.0f, 1.0f);
    public void SetFontSize(int value) => fontSize = Mathf.Clamp(fontSize + value, 5, 42);
}
