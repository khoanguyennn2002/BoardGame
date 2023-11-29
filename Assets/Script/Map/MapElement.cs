
using DG.Tweening;
using UnityEngine;
public class MapElement : MonoBehaviour
{
    GridMap grid;
    GridManager gridManager;

    int x_pos = 0;
    int y_pos = 0;
    void Awake()
    {
        SetGrid();
        PlaceObjectOnGrid();
    }
    //void Update()
    //{
    //    PlaceMonsterOnGrid();
    //}
    public void SetGrid()
    {
         grid = transform.parent.GetComponent<GridMap>();
         gridManager = transform.parent.GetComponent<GridManager>(); ;
    }
    public void MovePlayer(int targetPosX, int targetPosY)
    {
        RemoveObject();
        MoveTo(targetPosX, targetPosY);
        MoveObject();

    }
    private void MoveTo(int targetPosX, int targetPosY)
    {
        grid.SetPlayer(this, targetPosX, targetPosY);
        x_pos = targetPosX;
        y_pos = targetPosY;
    }
    public void MoveObject()
    {
        Vector3 worldPosition;
        if (y_pos % 2 == 0)
        {
            worldPosition = new Vector3(x_pos * 1f, y_pos * 0.75f, -0.5f);
            transform.DOMove(worldPosition, 0.2f);
        }
        else
        {
            worldPosition = new Vector3(x_pos * 1f + 0.5f, y_pos * 0.75f, -0.5f);
            transform.DOMove(worldPosition, 0.2f);
        }
    }
    public void PlaceObjectOnGrid()
    {
        Transform t = transform;
        Vector3 pos = t.position;
        pos.y = Mathf.RoundToInt(pos.y / 0.75f);
        if (pos.y % 2 != 0)
        {
            pos.x -= 0.5f;
        }
        x_pos = (int)pos.x;
        y_pos = (int)pos.y;
        grid.SetPlayer(this, x_pos, y_pos);
        grid.SetMonster(this, x_pos, y_pos);
    }
    public void PlaceMonsterOnGrid()
    {
        Transform t = transform;
        Vector3 pos = t.position;
        pos.y = Mathf.RoundToInt(pos.y / 0.75f);
        if (pos.y % 2 != 0)
        {
            pos.x -= 0.5f;
        }
        x_pos = (int)pos.x;
        y_pos = (int)pos.y;
        grid.SetMonster(this, x_pos, y_pos);
    }
    public void RemoveObject()
    {
        grid.ClearPlayer(x_pos, y_pos);
    }    
}
