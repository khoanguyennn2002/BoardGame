using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [System.Serializable]
    public class MyList
    {
        public List<CardData> cardList;
    }

    MyList myList = new MyList();

    void Awake()
    {
        myList.cardList = new List<CardData>();
        myList.cardList.Add(new CardData(1, "Xa thu", "Day la the xa thu", 1, "Characters/archer-4"));
        myList.cardList.Add(new CardData(2, "Bac hoc", "Day la the bac hoc", 1, "Characters/captain-3"));
        myList.cardList.Add(new CardData(3, "Thuyen pho", "Day la the thuyen pho", 1, "Characters/captain-4"));
        myList.cardList.Add(new CardData(4, "Lai tau", "Day la the lai tau", 1, "Characters/driver-2"));
        myList.cardList.Add(new CardData(5, "Tho may", "Day la the tho may", 1, "Characters/mechanic-2"));
        myList.cardList.Add(new CardData(6, "Nguoi choi nhac", "Day la the nguoi choi nhac", 1, "Characters/musician-4"));

        myList.cardList.Add(new CardData(7, "Đạn", "Gây 1 sát thương", 2, "Items/bullet"));
        myList.cardList.Add(new CardData(8, "Rượu", "Sát thương đạn +1", 2, "Items/wine"));
        myList.cardList.Add(new CardData(9, "Thuốc", "Tầm đánh +1", 2, "Items/medicine"));
        myList.cardList.Add(new CardData(10, "Gỗ", "Độ bền +1", 2, "Items/wood"));

        string json = JsonUtility.ToJson(myList);
        string filePath = Path.Combine(Application.streamingAssetsPath, "cardData.json");
        File.WriteAllText(filePath, json);
    }
}
