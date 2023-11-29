using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class TurnBase : MonoBehaviour
{
    List<Player> players;
    [SerializeField] Deck deck;
    [SerializeField] GridManager gridManager;
    [SerializeField] Dice dice;
    [SerializeField] LimitCardInHand HandPlayer1;
    [SerializeField] LimitCardInHand HandPlayer2;
    [SerializeField] Tutorial tutorial;
    public enum PHASE
   {
        BEGIN,
        INIT,
        DRAW,
        BATTLE,
   };
   public PHASE currentPhase;
    void Start()
    {
        currentPhase = PHASE.BEGIN;
        players = gridManager.GetAllPlayers();
        if(tutorial==null)
        {
            StartCoroutine(deck.StartGame());
        }    
      
    }
    void Update()
    {

      
        if (currentPhase == PHASE.INIT)
        {

            dice.hasRolledDice = false;
            currentPhase = PHASE.DRAW;


        }
        else if (currentPhase == PHASE.DRAW && players[0].playerTurn)
        {

            if(tutorial != null)
            {
                currentPhase = PHASE.BATTLE;
            }
            else
            {
                if (HandPlayer1.CountCard < 10)
                {
                    deck.Draw();
                    currentPhase = PHASE.BATTLE;
                }
                else
                {
                    currentPhase = PHASE.BATTLE;
                }
            }
        }
        else if (currentPhase == PHASE.DRAW && players[1].playerTurn)
        {
            if (tutorial != null)
            {
                currentPhase = PHASE.BATTLE;
            }
            else
            {
                if (HandPlayer2.CountCard < 10)
                {
                    deck.Draw2();
                    currentPhase = PHASE.BATTLE;
                }
                else
                {
                    currentPhase = PHASE.BATTLE;
                }
            }
            
        }
    }
}
