using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class EndGame : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    public List<Player> players=new List<Player>();
    public Text victoryText;
    public GameObject panel;
    void Start()
    {
        panel.SetActive(false);
        players = gridManager.GetAllPlayers();
    }

    void Update()
    {
        if (players[0].heal <=0)
        {
            victoryText.text = "THUA CUỘC";
            victoryText.color = new Color(0f, 0.7843137f, 0.7372549f);
            panel.SetActive(true);
             Time.timeScale = 0f;
            return;
        }
        if(players[1].heal <= 0)
        {
            victoryText.text = "CHIẾN THẮNG";
            victoryText.color = new Color(1f, 0.4666667f, 0.08235294f);
            panel.SetActive(true);
            Time.timeScale = 0f;
            return;
        }
    }
}
