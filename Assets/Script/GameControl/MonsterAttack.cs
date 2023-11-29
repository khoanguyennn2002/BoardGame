using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MonsterAttack : MonoBehaviour
{
    public Monster monster;
    public Tilemap targetTilemap;
    public GameObject player;
    public TurnBase turn;
    public GameObject map;
    public GridMap grid;
    public Animator animator;
    public GameObject krakenAttackEffect;
    private List<Player> players;
    public Text toolTip;
    private void Start()
    {
        
        map = GameObject.Find("Map");
        grid=map.GetComponent<GridMap>();
        targetTilemap = map.GetComponent<Tilemap>();
        turn = map.GetComponent<TurnBase>();
        player = GameObject.Find("Main Camera");
        players = player.GetComponent<GameControl>().players;
        toolTip = player.GetComponent<GameControl>().toolTip;
        animator = monster.GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if (turn.currentPhase == TurnBase.PHASE.DRAW)
        {
            monster.canAttack = false;
        }
        AttackPlayer();
    }
    void AttackPlayer()
    {
        Vector3 worldPointPlayer;
        Vector3 monsterPoint;
        Vector3Int monsterPosition;
        Vector3Int playerPosition = Vector3Int.zero;
        monsterPoint = monster.transform.position;
        monsterPosition = targetTilemap.WorldToCell(monsterPoint);
        //if(monsterPosition.y%2==0)
        //{

        //}    
        if (players[0].playerTurn)
        {
            worldPointPlayer = players[1].transform.position;
            playerPosition = targetTilemap.WorldToCell(worldPointPlayer);
            if (monsterPosition.y % 2 == 0)
            {
                if ((monsterPosition.x - 1 == playerPosition.x && monsterPosition.y == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[1]);
                }
                else if ((monsterPosition.x + 1 == playerPosition.x && monsterPosition.y == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[1]);
                }
                else if ((monsterPosition.x - 1 == playerPosition.x && monsterPosition.y - 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[1]);
                }
                else if ((monsterPosition.x - 1 == playerPosition.x && monsterPosition.y + 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[1]);
                }
                else if ((monsterPosition.x == playerPosition.x && monsterPosition.y + 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[1]);
                }
                else if ((monsterPosition.x == playerPosition.x && monsterPosition.y - 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[1]);
                }
            }
            else
            {
                if ((monsterPosition.x - 1 == playerPosition.x && monsterPosition.y == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[1]);
                }
                else if ((monsterPosition.x + 1 == playerPosition.x && monsterPosition.y == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[1]);

                }
                else if ((monsterPosition.x == playerPosition.x && monsterPosition.y - 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[1]);
                }
                else if ((monsterPosition.x == playerPosition.x && monsterPosition.y + 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[1]);
                }
                else if ((monsterPosition.x + 1 == playerPosition.x && monsterPosition.y + 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[1]);
                }
                else if ((monsterPosition.x + 1 == playerPosition.x && monsterPosition.y - 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[1]);
                }
            }

        }
        else
        {
            worldPointPlayer = players[0].transform.position;
            playerPosition = targetTilemap.WorldToCell(worldPointPlayer);
            if (monsterPosition.y % 2 == 0)
            {
                if ((monsterPosition.x - 1 == playerPosition.x && monsterPosition.y == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[0]);
                }
                else if ((monsterPosition.x + 1 == playerPosition.x && monsterPosition.y == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[0]);
                }
                else if ((monsterPosition.x - 1 == playerPosition.x && monsterPosition.y - 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[0]);
                }
                else if ((monsterPosition.x - 1 == playerPosition.x && monsterPosition.y + 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[0]);
                }
                else if ((monsterPosition.x == playerPosition.x && monsterPosition.y + 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[0]);
                }
                else if ((monsterPosition.x == playerPosition.x && monsterPosition.y - 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[0]);
                }
            }
            else
            {
                if ((monsterPosition.x - 1 == playerPosition.x && monsterPosition.y == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[0]);
                }
                else if ((monsterPosition.x + 1 == playerPosition.x && monsterPosition.y == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[0]);
                }
                else if ((monsterPosition.x == playerPosition.x && monsterPosition.y - 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[0]);
                }
                else if ((monsterPosition.x == playerPosition.x && monsterPosition.y + 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[0]);
                }
                else if ((monsterPosition.x + 1 == playerPosition.x && monsterPosition.y + 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[0]);
                }
                else if ((monsterPosition.x + 1 == playerPosition.x && monsterPosition.y - 1 == playerPosition.y) && monster.canAttack == false)
                {
                    AttackPlayer(players[0]);
                }
            }
        }
        
    }
    private void AttackPlayer(Player player)
    {
        GameObject effectAttack = Instantiate(krakenAttackEffect, player.transform.position, Quaternion.identity);
        monster.canAttack = true;
        animator.enabled = true;
        toolTip.color = new Color(1f, 0.15f, 0f);
        toolTip.text = "-" + monster.dame + " độ bền";
        toolTip.gameObject.transform.position = new Vector3(player.transform.position.x - 0.15f, player.transform.position.y + 3f);
        StartCoroutine(DamagePlayer(player));
    }
    private IEnumerator DamagePlayer(Player player)
    {
        yield return new WaitForSeconds(0.5f);
        player.heal -= monster.dame;
        toolTip.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        toolTip.gameObject.SetActive(false);
        animator.enabled = false;
    }

}
