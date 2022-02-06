using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup loadingScreenGroup, mainGroup;
    [SerializeField] private Material characterMaterial;

    [SerializeField] private UnityEvent onFadeIn, onFadeOut;

    private static readonly int matColor = Shader.PropertyToID("_Color");

    private void Start()
    {
        if (characterMaterial)
        {
            characterMaterial.SetColor(matColor, new Color(1.0f, 1.0f, 1.0f, 0.0f));
        }
    }

    public void FadeIn() => StartCoroutine(FadeGameIn());

    public void FadeOut() => StartCoroutine(FadeGameOut());

    private IEnumerator FadeGameIn()
    {
        bool fadeMainGroup = mainGroup != null;
        bool fadeCharacter = characterMaterial != null;

        if (fadeMainGroup)
        {
            mainGroup.blocksRaycasts = true;
            mainGroup.interactable = true;
        }

        loadingScreenGroup.blocksRaycasts = false;
        loadingScreenGroup.interactable = false;
        
        Color col = Color.white;

        if (fadeCharacter)
        {
            col = characterMaterial.GetColor(matColor);
        }

        for (float t = 0.0f; t <= 1.25f; t += Time.deltaTime)
        {
            loadingScreenGroup.alpha = Mathf.Lerp(1.0f, 0.0f, t / 1.25f);

            if (fadeMainGroup)
            {
                mainGroup.alpha = Mathf.Lerp(0.0f, 1.0f, t / 1.25f);
            }

            if (fadeCharacter)
            {
                col.a = Mathf.Lerp(0.0f, 1.0f, t / 1.25f);
                characterMaterial.SetColor(matColor, col);
            }

            yield return null;
        }

        loadingScreenGroup.alpha = 0;

        if (fadeMainGroup)
        {
            mainGroup.alpha = 1;
        }

        if (fadeCharacter)
        {
            col.a = 1;
            characterMaterial.SetColor(matColor, col);
        }
        
        for (float t = 0.0f; t <= 0.5f; t += Time.deltaTime) yield return null;

        onFadeIn?.Invoke();
    }

    private IEnumerator FadeGameOut()
    {
        bool fadeMainGroup = mainGroup != null;
        bool fadeCharacter = characterMaterial != null;

        if (fadeMainGroup)
        {
            mainGroup.blocksRaycasts = false;
            mainGroup.interactable = false;
        }
        
        loadingScreenGroup.blocksRaycasts = true;
        loadingScreenGroup.interactable = true;
        
        

        Color col = Color.white;
        if (fadeCharacter)
        {
            col = characterMaterial.GetColor(matColor);
        }

        for (float t = 0.0f; t <= 1.25f; t += Time.deltaTime)
        {
            loadingScreenGroup.alpha = Mathf.Lerp(0.0f, 1.0f, t / 1.25f);

            if (fadeMainGroup)
            {
                mainGroup.alpha = Mathf.Lerp(1.0f, 0.0f, t / 1.25f);
            }

            if (fadeCharacter)
            {
                col.a = Mathf.Lerp(1.0f, 0.0f, t / 1.25f);
                characterMaterial.SetColor(matColor, col);
            }

            yield return null;
        }

        loadingScreenGroup.alpha = 1;

        if (fadeMainGroup)
        {
            mainGroup.alpha = 0;
        }

        if (fadeCharacter)
        {
            col.a = 0;
            characterMaterial.SetColor(matColor, col);
        }
        
        for (float t = 0.0f; t <= 0.5f; t += Time.deltaTime) yield return null;

        onFadeOut?.Invoke();
    }
}