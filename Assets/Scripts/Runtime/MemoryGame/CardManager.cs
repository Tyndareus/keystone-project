using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField, Range(0f, 2.0f)] private float cardDisplaySpeed;
    [SerializeField, Range(1f, 10f)] private float cardMemoryTime;
    [SerializeField, Range(4, 13)] private int difficulty;
    
    [SerializeField] private Transform containerTransform;
    [SerializeField] private List<CardData> cardSelection;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GridLayoutGroup layout;

    [SerializeField] private FadeManager fadeManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private CanvasGroup worldScreen, canvasScreen;
    [SerializeField] private AudioSource resultAudio;

    private List<CardData> selectedCards;
    private Card[] playerSelection;
    
    private bool canSelect;

    private void Start()
    {
        layout.constraintCount = difficulty;
        playerSelection = new Card[PlayerDataManager.Instance.playerCount];

        PopulateCards();
        RandomiseCards();
        ConstructCards();
        PaintCards();
    }

    public void OnStart() => StartCoroutine(DisplayCards());

    //Temp but cant be bothered thinking of a solution at this stage
    public void Update()
    {
        canvasScreen.alpha = worldScreen.alpha;
        canvasScreen.interactable = worldScreen.interactable;
        canvasScreen.blocksRaycasts = worldScreen.blocksRaycasts;
    }

    private void PopulateCards()
    {
        selectedCards = new List<CardData>();
        for (int i = 0; i < difficulty; i++)
        {
            CardData rand = cardSelection[Random.Range(0, cardSelection.Count)];
            if (!selectedCards.Contains(rand))
            {
                selectedCards.Add(rand);
            }
            else
            {
                //Unsafe but fuck it
                i--;
            }
        }
    }

    private void RandomiseCards()
    {
        System.Random rand = new System.Random();

        List<CardData> tempList = selectedCards.ToList();

        tempList = tempList.OrderBy(c => rand.Next()).ToList();

        bool needsToRandomAgain = false;
        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].sprite == selectedCards[i].sprite)
            {
                needsToRandomAgain = true;
            }
        }

        if (needsToRandomAgain)
        {
            RandomiseCards();
        }
        else
        {
            selectedCards = tempList.ToList();
        }
    }

    private void ConstructCards()
    {
        for (int i = 0; i < difficulty * 2; i++)
        {
            Instantiate(cardPrefab, containerTransform);
        }
    }

    private void PaintCards()
    {
        for (int i = 0; i < containerTransform.childCount; i++)
        {
            Transform child = containerTransform.GetChild(i);
            Card card = child.GetComponent<Card>();
            card.UpdateCard(selectedCards[i % selectedCards.Count], this, cardDisplaySpeed);

            if (i == selectedCards.Count - 1 && i != containerTransform.childCount - 1)
            {
                RandomiseCards();
            }
        }
    }

    private IEnumerator DisplayCards()
    {
        for (float t = 0.0f; t <= cardDisplaySpeed; t += Time.deltaTime)
            yield return null;

        for (int i = 0; i < containerTransform.childCount; i++)
        {
            Transform child = containerTransform.GetChild(i);
            Card card = child.GetComponent<Card>();
            card.Display();

            for (float t = 0.0f; t <= cardDisplaySpeed; t += Time.deltaTime) yield return null;
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

            scoreManager.TryScore(true, true);
        }
        else
        {
            resultAudio.Play();
            playerSelection[index].Toggle(true);
            playerSelection[index].Hide();
            other.Toggle(true);
            other.Hide();
        }

        playerSelection[index] = null;
        canSelect = true;

        CheckIfComplete();
    }

    private void CheckIfComplete()
    {
        if (GetComponentsInChildren<Card>().All(x => x.HasBeenCompleted()))
        {
            StartCoroutine(GoToFade());
        }
    }

    private IEnumerator GoToFade()
    {
        FindObjectOfType<AudioManager>().OnGameComplete();
        
        for (float t = 0.0f; t <= 3.0f; t += Time.deltaTime) yield return null;

        fadeManager.FadeOut();
    }
}
