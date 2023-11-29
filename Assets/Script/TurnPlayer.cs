using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPlayer : MonoBehaviour
{
    public GameControl gameControl;
    public Tutorial tutorial;
    public GameObject prefab;
  
    void Update()
    {
        if (gameControl.players[1].playerTurn)
        {
            if(tutorial != null)
            {
                prefab.SetActive(true);
                prefab.transform.DOMove(new Vector3(12f, 12f, 0), 1f);
            }    
            else
            {
                prefab.SetActive(true);
                prefab.transform.DOMove(new Vector3(19, 12f, 0), 1f);
            }    
          
        }
        else if(gameControl.players[0].playerTurn)
        {
            if(tutorial != null)
            {
                prefab.SetActive(true);
                prefab.transform.DOMove(new Vector3(12f, -1.5f, 0), 1f);
            }   
            else
            {
                prefab.SetActive(true);
                prefab.transform.DOMove(new Vector3(19f, -0.75f, 0), 1f);
            }    
         
        }
    }
}
