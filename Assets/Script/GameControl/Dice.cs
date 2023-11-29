using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Dice : MonoBehaviour
{
    private Sprite[] diceSides;
    private SpriteRenderer rend;
    public Deck deck;
    internal int rollResult = 0;
    internal int rollCount = 0;
    internal List<Player> players;
    public bool hasRolledDice;
    public int temp = 0;
    public AudioClip clip;
    public AudioSource source;
    int currentPlayerTurn = 0;
    [SerializeField] GridManager gridManager;
    [SerializeField] Tilemap targetTilemap;
    public Tutorial tutorial;
    internal TurnBase turnBase;
    private bool isRolling = false;
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        turnBase = targetTilemap.GetComponent<TurnBase>();
        players = gridManager.GetAllPlayers();
    }
    private void Update()
    {
      
        if (CanRollDice() && players[1].playerTurn == true)
        {
            StartCoroutine(RollTheDiceForPlayer2());
        }
    }
    private void OnMouseDown()
    {
        if(Time.timeScale>0f)
        {
            OnButtonMouse();
        }
     
    }
    private void OnButtonMouse()
    {
        if (CanRollDice() && isRolling == false)
        {
            StartCoroutine(RollTheDice());
            source.PlayOneShot(clip);
            hasRolledDice = true;
            if (currentPlayerTurn == 0 && turnBase.currentPhase == TurnBase.PHASE.BEGIN)
            {
                StartCoroutine(AutoRollDiceForBegin());
            }
            if (tutorial != null)
            {
                tutorial.panel.SetActive(false);
            }
            if (tutorial != null && (tutorial.temp == 2 || tutorial.temp==5 || tutorial.temp== 7 || tutorial.temp == 10))
            {
                tutorial.temp++;
            }

        }
    }
    public bool CanRollDice()
    {
        if (turnBase.currentPhase == TurnBase.PHASE.BEGIN)
        {
            if (rollCount < players.Count)
            {
                return true;
            }
            return false;
        }
        else if (turnBase.currentPhase == TurnBase.PHASE.BATTLE)
        {
            if (hasRolledDice == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    private IEnumerator AutoRollDiceForBegin()
    {
        yield return new WaitForSeconds(2.5f);
        source.PlayOneShot(clip);
        if (currentPlayerTurn == 0)
        {
            currentPlayerTurn = 1;
            int randomDiceSide = 0;
            for (int i = 0; i <= 20; i++)
            {
                randomDiceSide = Random.Range(0, 6);
                rend.sprite = diceSides[randomDiceSide];
                yield return new WaitForSeconds(0.05f);
            }
            rollResult = randomDiceSide + 1;
            if (turnBase.currentPhase == TurnBase.PHASE.BEGIN)
            {
                if (temp < rollResult)
                {
                    players[currentPlayerTurn].playerTurn = true;
                    turnBase.currentPhase = TurnBase.PHASE.INIT;
                    temp = 0;
                }
                else if (temp >= rollResult)
                {
                    currentPlayerTurn = 0;
                    players[currentPlayerTurn].playerTurn = true;
                    turnBase.currentPhase = TurnBase.PHASE.INIT;
                    temp = 0;
                }
            }
            rollCount++;
            hasRolledDice = false;
    
        }
    }
    public IEnumerator RollTheDice()
    {
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }
        rollResult = randomDiceSide + 1;
        rollCount++;
        temp = rollResult;
    }
    public IEnumerator RollTheDiceForPlayer2()
    {
        if (!hasRolledDice && !isRolling)
        {
            isRolling = true;
            yield return new WaitForSeconds(2.25f);
            source.PlayOneShot(clip);
            int randomDiceSide = 0;
            for (int i = 0; i <= 20; i++)
            {
                randomDiceSide = Random.Range(0, 6);
                rend.sprite = diceSides[randomDiceSide];
                yield return new WaitForSeconds(0.05f);
            }
            rollResult = randomDiceSide + 1;
            rollCount++;
            temp = rollResult;
            hasRolledDice = true;
            isRolling = false;
        }
    }
}
