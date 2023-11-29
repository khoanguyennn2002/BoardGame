using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;


public class CardInHand : MonoBehaviour
{
    public List<ItemCard> CardsInHand = new List<ItemCard>();
    public GameObject hand;
    public GameObject zone;
    [SerializeField] GameControl gameControl;
    [SerializeField] Dice dice;
    [SerializeField] GridMap grid;
    public Text notification;

    void Update()
    {
        
        CardInHandPlayer();
        if (gameControl.players[0].playerTurn && dice.hasRolledDice && !gameControl.isMoving)
        {
            gameControl.CheckHeal(CardsInHand);
            gameControl.CheckRange(CardsInHand);
            foreach (ItemCard card in CardsInHand)
            {
                card.GetComponent<DragCard>().enabled = true;
            }
            if (gameControl.hasBullet || gameControl.countBullet > 0)
            {
                foreach (ItemCard card in CardsInHand)
                {
                    if (card.cardIdGame == 7)
                    {
                        Image cardImage = card.GetComponentInChildren<Image>();
                        cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0.5f);
                        card.img.color = new Color(card.img.color.r, card.img.color.g, card.img.color.b, 0.5f);
                        card.GetComponent<DragCard>().enabled = false;
                    }
                }
            }
            else
            {
                foreach (ItemCard card in CardsInHand)
                {
                    if (card.cardIdGame == 7)
                    {
                        Image cardImage = card.GetComponentInChildren<Image>();
                        cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 1f);
                        card.img.color = new Color(card.img.color.r, card.img.color.g, card.img.color.b, 1f);
                        card.GetComponent<DragCard>().enabled = true;
                    }
                }
            }
            if (gameControl.checkRange)
            {
                foreach (ItemCard card in CardsInHand)
                {
                    if (card.cardIdGame == 9)
                    {
                        Image cardImage = card.GetComponentInChildren<Image>();
                        cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0.5f);
                        card.img.color = new Color(card.img.color.r, card.img.color.g, card.img.color.b, 0.5f);
                        card.GetComponent<DragCard>().enabled = false;
                    }
                }
            }
            else
            {
                foreach (ItemCard card in CardsInHand)
                {
                    if (card.cardIdGame == 9)
                    {
                        Image cardImage = card.GetComponentInChildren<Image>();
                        cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 1f);
                        card.img.color = new Color(card.img.color.r, card.img.color.g, card.img.color.b, 1f);
                        card.GetComponent<DragCard>().enabled = true;
                    }
                }
            }
            if (gameControl.checkHeal)
            {
                foreach (ItemCard card in CardsInHand)
                {
                    if (card.cardIdGame == 10)
                    {
                        Image cardImage = card.GetComponentInChildren<Image>();
                        cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0.5f);
                        card.img.color = new Color(card.img.color.r, card.img.color.g, card.img.color.b, 0.5f);
                        card.GetComponent<DragCard>().enabled = false;
                    }
                }
            }
            else
            {
                foreach (ItemCard card in CardsInHand)
                {
                    if (card.cardIdGame == 10)
                    {
                        Image cardImage = card.GetComponentInChildren<Image>();
                        cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 1f);
                        card.img.color = new Color(card.img.color.r, card.img.color.g, card.img.color.b, 1f);
                        card.GetComponent<DragCard>().enabled = true;
                    }
                }
            }
        }
        else
        {
            foreach (ItemCard card in CardsInHand)
            {
                card.GetComponent<DragCard>().enabled = false;
            }
        }
       
    }
   public void CardInHandPlayer()
    {
        foreach (Transform child in hand.transform)
        {
            ItemCard card = child.GetComponent<ItemCard>();
            if (card != null && !CardsInHand.Contains(card))
            {
                CardsInHand.Add(card);
            }
        }
        CardsInHand.RemoveAll(card => card == null || !card.transform.IsChildOf(hand.transform));
    }
}
