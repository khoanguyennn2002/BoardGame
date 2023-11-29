using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public GameObject setting;
    public GameObject[] tabs;

    public void OpenSetting()
    {
        if (setting != null)
        {
            setting.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void CloseSetting()
    {
        if (setting != null)
        {
            setting.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    public void TurnOnTabs(int tab)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(false);
        }
        tabs[tab - 1].SetActive(true);
    }
}
