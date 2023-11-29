
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{

    public List<CardData> deck = new List<CardData>();
    public List<CardData> myList = new List<CardData>();
    public List<CardData> container = new List<CardData>();
  //  public static List<CardData> staticDeck = new List<CardData>();
    public Tutorial tutorial;
    //internal bool isStartGame;

    public Text numberOfDeck;
    public GameObject cardIndex1;
    public GameObject cardIndex2;
    public GameObject cardIndex3;
    public GameObject cardIndex4;

    public GameObject CardToHand;
    public GameObject CardToHand2;

    public GameObject cardShuffle;

    public GameObject DeckPanel;

    public GameObject[] Clones;

    public int deckSize = 90;


    void Start()
    {
        //####18/6
        //StartCoroutine(WaitFiveSecond());
        //
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "cardData.json");
        if (File.Exists(jsonFilePath))
        {
            string jsonContent = File.ReadAllText(jsonFilePath);
            CardManager.MyList cardList = JsonUtility.FromJson<CardManager.MyList>(jsonContent);
            List<CardData> cardToDisplay = cardList.cardList.FindAll(card => card.cardType == 2);
            if (cardToDisplay != null)
            {
                myList.AddRange(cardToDisplay);
            }
            CreateDeck();
        }
    }
    void Update()
    {
        numberOfDeck.text = deckSize.ToString();
        if (deckSize < 40)
        {
            cardIndex1.SetActive(false);
        }
       else if (deckSize < 30)
        {
            cardIndex2.SetActive(false);
        }
        else if (deckSize < 20)
        {
            cardIndex3.SetActive(false);
        }
        else if (deckSize < 5)
        {
            cardIndex4.SetActive(false);
        }
         if (deckSize <= 0)
        {
            ResetDeck();
        }
    }
    void CreateDeck()
    {
        for (int i = 0; i < deckSize; i++)
        {
            int x = Random.Range(0, 4);
            deck[i] = myList[x];
        }
    }
    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Clones = GameObject.FindGameObjectsWithTag("Clone");
        foreach (GameObject Clone in Clones)
        {
            if (Clone != null)
            {
                Destroy(Clone);
            }
        }
    }
    public IEnumerator StartGame()
    {
        for (int i = 0; i <= 4; i++)
        {
            yield return new WaitForSeconds(0.4f);
            Instantiate(CardToHand, transform.position, transform.rotation);
            Instantiate(CardToHand2, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(0.4f);
        Shuffle();
        //  isStartGame = true;
    }
    public IEnumerator DrawCard()
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(0.4f);
            Instantiate(CardToHand, transform.position, transform.rotation);
        }
    }
    public void Draw()
    {
        StartCoroutine(DrawCard());
    }

    public void Shuffle()
    {
        for (int i = 0; i < deckSize; i++)
        {
            container[0] = deck[i];
            int random = Random.Range(i, deckSize);
            deck[i] = deck[random];
            deck[random] = container[0];
        }
        Instantiate(cardShuffle, transform.position, transform.rotation);
        StartCoroutine(DelayedDestroy());
    }
    public IEnumerator DrawCard2()
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(0.4f);
            Instantiate(CardToHand2, transform.position, transform.rotation);
        }
    }
    public void Draw2()
    {
        StartCoroutine(DrawCard2());
    }
    IEnumerator ActivateCardIndexWithDelay(GameObject cardIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        cardIndex.SetActive(true);
    }
    void ResetDeck()
    {
      
        deckSize = 90;
        StartCoroutine(ActivateCardIndexWithDelay(cardIndex1, 0.5f));
        StartCoroutine(ActivateCardIndexWithDelay(cardIndex2, 0.6f));
        StartCoroutine(ActivateCardIndexWithDelay(cardIndex3, 0.7f));
        StartCoroutine(ActivateCardIndexWithDelay(cardIndex4, 0.8f));
        Shuffle();
        CreateDeck();
    }
}
