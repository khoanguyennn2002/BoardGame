using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class SetScreen : MonoBehaviour
{
    public Toggle toggle;
    Resolution[] resolutions;
    public List<string> resolutionText = new List<string>();
    public Text text;
    int currentResolutionIndex = 7;
    static bool isFullScreen;
    private void Start()
    {
        resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionText.Add(resolutions[i].width + " x " + resolutions[i].height);
        }
        toggle.isOn = isFullScreen ? true : false;

        text.text = Screen.width + " x " + Screen.height;
    }

    public void ChangeResolutionRight()
    {
        currentResolutionIndex--;
        if (currentResolutionIndex < 0)
        {
            currentResolutionIndex = resolutions.Length - 1;
        }
        UpdateResolutionText();
     
    }
    public void ChangeResolutionLeft()
    {
        currentResolutionIndex++;
        if (currentResolutionIndex >= resolutions.Length)
        {
            currentResolutionIndex = 0;
        }
        UpdateResolutionText();
     
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        isFullScreen = isFullscreen;
    }

    public void SetResolution()
    {
        Resolution selectedResolution = resolutions[currentResolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }

    private void UpdateResolutionText()
    {
        text.text = resolutionText[currentResolutionIndex];
    }
}
