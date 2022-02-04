using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Card : Selectable
{
    [SerializeField] private Image image;
    public CardData cardData { get; private set; }
    private CardManager cardManager;

    private float cardRotationSpeed;

    private Image[] childImages;

    protected override void Awake()
    {
        childImages = GetComponentsInChildren<Image>();
        Toggle(false);
    }

    public void UpdateCard(CardData data, CardManager manager, float speed)
    {
        cardData = data;
        image.sprite = data.sprite;
        cardManager = manager;
        cardRotationSpeed = speed;
    }

    public void Submit(int playerIndex) => cardManager.Select(playerIndex, this);

    public void Toggle(bool canInteract, bool matched = false)
    {
        foreach (Image img in childImages)
        {
            img.raycastTarget = canInteract;

            if (matched)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
            }
        }
    }

    public void Display() => StartCoroutine(RotateCard(new Vector3(0, 180, 0)));
    public void Hide() => StartCoroutine(RotateCard(new Vector3(0, 0, 0)));
    public bool HasBeenCompleted() => childImages.All(img => !img.raycastTarget);

    private IEnumerator RotateCard(Vector3 rotation)
    {
        Quaternion original = transform.rotation;

        for (float t = 0.0f; t <= cardRotationSpeed; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Slerp(original, Quaternion.Euler(rotation), t / cardRotationSpeed);
            yield return null;
        }
    }
}
