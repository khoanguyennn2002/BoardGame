using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ItemEnemyCard : MonoBehaviour
{
    private List<CardData> listCard = new List<CardData>();
    public List<Player> players;
    private GameObject[] enemyItems;
    public GameObject cardBack;
    private int cardIdData;
    private string cardName;
    private string cardDescription;
    public GameObject deckZone;
    public Deck deck;
    public GameObject dice;
    public Dice diceroll;
    public Text nameText;
    public int cardIdGame;
    public Text descriptionText;
    public Sprite thisSprite;
    public Image img;
    public bool IsCardBack = false;
    public int CardOfPlayer;
    public bool canBeDrag;
    public GameObject turn;
    void Awake()
    {
        deckZone = GameObject.Find("Deck Panel");
        deck = deckZone.GetComponent<Deck>();
        turn = GameObject.Find("Main Camera");
        players = turn.GetComponent<GameControl>().players;
        dice = GameObject.Find("Dice");
        diceroll = dice.GetComponent<Dice>();
        
    }
    void Start()
    {
        cardIdData = Random.Range(7, 11);
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "cardData.json");
        if (File.Exists(jsonFilePath))
        {
            string jsonContent = File.ReadAllText(jsonFilePath);
            CardManager.MyList cardList = JsonUtility.FromJson<CardManager.MyList>(jsonContent);
            List<CardData> listCardItem = cardList.cardList.FindAll(card => card.cardId == cardIdData);

            if (listCardItem != null)
            {
                listCard = listCardItem;
            }
        }
    }
    void Update()
    {
        cardIdGame = listCard[0].cardId;
        nameText.text = listCard[0].cardName;
        descriptionText.text = listCard[0].cardDescription;
        thisSprite = Resources.Load<Sprite>(listCard[0].img);
        cardName = listCard[0].cardName;
        cardDescription = listCard[0].cardDescription;
        img.sprite = thisSprite;
        if (this.tag == "Item2" && deck.deckSize!=0)
        {
            listCard[0] = deck.deck[deck.deckSize - 1];
            deck.deckSize -= 1;
            enemyItems = GameObject.FindGameObjectsWithTag("Item2");
            for (int j = 0; j < enemyItems.Length; j++)
            {
                enemyItems[j].name = listCard[0].cardName;
            }
            this.tag = "Untagged";
        }
        if (IsCardBack)
        {
            cardBack.SetActive(true);
        }
        else
        {
            cardBack.SetActive(false);

        }
    }
}