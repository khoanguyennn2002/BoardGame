using UnityEngine;
public class Node
{
    [HideInInspector] public int tileId;
    public Player player;
    public Monster monster;
}
public class GridMap : MonoBehaviour
{
    [SerializeField] Pathfinding pathfinding;
    [SerializeField] Dice dice;
    [HideInInspector] public int height;
    [HideInInspector] public int length;
    public int inAttackRange;
    public int outAttackRange;
    Node[,] grid;
    public void Init(int length, int height)
    {
        grid = new Node[length, height];
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new Node();
            }
        }
        this.length = length;
        this.height = height;
      
    }
    public void SetPlayer(MapElement mapElement, int x_pos, int y_pos)
    {
        grid[x_pos, y_pos].player = mapElement.GetComponent<Player>();
    }
    public Node[,] GetGrid()
    {
        return grid;
    }
    public void SetMonster(MapElement mapElement, int x_pos, int y_pos)
    {
        grid[x_pos, y_pos].monster = mapElement.GetComponent<Monster>();
    }
    public Player GetPlayer(int x, int y)
    {
        if (CheckPosition(x, y) == true)
        {
            return grid[x, y].player;
            
        }
        return null;
    }
    public Monster GetMonster(int x, int y)
    {
        if (CheckPosition(x, y) == true)
        {
            return grid[x, y].monster;
          
        }
        return null;
    }
    public void Set(int x, int y, int to)
    {
        if (CheckPosition(x, y) == false)
        {
            return;
        }
        grid[x, y].tileId = to;
    }
    public int Get(int x, int y)
    {
        if (CheckPosition(x, y) == false)
        {
            return -1;
        }
        return grid[x, y].tileId;
    }
    public bool CheckPosition(int x, int y)
    {
        if (x < 0 || x >= length)
        {
            return false;
        }
        if (y < 0 || y >= height)
        {
            return false;
        }
        return true;
    }
    internal int CheckAttackRange(PathNode currentNode, PathNode targetNode)
    {
        int distance = pathfinding.CalculateDistance2(currentNode, targetNode);
        int temp = distance;

        while (targetNode != null)
        {
            currentNode = targetNode;
            targetNode = targetNode.parentNode;
            if (targetNode != null)
            {
                distance = pathfinding.CalculateDistance2(currentNode, targetNode);
                temp += distance;
            }
        }
        return temp;
    }
    
    internal bool CheckPositionPlayer(int x, int y)
    {
        if (!CheckPosition(x, y))
        {
            return false;
        }
        return grid[x, y].player != null;
    }
    internal bool CheckPositionMonster(int x, int y)
    {
        if (!CheckPosition(x, y))
        {
            return false;
        }
        return grid[x, y].monster != null;
    }
    internal bool CheckWalkable(int xPos, int yPos)
    {
        return grid[xPos, yPos].tileId != 1;
    }
    internal bool CheckEvent(int xPos, int yPos)
    {
        return grid[xPos, yPos].tileId == 2;
    }

    internal void ClearPlayer(int x_pos, int y_pos)
    {
        if (grid[x_pos, y_pos].player != null)
        {
            grid[x_pos, y_pos].player = null;
        }
    }
    internal bool IsPlayerInAttackRange(Player selectedPlayer, int x, int y)
    {

        int playerX = selectedPlayer.x;
        int playerY = selectedPlayer.y;
        PathNode currentPlayerNode = new PathNode(playerX, playerY);
        PathNode targetNode = new PathNode(x, y);
        int attackRange = CheckAttackRange(currentPlayerNode, targetNode);
        inAttackRange = attackRange;
        if (attackRange >= 0 && attackRange <= selectedPlayer.range && CheckPositionPlayer(x,y))
        {
            return true;
        }
       return false;
    }
    internal bool IsPlayerOutAttackRange(Player selectedPlayer, int x, int y)
    {
        int playerX = selectedPlayer.x;
        int playerY = selectedPlayer.y;
        PathNode currentPlayerNode = new PathNode(playerX, playerY);
        PathNode targetNode = new PathNode(x, y);
        int attackRange = CheckAttackRange(currentPlayerNode, targetNode);
        if (attackRange == 2 && attackRange <= dice.temp && CheckPositionPlayer(x, y))
        {
            return true;
        }
        return false;
    }
}
