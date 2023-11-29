
using UnityEngine;

[System.Serializable]

public class CardData
{
    public int cardId;
    public string cardName;
    public string cardDescription;

    public int cardType;

    public string img;

    public CardData(int id, string name, string description, int type, string sprite)
    {
        cardId = id;
        cardName = name;
        cardDescription = description;
        cardType = type;
        img = sprite;
    }
}
