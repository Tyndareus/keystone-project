using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField, Range(0f, 2.0f)] private float cardDisplaySpeed;
    [SerializeField, Range(1f, 10f)] private float cardMemoryTime;

    [SerializeField] private Transform containerTransform;
    [SerializeField] private List<CardData> cardSelection;

    private List<Card> playerSelection;
    
    private bool canSelect;
    
    private void Start()
    {
        playerSelection = new List<Card>(PlayerDataManager.Instance.maxPlayerCount);
        RandomiseCards();

        StartCoroutine(PaintAndDisplay());
    }

    private void RandomiseCards()
    {
        System.Random rand = new System.Random();
        cardSelection = cardSelection.OrderBy(c => rand.Next()).ToList();
    }

    private IEnumerator PaintAndDisplay()
    {
        for (float t = 0.0f; t <= cardDisplaySpeed; t += Time.deltaTime)
            yield return null;

        for (int i = 0; i < containerTransform.childCount; i++)
        {
            Transform child = containerTransform.GetChild(i);
            Card card = child.GetComponent<Card>();
            card.UpdateCard(cardSelection[i % cardSelection.Count], this, cardDisplaySpeed);
            
            card.Display();

            for (float t = 0.0f; t <= cardDisplaySpeed; t += Time.deltaTime) yield return null;

            if (i == cardSelection.Count - 1 && i != containerTransform.childCount - 1)
            {
                RandomiseCards();
            }
        }

        for (float t = 0.0f; t <= cardMemoryTime; t += Time.deltaTime) yield return null;

        for (int i = 0; i < containerTransform.childCount; i++)
        {
            Transform child = containerTransform.GetChild(i);
            Card card = child.GetComponent<Card>();
            card.Hide();
            card.Toggle(true);
        }

        canSelect = true;
    }

    public void Select(int playerIndex, Card selectionCard)
    {
        if (!canSelect) return;
        
        if (playerSelection[playerIndex] != null &&
            playerSelection[playerIndex] == selectionCard) return;
        
        selectionCard.Toggle(false);
        StartCoroutine(DisplayCard(playerIndex, selectionCard));
    }

    private IEnumerator DisplayCard(int index, Card card)
    {
        card.Display();

        for (float t = 0.0f; t <= cardDisplaySpeed; t += Time.deltaTime)
        {
            yield return null;
        }

        if (playerSelection[index] != null)
        {
            CompareCards(index, card);
        }
        else
        {
            playerSelection[index] = card;
        }
    }

    private void CompareCards(int index, Card other)
    {
        canSelect = false;
        
        if (playerSelection[index].cardData.sprite ==
            other.cardData.sprite)
        {
            playerSelection[index].Toggle(false, true);
            other.Toggle(false, true);
        }
        else
        {
            playerSelection[index].Hide();
            other.Hide();
        }

        playerSelection[index] = null;
        canSelect = true;
    }
}
