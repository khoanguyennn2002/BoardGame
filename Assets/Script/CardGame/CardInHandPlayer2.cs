using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CardInHandPlayer2 : MonoBehaviour
{
    public List<ItemEnemyCard> CardsInHand = new List<ItemEnemyCard>();
    public GameObject hand;
    public GameObject zone;
    [SerializeField] GameControl gameControl;
    [SerializeField] Dice dice;
    [SerializeField] GridMap grid;
    public bool[] AiCanUseCard;
    public int[] cardsId;
    public int useCardID;
    private bool checkAttackCard = false;
    private bool foundWineCard = false;
    private bool foundBulletCard = false;
    private int countRangeCard = 0;
    void Update()
    {
        CardInHand();
        //if (!isUpdated)
        //{
        //    UpdateAiCanUseCard();
        //}
        //ChooseCardToUse();
        UseCard();
        ResetFlag();
    }
    void CardInHand()
    {
        foreach (Transform child in hand.transform)
        {
            ItemEnemyCard card = child.GetComponent<ItemEnemyCard>();
            if (card != null && !CardsInHand.Contains(card))
            {
                CardsInHand.Add(card);
            }
        }
        CardsInHand.RemoveAll(card => card == null || !card.transform.IsChildOf(hand.transform));
    }
     bool checkBullet()
    {
        for (int i = 0; i < CardsInHand.Count; i++)
        {
            if (CardsInHand[i].cardIdGame == 7)
            {
                return true; }
        }
        return false;
    }    

    void UseCard()
    {
        if (gameControl.players[1].playerTurn && dice.hasRolledDice == true)
        {
            for (int i = 0; i < CardsInHand.Count; i++)
            {
               
                if (CardsInHand[i].cardIdGame == 7)
                {
                    if (grid.IsPlayerInAttackRange(gameControl.players[1], gameControl.players[0].x, gameControl.players[0].y) && !foundBulletCard)
                    {
                        CardsInHand[i].transform.SetParent(zone.transform);
                        CardsInHand[i].GetComponent<ItemEnemyCard>().IsCardBack = false;
                        foundBulletCard = true;
                        foundWineCard = true;
                        break;
                    }
                    if (checkAttackCard && !foundBulletCard)
                    {
                        CardsInHand[i].transform.SetParent(zone.transform);
                        CardsInHand[i].GetComponent<ItemEnemyCard>().IsCardBack = false;
                        foundBulletCard = true;
                        foundWineCard = true;
                        break;
                    }
                }
                if (CardsInHand[i].cardIdGame == 9)
                {
                    if (grid.IsPlayerOutAttackRange(gameControl.players[1], gameControl.players[0].x, gameControl.players[0].y) && countRangeCard < 1 && checkBullet())
                    {
                        CardsInHand[i].transform.SetParent(zone.transform);
                        CardsInHand[i].GetComponent<ItemEnemyCard>().IsCardBack = false;
                        checkAttackCard = true;
                        countRangeCard++;
                        break;
                    }
                }
                if (CardsInHand[i].cardIdGame == 8)
                {
                    if (foundWineCard)
                    {
                        CardsInHand[i].transform.SetParent(zone.transform);
                        CardsInHand[i].GetComponent<ItemEnemyCard>().IsCardBack = false;
                        break;
                    }

                }
                if (CardsInHand[i].cardIdGame == 10)
                {
                    if (gameControl.players[1].heal <= 5)
                    {
                        CardsInHand[i].transform.SetParent(zone.transform);
                        CardsInHand[i].GetComponent<ItemEnemyCard>().IsCardBack = false;
                        break;
                    }
                }
            }
        }
    }
    /*void UpdateAiCanUseCard()
    {
        if (gameControl.players[1].playerTurn && !isUpdated && dice.hasRolledDice == true)
        {
            for (int i = 0; i < CardsInHand.Count; i++)
            {
                AiCanUseCard[i] = true;
            }
            isUpdated = true;
        }
        else
        {
            for (int i = 0; i < CardsInHand.Count; i++)
            {
                AiCanUseCard[i] = false;
            }
        }
    }
    */
    /*
    void ChooseCardToUse()
    {
        if (gameControl.players[1].playerTurn == true && dice.hasRolledDice == true)
        {
            useCardID = 0;
            int index = 0;
            int[] cardsId = new int[CardsInHand.Count];

            for (int i = 0; i < CardsInHand.Count; i++)
            {
                if (AiCanUseCard[i] == true)
                {
                    cardsId[index] = CardsInHand[i].cardIdGame;
                    index++;
                }
            }
            for (int i = 0; i < CardsInHand.Count; i++)
            {
                if (cardsId[i] == 9)
                {
                    if (grid.IsPlayerOutAttackRange(gameControl.players[1], gameControl.players[0].x, gameControl.players[0].y) && countRangeCard < 1)
                    {
                        useCardID = cardsId[i];
                        checkAttackCard = true;
                        countRangeCard++;
                        break;
                    }
                }
                if (cardsId[i] == 7)
                {
                    if (grid.IsPlayerInAttackRange(gameControl.players[1], gameControl.players[0].x, gameControl.players[0].y) && !foundBulletCard)
                    {
                        useCardID = cardsId[i];
                        foundBulletCard = true;
                        foundWineCard = true;
                        break;
                    }
                    if (checkAttackCard && !foundBulletCard)
                    {
                        useCardID = cardsId[i];
                        foundBulletCard = true;
                        foundWineCard = true;
                        break;
                    }

                }
                if (cardsId[i] == 8)
                {
                    if (foundWineCard)
                    {
                        useCardID = cardsId[i];
                        break;
                    }

                }
                if (cardsId[i] == 10)
                {
                    if (gameControl.players[1].heal <= 5)
                    {
                        useCardID = cardsId[i];
                        break;
                    }
                }
            }
            foreach (Transform child in hand.transform)
            {
                if (child.GetComponent<ItemEnemyCard>().cardIdGame == useCardID && dice.hasRolledDice == true)
                {
                    child.transform.SetParent(zone.transform);
                    child.GetComponent<ItemEnemyCard>().IsCardBack = false;
                    break;
                }
            }

        }
    }
    */

    void ResetFlag()
    {
        if (gameControl.players[0].playerTurn == true)
        {
            checkAttackCard = false;
            countRangeCard = 0;
            foundBulletCard = false;
            foundWineCard = false;
        }
    }
   
}
