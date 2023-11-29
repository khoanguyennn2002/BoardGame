using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardTutorial : MonoBehaviour
{
    public Text notification;
    private GameObject TextSystem;
    private List<CardData> listCard = new List<CardData>();
    private List<Player> players;
    private GameObject[] Items;
    public GameObject cardBack;
    public int cardIdData;
    private string cardName;
    private string cardDescription;
    public GameObject dice;
    public Dice diceroll;
    public Text nameText;
    public int cardIdGame;
    public Text descriptionText;
    public Sprite thisSprite;
    public Image img;
    public bool IsCardBack = false;
    public bool isInUse;
    private GameObject turn;
    public GameControl gameControl;
    private GameObject zone;
    void Awake()
    {
        zone = GameObject.Find("Drop Zone");
        turn = GameObject.Find("Main Camera");
        gameControl = turn.GetComponent<GameControl>();
        players = turn.GetComponent<GameControl>().players;
        dice = GameObject.Find("Dice");
        diceroll = dice.GetComponent<Dice>();
        TextSystem = GameObject.Find("TextSystem");
        notification = TextSystem.transform.Find("Notification").gameObject.GetComponent<Text>();
    }
    void Start()
    {
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
