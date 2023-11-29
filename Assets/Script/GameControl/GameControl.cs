
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class GameControl : MonoBehaviour
{
    private List<ItemCard> usedCards = new List<ItemCard>();
    private List<ItemEnemyCard> player2UsedCards = new List<ItemEnemyCard>();
    private List<ItemCardTutorial> cardTutorial = new List<ItemCardTutorial>();
    public Tutorial tutorial;
    internal List<Player> players;
    private GameObject map;
    private GridMap grid;
    [SerializeField] Tilemap targetTilemap;
    [SerializeField] Tilemap highlightTilemap;
    [SerializeField] Tilemap highlightTilemap2;
    [SerializeField] TileBase highlightTile;
    [SerializeField] TileBase highlightTile2;
    private GridManager gridManager;
    private TurnBase turn;
    private FireCanon playerShoot;
    private FireCanon player2Shoot;
    [SerializeField] Dice dice;
    Pathfinding pathfinding;
    public Text namePlayer;
    public Text namePlayer2;
    public Text healPlayer;
    public Text healPlayer2;
    public Text toolTip;
    private int currentPlayerTurn = 0;
    private Player selectedPlayer = null;
    public Text notification;
    private GameObject dropZone;
    private GameObject hand;
    public GameObject kraken;
    public GameObject chest;
    public GameObject gift;
    public GameObject healEffect;
    public GameObject rangeEffect;
    public GameObject dameUpEffect;
    public GameObject cardEvent;
    public GameObject cardEvent2;
    public GameObject chestEffect;
    private List<PathNode> highlight;
    internal int index = -1;
    private bool player2IsUseCard = false;
    private bool hasUpdate = false;
    private int countHeal = 0;
    internal bool isMoving;
    internal bool isHover;
    internal bool hasBullet;
    internal int countBullet = 0;
    internal bool checkRange;
    internal bool checkHeal;
    private bool checkEvent;
    private void Start()
    {
        grid = map.GetComponent<GridMap>();
        gridManager = map.GetComponent<GridManager>();
        turn = map.GetComponent<TurnBase>();
        pathfinding = targetTilemap.GetComponent<Pathfinding>();
        players = gridManager.GetAllPlayers();
        toolTip.gameObject.SetActive(false);
        playerShoot = players[0].GetComponent<FireCanon>();
        player2Shoot = players[1].GetComponent<FireCanon>();
    }
    void Awake()
    {
        map = GameObject.Find("Map");
        dropZone = GameObject.Find("Drop Zone");
        hand = GameObject.Find("Hand Player 1");
    }
    private void Update()
    {
        InfoPlayer();
        if (index == -1)
        {
            MovePlayer();
        }
        UsedCard();
        //###### 21/6
        if (players[1].playerTurn && dice.hasRolledDice)
        {
            ConfirmCardPlayer2();
        }
        CheckBullet(usedCards);
        CheckRange(usedCards);
        CheckHeal(usedCards);
        
        
    }
    private void MovePlayer()
    {
        Vector3 worldPointPlayer;
        Vector3 worldPoint;
        Vector3Int playerPosition = Vector3Int.zero;
        if (players[currentPlayerTurn].playerTurn)
        {
            worldPointPlayer = players[currentPlayerTurn].transform.position;
            playerPosition = targetTilemap.WorldToCell(worldPointPlayer);
        }
        else if (players[currentPlayerTurn + 1].playerTurn)
        {
            worldPointPlayer = players[currentPlayerTurn + 1].transform.position;
            playerPosition = targetTilemap.WorldToCell(worldPointPlayer);
        }
        if ((players[currentPlayerTurn].playerTurn == true || players[currentPlayerTurn + 1].playerTurn == true) && turn.currentPhase == TurnBase.PHASE.BATTLE)
        {
            if (currentPlayerTurn < players.Count - 1 && players[currentPlayerTurn + 1].playerTurn == true)
            {
                currentPlayerTurn++;
            }
            if (players[currentPlayerTurn].x == playerPosition.x && players[currentPlayerTurn].y == playerPosition.y)
            {
                ClearHighlightTiles();
                selectedPlayer = players[currentPlayerTurn];
                if (tutorial != null && currentPlayerTurn == 1 && tutorial.temp < 10)
                {
                    Deselect();
                    tutorial.text.text = "TIẾP TỤC LẮC XÚC XẮC";
                    tutorial.diceRenderer.sortingLayerName = "UI";
                    tutorial.panel.SetActive(true);
                }
                int temp = dice.temp;
                if (temp == 0)
                {
                    return;
                }
                selectedPlayer.moveDistance = dice.temp;
                if (selectedPlayer != null)
                {
                    List<PathNode> toHighlight = new List<PathNode>();
                    List<PathNode> toHighlight2 = new List<PathNode>();
                    pathfinding.Clear();
                    pathfinding.CalculateWalkableTerrain(playerPosition.x, playerPosition.y, players[currentPlayerTurn].moveDistance, ref toHighlight);
                    pathfinding.CalculateWalkableTerrain(playerPosition.x, playerPosition.y, players[currentPlayerTurn].range, ref toHighlight2);
                    for (int i = 1; i < toHighlight.Count; i++)
                    {
                        if (grid.CheckPositionPlayer(toHighlight[i].xPos, toHighlight[i].yPos))
                        {
                            continue;
                        }
                        highlightTilemap.SetTile(new Vector3Int(toHighlight[i].xPos, toHighlight[i].yPos, 0), highlightTile);
                    }
                    highlight = toHighlight;
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (players[i].x != playerPosition.x || players[i].y != playerPosition.y)
                        {
                            for (int j = 1; j < toHighlight2.Count; j++)
                            {
                                if (toHighlight2[j].xPos == players[i].x && toHighlight2[j].yPos == players[i].y && selectedPlayer.canBeAttack)
                                {
                                    highlightTilemap2.SetTile(new Vector3Int(players[i].x, players[i].y, 0), highlightTile2);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int clickPosition = targetTilemap.WorldToCell(worldPoint);
        Vector3 player1Position = players[0].transform.position;
        //15/6 sua bot di chuyen
        if (selectedPlayer != null)
        {
            if ((Input.GetMouseButtonDown(0) && players[0].playerTurn == true) || (players[1].playerTurn == true))
            {
                List<PathNode> path = new List<PathNode>();
                if (players[0].playerTurn == true)
                {
                    index = 1;
                    path = pathfinding.TrackBackPath(selectedPlayer, clickPosition.x, clickPosition.y);
                }
                else if (players[1].playerTurn && player2IsUseCard)
                {
                    index = Random.Range(1, highlight.Count);
                    PathNode randomNode = highlight[index];
                    if (selectedPlayer.canBeAttack)
                    {
                        path = pathfinding.TrackBackPath(selectedPlayer, players[0].x, players[0].y);
                    }
                    else
                    {
                        path = pathfinding.TrackBackPath(selectedPlayer, randomNode.xPos, randomNode.yPos);
                    }
                }
                if (usedCards.Count > 0)
                {
                    index = -1;
                }
                else if (cardTutorial.Count > 0)
                {
                    index = -1;
                }
                else if (selectedPlayer.shouldAttack && selectedPlayer.canBeAttack)
                {
                    if (players[0].playerTurn)
                    {
                        playerShoot.Shoot(worldPoint);
                    }
                    else
                    {
                        player2Shoot.Shoot(player1Position);
                    }
                    selectedPlayer.canBeAttack = false;
                    StartCoroutine(TakeDame());
                }
                else if (path != null && path.Count > 0 && !isMoving && !isHover)
                {
                    StartCoroutine(MovePlayerCoroutine(path));
                }
                else
                {
                    index = -1;
                }
            }
        }
    }
    private IEnumerator TakeDame()
    {
        yield return new WaitForSeconds(0.75f);
        if (currentPlayerTurn == 0)
        {
            players[1].heal -= selectedPlayer.dame;
            toolTip.text = "-" + selectedPlayer.dame + " độ bền";
            toolTip.color = new Color(1f, 0.15f, 0f);
            if (tutorial != null)
            {
                toolTip.gameObject.transform.position = new Vector3(players[1].transform.position.x - 0.15f, players[1].transform.position.y + 2.25f);
            }
            else
            {
                toolTip.gameObject.transform.position = new Vector3(players[1].transform.position.x - 0.15f, players[1].transform.position.y + 3f);
            }
            toolTip.gameObject.SetActive(true);
            StartCoroutine(DelayHideNoti());
            Deselect();
        }
        else if (currentPlayerTurn == 1)
        {
            players[0].heal -= selectedPlayer.dame;
            toolTip.text = "-" + selectedPlayer.dame + " độ bền";
            toolTip.color = new Color(1f, 0.15f, 0f);
            if (tutorial != null)
            {
                toolTip.gameObject.transform.position = new Vector3(players[0].transform.position.x - 0.15f, players[0].transform.position.y + 2.25f);
            }
            else
            {
                toolTip.gameObject.transform.position = new Vector3(players[0].transform.position.x - 0.15f, players[0].transform.position.y + 3f);
            }
            toolTip.gameObject.SetActive(true);
            StartCoroutine(DelayHideNoti());
            Deselect();
        }
    }
    private IEnumerator MovePlayerCoroutine(List<PathNode> path)
    {
        isMoving = true;
        isHover = true;
        ClearHighlightTiles();
        for (int i = path.Count - 1; i >= 0; i--)
        {
            highlightTilemap.SetTile(new Vector3Int(path[i].xPos, path[i].yPos, 0), highlightTile);
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = path.Count - 1; i >= 0; i--)
        {
            selectedPlayer.GetComponent<MapElement>().MovePlayer(path[i].xPos, path[i].yPos);
            yield return new WaitForSeconds(0.205f);
        }
        selectedPlayer.x = path[0].xPos;
        selectedPlayer.y = path[0].yPos;
        CreateEvent(selectedPlayer);
        if (checkEvent)
        {
            yield return new WaitForSeconds(3f);
        }
        Deselect();
    }
    private void Deselect()
    {
        pathfinding.Clear();
        players[currentPlayerTurn].playerTurn = false;
        if (currentPlayerTurn + 1 >= players.Count)
        {
            currentPlayerTurn = 0;
        }
        else
        {
            currentPlayerTurn += 1;
        }
        players[currentPlayerTurn].playerTurn = true;
        selectedPlayer.range = 1;
        selectedPlayer.dame = 1;
        selectedPlayer.canBeAttack = false;
        selectedPlayer.shouldAttack = false;
        selectedPlayer = null;
        dice.temp = 0;
        dice.hasRolledDice = false;
        turn.currentPhase = TurnBase.PHASE.INIT;
        player2IsUseCard = false;
        index = -1;
        hasUpdate = false;
        isMoving = false;
        isHover = false;
        countHeal = 0;
        checkHeal = false;
        checkRange = false;
        countBullet = 0;
        checkEvent = false;
    }
    private void ClearHighlightTiles()
    {
        highlightTilemap.ClearAllTiles();
        highlightTilemap2.ClearAllTiles();
    }
    private void InfoPlayer()
    {
        namePlayer.text = players[0].Name;
        namePlayer2.text = players[1].Name;
        healPlayer.text = players[0].heal.ToString();
        healPlayer2.text = players[1].heal.ToString();
    }
    private void UsedCard()
    {
        ItemCard[] cardsToAdd = dropZone.GetComponentsInChildren<ItemCard>();

        ItemEnemyCard[] cardsToAdd2 = dropZone.GetComponentsInChildren<ItemEnemyCard>();

        ItemCardTutorial[] cardsToAdd3 = dropZone.GetComponentsInChildren<ItemCardTutorial>();
        foreach (ItemCard card in cardsToAdd)
        {
            if (!usedCards.Contains(card))
            {
                usedCards.Add(card);
            }
        }
        foreach (ItemEnemyCard card2 in cardsToAdd2)
        {
            if (!player2UsedCards.Contains(card2))
            {
                player2UsedCards.Add(card2);
            }
        }
        foreach (ItemCardTutorial card3 in cardsToAdd3)
        {
            if (!cardTutorial.Contains(card3))
            {
                cardTutorial.Add(card3);
            }
        }
        usedCards.RemoveAll(card => card == null || !card.transform.IsChildOf(dropZone.transform));
        player2UsedCards.RemoveAll(card2 => card2 == null || !card2.transform.IsChildOf(dropZone.transform));
        cardTutorial.RemoveAll(card3 => card3 == null || !card3.transform.IsChildOf(dropZone.transform));
    }
    public void ConfirmCard()
    {
        if(usedCards.Count >0)
        {
            ApplyCardEffects(usedCards);
            DeleteCard();
        }    
   
    }
    public void ConfirmCardTutorial()
    {
        if(cardTutorial.Count>0)
        {
            ApplyCardEffectsTutorial(cardTutorial);
            DeleteCard();
            isMoving = false;
        }    
      
    }
    public void ConfirmCardPlayer2()
    {
        if (!hasUpdate)
        {
            StartCoroutine(ConfirmCardPlayer2WithDelay());
            hasUpdate = true;
        }
    }
    private IEnumerator ConfirmCardPlayer2WithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        if (player2UsedCards.Count > 0)
        {
            yield return new WaitForSeconds(2.5f);
            ApplyCardEffectsForPlayer2(player2UsedCards);
            DeleteCard();
        }
        yield return new WaitForSeconds(0.1f);
        if (player2UsedCards.Count == 0)
        {
            yield return new WaitForSeconds(2.5f);
            player2IsUseCard = true;
        }
    }
    void CheckBullet(List<ItemCard> cards)
    {
        if (usedCards.Count > 0)
        {
            foreach (ItemCard card in cards)
            {
                if (card.cardIdGame == 7)
                {
                    hasBullet = true;
                }
                else
                {
                    hasBullet = false;
                }
            }
        }
        else
        {
            hasBullet = false;
        }
    }
  public void CheckRange(List<ItemCard> cards)
    {
        foreach (ItemCard card in cards)
        {
            if (selectedPlayer.range >= dice.temp)
            {
                checkRange = true;
            }
            else
            {
                checkRange = false;
            }
        }

    }
   public void CheckHeal(List<ItemCard> cards)
    {
        foreach (ItemCard card in cards)
        {
            if (selectedPlayer.heal >= selectedPlayer.maxHeal)
            {
                checkHeal = true;
            }
            else
            {
                checkHeal = false;
            }
        }
    }
    private void ApplyCardEffects(List<ItemCard> cards)
    {
        toolTip.text = "";
        foreach (ItemCard card in cards)
        {
            switch (card.cardIdGame)
            {
                case 7:
                    selectedPlayer.canBeAttack = true;
                    countBullet++;
                    index = -1;
                    break;
                case 8:
                    ApplyDamageUpEffect();
                    break;
                case 9:
                    if (selectedPlayer.range < dice.temp)
                    {
                        ApplyRangeUpEffect();
                    }
                    else
                    {
                        notification.text = "Tầm đánh bằng số ô di chuyển ";
                        notification.gameObject.SetActive(true);
                        card.transform.SetParent(hand.transform);
                        StartCoroutine(DelayHideNoti());
                    }
                    index = -1;
                    break;
                case 10:
                    if (selectedPlayer.heal < selectedPlayer.maxHeal)
                    {
                        ApplyHealingEffect();
                    }
                    else
                    {
                        notification.text = "Độ bền đã đạt đến giới hạn ";
                        notification.gameObject.SetActive(true);
                        card.transform.SetParent(hand.transform);
                        StartCoroutine(DelayHideNoti());
                    }
                    break;
            }
        }
    }
    private void ApplyCardEffectsForPlayer2(List<ItemEnemyCard> cards)
    {
        toolTip.text = "";
        foreach (ItemEnemyCard card in cards)
        {
            switch (card.cardIdGame)
            {
                case 7:
                    selectedPlayer.canBeAttack = true;
                    index = -1;
                    break;
                case 8:
                    ApplyDamageUpEffect();
                    break;
                case 9:
                    ApplyRangeUpEffect();
                    index = -1;
                    break;
                case 10:
                    ApplyHealingEffect();
                    break;
            }
        }
    }
    private void ApplyCardEffectsTutorial(List<ItemCardTutorial> cards)
    {
        toolTip.text = "";
        foreach (ItemCardTutorial card in cards)
        {
            switch (card.cardIdGame)
            {
                case 7:
                    selectedPlayer.canBeAttack = true;
                    countBullet++;
                    index = -1;
                    break;
                case 8:
                    ApplyDamageUpEffect();
                    break;
                case 9:
                    if (selectedPlayer.range < dice.temp)
                    {
                        ApplyRangeUpEffect();
                    }
                    else
                    {
                        notification.text = "Tầm đánh bằng số ô di chuyển ";
                        notification.gameObject.SetActive(true);
                        card.transform.SetParent(hand.transform);
                        StartCoroutine(DelayHideNoti());
                    }
                    index = -1;
                    break;
                case 10:
                    if (selectedPlayer.heal < selectedPlayer.maxHeal)
                    {
                        ApplyHealingEffect();
                    }
                    else
                    {
                        notification.text = "Độ bền đã đạt đến giới hạn ";
                        notification.gameObject.SetActive(true);
                        card.transform.SetParent(hand.transform);
                        StartCoroutine(DelayHideNoti());
                    }
                    break;
            }
        }
    }
    private void ApplyDamageUpEffect()
    {
        GameObject dameUp = Instantiate(dameUpEffect, selectedPlayer.transform.position, Quaternion.identity);
        selectedPlayer.dame += 1;
        toolTip.color = Color.white;
        toolTip.text = toolTip.text + "\n+" + selectedPlayer.dame + " sát thương";

        if (tutorial != null)
        {
            toolTip.gameObject.transform.position = new Vector3(selectedPlayer.transform.position.x - 0.15f, selectedPlayer.transform.position.y + 2.25f);
        }
        else
        {
            toolTip.gameObject.transform.position = new Vector3(selectedPlayer.transform.position.x - 0.15f, selectedPlayer.transform.position.y + 3f);
        }
        toolTip.gameObject.SetActive(true);
        StartCoroutine(DelayHideNoti());
        Destroy(dameUp, 3f);
    }
    private void ApplyRangeUpEffect()
    {
        GameObject rangeUp = Instantiate(rangeEffect, selectedPlayer.transform.position, Quaternion.identity);
        selectedPlayer.range += 1;
        toolTip.color = Color.white;
        toolTip.text = toolTip.text + "\n+" + selectedPlayer.range + " tầm đánh";

        if (tutorial != null)
        {
            toolTip.gameObject.transform.position = new Vector3(selectedPlayer.transform.position.x - 0.15f, selectedPlayer.transform.position.y + 2.25f);
        }
        else
        {
            toolTip.gameObject.transform.position = new Vector3(selectedPlayer.transform.position.x - 0.15f, selectedPlayer.transform.position.y + 3f);
        }
        toolTip.gameObject.SetActive(true);
        StartCoroutine(DelayHideNoti());
        Destroy(rangeUp, 3f);
    }
    private void ApplyHealingEffect()
    {
        GameObject healing = Instantiate(healEffect, selectedPlayer.transform.position, Quaternion.identity);
        selectedPlayer.heal += 1;
        countHeal += 1;
        toolTip.color = Color.white;
        toolTip.text = toolTip.text + "\n+" + countHeal + " độ bền";
        if (tutorial != null)
        {
            toolTip.gameObject.transform.position = new Vector3(selectedPlayer.transform.position.x - 0.15f, selectedPlayer.transform.position.y + 2.25f);
        }
        else
        {
            toolTip.gameObject.transform.position = new Vector3(selectedPlayer.transform.position.x - 0.15f, selectedPlayer.transform.position.y + 3f);
        }
        toolTip.gameObject.SetActive(true);
        StartCoroutine(DelayHideNoti());
        Destroy(healing, 3f);
    }
    ////##########################
    public void DeleteCard()
    {
        if (usedCards.Count > 0 || player2UsedCards.Count > 0 || cardTutorial.Count > 0)
        {
            foreach (Transform card in dropZone.transform)
            {
                Destroy(card.gameObject);
            }
            usedCards.Clear();
            player2UsedCards.Clear();
            cardTutorial.Clear();
        }
    }
    IEnumerator DelayHideNoti()
    {
        yield return new WaitForSeconds(2f);
        toolTip.gameObject.SetActive(false);
        notification.gameObject.SetActive(false);
    }
    public IEnumerator DelayChest()
    {
        yield return new WaitForSeconds(1f);
        GameObject spawnedGift = Instantiate(gift, new Vector3(7.8f, 6.3f), Quaternion.identity);
        GameObject effectChest = Instantiate(chestEffect, new Vector3(7.8f, 6.3f), Quaternion.identity);
        Destroy(spawnedGift, 1f);
        yield return new WaitForSeconds(1f);
        if (players[1].playerTurn)
        {
            Instantiate(cardEvent2, transform.position, transform.rotation);
        }
        else 
        {
            Instantiate(cardEvent, transform.position, transform.rotation);
        }
    }
    public void CreateEvent(Player player)
    {
        int random = Random.Range(1, 3);
        if (grid.CheckEvent(player.x, player.y) && random == 1)
        {
            grid.Set(player.x, player.y, 0);
            gridManager.UpdateTile(player.x, player.y);
            if (player.y % 2 == 0)
            {
                if (grid.CheckWalkable(player.x + 1, player.y) && !grid.CheckEvent(player.x + 1, player.y) && !grid.CheckPositionPlayer(player.x + 1, player.y))
                {
                    GameObject spawnedEnemy = Instantiate(kraken, new Vector3(player.x * 1f + 1f, player.y * 0.75f), Quaternion.identity);
                    spawnedEnemy.GetComponent<MonsterAttack>();
                }
                else if (grid.CheckWalkable(player.x, player.y + 1) && !grid.CheckEvent(player.x, player.y + 1) && !grid.CheckPositionPlayer(player.x, player.y + 1))
                {
                    GameObject spawnedEnemy = Instantiate(kraken, new Vector3(player.x * 1f + 0.5f, player.y * 0.75f + 0.75f), Quaternion.identity);
                }
                else if (grid.CheckWalkable(player.x, player.y - 1) && !grid.CheckEvent(player.x, player.y - 1) && !grid.CheckPositionPlayer(player.x, player.y - 1))
                {
                    GameObject spawnedEnemy = Instantiate(kraken, new Vector3(player.x * 1f + 0.5f, player.y * 0.75f - 0.75f), Quaternion.identity);
                }
                else if (grid.CheckWalkable(player.x - 1, player.y) && !grid.CheckEvent(player.x - 1, player.y) && !grid.CheckPositionPlayer(player.x - 1, player.y))
                {
                    GameObject spawnedEnemy = Instantiate(kraken, new Vector3(player.x * 1f - 1f, player.y * 0.75f), Quaternion.identity);
                }
                else if (grid.CheckWalkable(player.x - 1, player.y + 1) && !grid.CheckEvent(player.x - 1, player.y + 1) && !grid.CheckPositionPlayer(player.x - 1, player.y + 1))
                {
                    GameObject spawnedEnemy = Instantiate(kraken, new Vector3(player.x * 1f - 0.5f, player.y * 0.75f + 0.75f), Quaternion.identity);

                }
                else if (grid.CheckWalkable(player.x - 1, player.y - 1) && !grid.CheckEvent(player.x - 1, player.y - 1) && !grid.CheckPositionPlayer(player.x - 1, player.y - 1))
                {
                    GameObject spawnedEnemy = Instantiate(kraken, new Vector3(player.x * 1f - 0.5f, player.y * 0.75f - 0.75f), Quaternion.identity);
                }
            }
            else
            {
                if (grid.CheckWalkable(player.x + 1, player.y) && !grid.CheckEvent(player.x + 1, player.y) && !grid.CheckPositionPlayer(player.x + 1, player.y))
                {
                    GameObject spawnedEnemy = Instantiate(kraken, new Vector3(player.x * 1f + 1.5f, player.y * 0.75f), Quaternion.identity);
                }
                else if (grid.CheckWalkable(player.x, player.y + 1) && !grid.CheckEvent(player.x, player.y + 1) && !grid.CheckPositionPlayer(player.x, player.y + 1))
                {
                    GameObject spawnedEnemy = Instantiate(kraken, new Vector3(player.x * 1f, player.y * 0.75f + 0.75f), Quaternion.identity);
                }
                else if (grid.CheckWalkable(player.x, player.y - 1) && !grid.CheckEvent(player.x, player.y - 1) && !grid.CheckPositionPlayer(player.x, player.y - 1))
                {
                    GameObject spawnedEnemy = Instantiate(kraken, new Vector3(player.x * 1f, player.y * 0.75f - 0.75f), Quaternion.identity);
                }
                else if (grid.CheckWalkable(player.x - 1, player.y) && !grid.CheckEvent(player.x - 1, player.y) && !grid.CheckPositionPlayer(player.x - 1, player.y))
                {
                    GameObject spawnedEnemy = Instantiate(kraken, new Vector3(player.x * 1f + 0.5f - 1f, player.y * 0.75f), Quaternion.identity);
                }
                else if (grid.CheckWalkable(player.x + 1, player.y + 1) && !grid.CheckEvent(player.x + 1, player.y + 1) && !grid.CheckPositionPlayer(player.x + 1, player.y + 1))
                {
                    GameObject spawnedEnemy = Instantiate(kraken, new Vector3(player.x * 1f + 1f, player.y * 0.75f + 0.75f), Quaternion.identity);
                }
                else if (grid.CheckWalkable(player.x + 1, player.y - 1) && !grid.CheckEvent(player.x + 1, player.y + 1) && !grid.CheckPositionPlayer(player.x + 1, player.y - 1))
                {
                    GameObject spawnedEnemy = Instantiate(kraken, new Vector3(player.x * 1f + 1f, player.y * 0.75f - 0.75f), Quaternion.identity);
                }
            }

        }
        else if (grid.CheckEvent(player.x, player.y) && random == 2)
        {
            if (players[0].playerTurn)
            {
                GameObject spawnedChest = Instantiate(chest, new Vector3(7.8f, 5.65f), Quaternion.identity);
                Destroy(spawnedChest, 1f);
                StartCoroutine(DelayChest());
                checkEvent = true;
            }
            else
            {
                GameObject spawnedChest = Instantiate(chest, new Vector3(7.8f, 5.65f), Quaternion.identity);
                Destroy(spawnedChest, 1f);
                StartCoroutine(DelayChest());
                checkEvent = true;
            }
            grid.Set(player.x, player.y, 3);
            gridManager.UpdateTile(player.x, player.y);
        }
    }
}