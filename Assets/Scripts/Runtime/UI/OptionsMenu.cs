using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class OptionsMenu : MonoBehaviour
{
    public enum ColorBlindModes
    {
        None,
        Protanopia,
        Deuteranopia,
        Tritanopia,
        Achromatopsia
    }

    [SerializeField] private TMP_Text colorBlindText;
    [SerializeField] private PostProcessVolume profile;

    private ColorBlindCorrection blindSetting;
    
    private ColorBlindModes currentMode = ColorBlindModes.None;

    private void Awake()
    {
        blindSetting = profile.sharedProfile.GetSetting<ColorBlindCorrection>();
        currentMode = (ColorBlindModes)(blindSetting.mode.value + 1);
        colorBlindText.text = currentMode.ToString();
    }

    public void ColorBlindOptionChanged(int direction)
    {
        int current = (int)currentMode;

        if (current + direction < 0 || current + direction > (int)ColorBlindModes.Achromatopsia) return;

        current += direction;

        currentMode = (ColorBlindModes)current;

        if (currentMode == ColorBlindModes.None)
        {
            blindSetting.enabled.value = false;
        }
        else
        {
            blindSetting.enabled.value = true;

            blindSetting.mode.value = (current - 1);
        }

        colorBlindText.text = currentMode.ToString();
    }
}
