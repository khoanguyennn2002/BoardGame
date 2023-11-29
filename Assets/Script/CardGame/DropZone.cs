using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // uint player = 0;  
        DragCard d = eventData.pointerDrag.GetComponent<DragCard>();
        //string a = Regex.Replace(this.transform.ToString(), "[^0-9]+", "");
        //if(a!="")
        //{
        //    player = Convert.ToUInt32(a);
        //}    
        if (d != null)
        {
            d.parentToReturnTo = this.transform;
        }
    }
}
