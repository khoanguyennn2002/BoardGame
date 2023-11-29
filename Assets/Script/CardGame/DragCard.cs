
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class DragCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject tutorial;
    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null;
    private GameObject dropzone;
    private GameObject hand;
    GameObject placeholder = null;
    public List<HoverCard> hoverCards = new List<HoverCard>();
    private Image dropzoneImage;
    private Image handImage;
    private void Awake()
    {
        tutorial = GameObject.Find("Main Camera");
        dropzone = GameObject.Find("Drop Zone");
        hand = GameObject.Find("Hand Player 1");
        dropzoneImage = dropzone.GetComponent<Image>();
        handImage = hand.GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(tutorial.GetComponent<Tutorial>() != null)
        {
            tutorial.GetComponent<Tutorial>().panel.SetActive(false);
        }    
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        this.transform.localScale = new Vector3(1f, 1f, 0);
        hoverCards = FindObjectsOfType<HoverCard>().ToList();
        foreach (HoverCard hoverCard in hoverCards)
        {
            hoverCard.enabled = false;
        }
        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        parentToReturnTo = this.transform.parent;
        placeholderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);
      
        dropzoneImage.color = new Color(dropzoneImage.color.r, dropzoneImage.color.g, dropzoneImage.color.b, 0.2f);
        handImage.color = new Color(dropzoneImage.color.r, dropzoneImage.color.g, dropzoneImage.color.b, 0.2f);
    }
    public void OnDrag(PointerEventData eventData)
    {
       // this.transform.position = eventData.position;

        Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y); 
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        this.transform.position = worldPos;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeholder);
        transform.localScale = new Vector3(1f, 1f, 0);
        foreach (HoverCard hoverCard in hoverCards)
        {
            hoverCard.game.GetComponent<GameControl>().isHover = false;
            hoverCard.enabled = true;
            hoverCard.layoutGroup.enabled = true;
            hoverCard.isChecked = false;
        }
        dropzoneImage.color = new Color(dropzoneImage.color.r, dropzoneImage.color.g, dropzoneImage.color.b, 0f);
        handImage.color = new Color(dropzoneImage.color.r, dropzoneImage.color.g, dropzoneImage.color.b, 0f);
    }
}
