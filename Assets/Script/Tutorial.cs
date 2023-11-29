using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{
    public GridMap grid;
    private List<ItemCardTutorial> CardsInHand = new List<ItemCardTutorial>();
    public Dice dice;
    public GameObject panel;
    public GameObject canvas;
    public Text text;
    private GameObject hand;
    public int temp = 0;
    public GameControl gameControl;
    public Renderer diceRenderer;
    public Canvas canvasRenderer;
    public bool isTutorial = true;
    public GameObject cardToHand;
    void Start()
    {
        hand = GameObject.Find("Hand Player 1");
        panel.SetActive(true);
        canvasRenderer.sortingOrder = 2;
        diceRenderer = dice.GetComponent<Renderer>();
        canvasRenderer = canvas.GetComponent<Canvas>();
    }
    private void Update()
    {
        CardInHandPlayer();
        if (gameControl.players[0].playerTurn)
        {
            if (temp == 0)
            {
                
                text.text = "LƯỢT CỦA BẠN, TUNG XÚC XẮC ĐỂ DI CHUYỂN";
                panel.SetActive(true);
                temp++;
            }
            else if (temp == 1 && dice.hasRolledDice && gameControl.players[0].moveDistance == dice.temp)
            {
                diceRenderer.sortingLayerName = "Default";
                text.text = "KÉO THẢ BÀI VÀ NHẤN VÀO NÚT OK ĐỂ SỬ DỤNG BÀI";
                panel.SetActive(true);
                gameControl.isMoving = true;
                cardToHand.GetComponent<ItemCardTutorial>().cardIdData = 10;
                Instantiate(cardToHand, transform.position, transform.rotation);
                temp++;
            }
            else if (temp == 2 && CardsInHand.Count > 0)
            {
                panel.SetActive(true);
            }
            else if (temp == 3 && dice.hasRolledDice && gameControl.players[0].moveDistance == dice.temp)
            {
                canvasRenderer.sortingOrder = 0;
                diceRenderer.sortingLayerName = "Default";
                text.text = "DI CHUYỂN ĐẾN GẦN KẺ ĐỊCH";
                panel.SetActive(true);
                gameControl.isMoving = true;
                temp++;
                StartCoroutine(Delay());
            }
            else if(temp == 4 && grid.IsPlayerInAttackRange(gameControl.players[0], gameControl.players[1].x, gameControl.players[1].y) && dice.hasRolledDice && gameControl.players[0].moveDistance == dice.temp)
            {
                canvasRenderer.sortingOrder = 2;
                diceRenderer.sortingLayerName = "Default";
                text.text = "SỬ DỤNG BÀI ĐỂ TẤN CÔNG";
                panel.SetActive(true);
                gameControl.isMoving = true;
                cardToHand.GetComponent<ItemCardTutorial>().cardIdData = 7;
                Instantiate(cardToHand, transform.position, transform.rotation);
                temp++;
            }
            else if (temp == 5 && CardsInHand.Count > 0)
            {
                panel.SetActive(true);
            }
            else if (temp == 6 && grid.IsPlayerInAttackRange(gameControl.players[0], gameControl.players[1].x, gameControl.players[1].y) && dice.hasRolledDice && gameControl.players[0].moveDistance == dice.temp )
            {
                diceRenderer.sortingLayerName = "Default";
                text.text = "SỬ DỤNG BÀI TĂNG SÁT THƯƠNG KẾT HỢP TẤN CÔNG";
                panel.SetActive(true);
                gameControl.isMoving = true;
                cardToHand.GetComponent<ItemCardTutorial>().cardIdData = 7;
                Instantiate(cardToHand, transform.position, transform.rotation);
                cardToHand.GetComponent<ItemCardTutorial>().cardIdData = 8;
                Instantiate(cardToHand, transform.position, transform.rotation);
                temp++;
            }
            else if(temp== 7 && CardsInHand.Count > 0)
            {
                panel.SetActive(true);
            }    
            else if(temp== 8 && dice.hasRolledDice && gameControl.players[0].moveDistance == dice.temp)
            {
                canvasRenderer.sortingOrder = 0;
                diceRenderer.sortingLayerName = "Default";
                text.text = "DI CHUYỂN CÁCH KẺ ĐỊCH 1 Ô";
                panel.SetActive(true);
                gameControl.isMoving = true;
                temp++;
                StartCoroutine(Delay());
            }    
            else if (temp == 9 && grid.IsPlayerOutAttackRange(gameControl.players[0], gameControl.players[1].x, gameControl.players[1].y) && dice.hasRolledDice && gameControl.players[0].moveDistance == dice.temp)
            {
                canvasRenderer.sortingOrder = 2;
                diceRenderer.sortingLayerName = "Default";
                text.text = "SỬ DỤNG BÀI TĂNG TẦM ĐÁNH KẾT HỢP TẤN CÔNG";
                panel.SetActive(true);
                gameControl.isMoving = true;
                cardToHand.GetComponent<ItemCardTutorial>().cardIdData = 7;
                Instantiate(cardToHand, transform.position, transform.rotation);
                cardToHand.GetComponent<ItemCardTutorial>().cardIdData = 9;
                Instantiate(cardToHand, transform.position, transform.rotation);
                temp++;
            }
            else if (temp == 10 && CardsInHand.Count > 0)
            {
                panel.SetActive(true);
            }
          else if ( temp == 11 )
            {
                cardToHand.GetComponent<ItemCardTutorial>().cardIdData = 7;
                Instantiate(cardToHand, transform.position, transform.rotation);
                cardToHand.GetComponent<ItemCardTutorial>().cardIdData = 8;
                Instantiate(cardToHand, transform.position, transform.rotation);
                cardToHand.GetComponent<ItemCardTutorial>().cardIdData = 9;
                Instantiate(cardToHand, transform.position, transform.rotation);
                cardToHand.GetComponent<ItemCardTutorial>().cardIdData = 10;
                Instantiate(cardToHand, transform.position, transform.rotation);
                temp++;
            }    
        }
    }
    public void CardInHandPlayer()
    {
        foreach (Transform child in hand.transform)
        {
            ItemCardTutorial card = child.GetComponent<ItemCardTutorial>();
            if (card != null && !CardsInHand.Contains(card))
            {
                CardsInHand.Add(card);
            }
        }
        CardsInHand.RemoveAll(card => card == null || !card.transform.IsChildOf(hand.transform));
    } 
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.5f);
        panel.SetActive(false);
        gameControl.isMoving=false;
    }    
}
