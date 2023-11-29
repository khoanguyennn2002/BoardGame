using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Cheat : MonoBehaviour
{
    public Toggle deckRenew;
    public Deck deck;
    public GameObject cheat;
    public ToggleGroup toggleGroup;
    public GameObject[] inputF;
    public GameObject healField;
    public List<Player> players = new List<Player>();
    public GridMap grid;
    public GameControl gameControl;
    public GridManager gridManager;
    public GameObject cardCheat;
    public CardInHand cardInHand;
    public GameObject hand;
    void Start()
    {

        players = gridManager.GetAllPlayers();
    }
    private void SetHeal()
    {
        int h = -1;

        if (healField.GetComponent<Text>().text != "")
        {
            h = Int32.Parse(healField.GetComponent<Text>().text);
        }
        if (h >= 0)
        {
            players[1].heal = h;
        }
    }
    public void OpenCheat()
    {
        if (cheat != null)
        {
            cheat.SetActive(true);
        }
    }
    public void CloseCheat()
    {
        if (cheat != null)
        {
            cheat.SetActive(false);
        }
    }
    private void MovePlayer()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
 
        if (toggle != null)
        {
            if (toggle.name == "ToggleTelebotToEnemy")
            {
                int x = players[1].x;
                int y = players[1].y;
                kiemtravitri(ref x, ref y);
                players[0].GetComponent<MapElement>().MovePlayer(x, y);
                players[0].x = x;
                players[0].y = y;
            }
            else if (toggle.name == "ToggleTelebotToEnemy1Block")
            {
                int x = players[1].x;
                int y = players[1].y;
                kiemtravitri(ref x, ref y);
                kiemtravitri(ref x, ref y);
                players[0].GetComponent<MapElement>().MovePlayer(x, y);
                players[0].x = x;
                players[0].y = y;
            }
            else if (toggle.name == "ToggleTelebotToSecret")
            {
                int x = 0;
                int y = 0;
                for (int i = 0; i < gridManager.length; i++)
                {
                    for (int j = 0; j < gridManager.height; j++)
                    {
                        if (grid.CheckEvent(i, j))
                        {
                            x = i;
                            y = j;
                            break;
                        }
                    }

                }
                players[0].GetComponent<MapElement>().MovePlayer(x, y);
                players[0].x = x;
                players[0].y = y;
                gameControl.CreateEvent(players[0]);

            }
        }
    }
    private void ResetDeck()
    {
        if(deckRenew.isOn==true)
        {
            deck.deckSize = 0;
        }    
    }
    private void kiemtravitri(ref int x, ref int y)
    {
         if (y%2==0)
        {
            if (grid.CheckWalkable(x + 1, y))
            {
                x = x + 1;

            }
            else if (grid.CheckWalkable(x - 1, y))
            {
                x = x - 1;
            }
            else if (grid.CheckWalkable(x, y + 1))
            {
                y = y + 1;
            }
            else if (grid.CheckWalkable(x, y - 1))
            {
                y = y - 1;
            }
            else if (grid.CheckWalkable(x - 1, y + 1))
            {
                x = x - 1;
                y = y + 1;
            }
            
            else if (grid.CheckWalkable(x - 1, y - 1))
            {
                x = x - 1;
                y = y - 1;
            }

        }
        else
        {
            if (grid.CheckWalkable(x + 1, y))
            {
                x = x + 1;

            }
            else if (grid.CheckWalkable(x - 1, y))
            {
                x = x - 1;
            }
           else if (grid.CheckWalkable(x, y + 1))
            {
                y = y + 1;
            }
            else if (grid.CheckWalkable(x, y - 1))
            {
                y = y - 1;
            }
            else if (grid.CheckWalkable(x + 1, y + 1))
            {
                x = x + 1;
                y = y + 1;
            }
            else if (grid.CheckWalkable(x + 1, y - 1))
            {
                x = x + 1;
                y = y - 1;
            }
        }
    }
    //private bool checkChan(int y)
    //{
    //    if (y % 2 == 0)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
    private void DrawCardCheat()
    {
        for (int i = 0; i < 4; i++)
        {
            int j;
            if (inputF[i].GetComponent<Text>().text == "")
            {
                j = 0;
            }
            else
            {
                foreach (Transform card in hand.transform)
                {
                    Destroy(card.gameObject);
                }
                cardInHand.CardsInHand.Clear();
                j = Int32.Parse(inputF[i].GetComponent<Text>().text);
            }
            for (int k = 0; k < j; k++)
            {
                cardCheat.GetComponent<ItemCardTutorial>().cardIdData = 7 + i;
                Instantiate(cardCheat, transform.position, transform.rotation);
                          }
        }
    }
    public void CheatForFull()
    {
        ResetDeck();
        SetHeal();
        MovePlayer();
        DrawCardCheat();
    }
}
