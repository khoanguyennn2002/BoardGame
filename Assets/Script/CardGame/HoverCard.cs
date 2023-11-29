using UnityEngine.EventSystems;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class HoverCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float yOffset = 125f;
    public GameObject dropZone;
    public bool isChecked = false;
    internal HorizontalLayoutGroup layoutGroup;
    public GameObject hand;
    public GameObject game;
    private GameControl gameControl;
    private int initialSiblingIndex;
    void Start()
    {
        game= GameObject.Find("Main Camera");
        gameControl= game.GetComponent<GameControl>();
        dropZone = GameObject.Find("Drop Zone");
        hand = GameObject.Find("Hand Player 1");
        layoutGroup = hand.GetComponent<HorizontalLayoutGroup>();
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.transform.IsChildOf(dropZone.transform))
        {
            return;
        }
        gameControl.isHover = true;
        initialSiblingIndex = transform.GetSiblingIndex();
        layoutGroup.enabled = false;
        transform.SetAsLastSibling();
        isChecked = true;
        transform.localScale = new Vector3(1.5f, 1.5f, 0);
        transform.localPosition += new Vector3(0, yOffset, 0);
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.transform.IsChildOf(dropZone.transform))
        {
            return;
        }
        if (!isChecked)
        {
            return;
        }
        gameControl.isHover = false;
        transform.SetSiblingIndex(initialSiblingIndex);
        transform.localScale = new Vector3(1f, 1f, 0);
        transform.localPosition -= new Vector3(0, yOffset, 0);
        isChecked = false;
        layoutGroup.enabled = true;
    }
}
