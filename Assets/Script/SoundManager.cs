
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private AudioSource[] backgroundMusic;
    //public AudioSource effect;
    public AudioSource diceSound;
    public AudioSource soundButton;
    public AudioSource soundButton2;
    public GameObject BackgroundMusic;
    public GameObject[] soundEffect;
    private string[] bgText;
    private string[] soundButtonText;
    public Text textBGMusic;
    public Text textSoundButton;
    int bgcount;
    int soundcount;
    static int staticbgcount;
    static int staticsoundcount;
    private void Start()
    {
        backgroundMusic = BackgroundMusic.GetComponentsInChildren<AudioSource>();
      
        bgText = new string[2];
        bgText[0] = "BẬT";
        bgText[1] = "TẮT";
        soundButtonText = new string[2];
        soundButtonText[0] = "BẬT";
        soundButtonText[1] = "TẮT";
        bgcount = staticbgcount;
        soundcount = staticsoundcount;
        textBGMusic.text = bgText[staticbgcount];
        textSoundButton.text = soundButtonText[staticsoundcount];
    }
    private void Update()
    {

        OnOffBackGroundMusic();
        OnOffSoundButton();
    }
    public void OnOffTextBGMusicRight()
    {
        bgcount++;
        if (bgcount > 1)
        {
            bgcount = 0;
        }
        textBGMusic.text = bgText[bgcount];
        staticbgcount = bgcount;
    }
    public void OnOffTextBGMusicLeft()
    {
        bgcount--;
        if (bgcount < 0)
        {
            bgcount = 1;
        }
        textBGMusic.text = bgText[bgcount];
        staticbgcount = bgcount;
    }
    public void OnOfTextSoundRight()
    {
        soundcount++;
        if (soundcount > 1)
        {
            soundcount = 0;
        }
        textSoundButton.text = soundButtonText[soundcount];
        staticsoundcount = soundcount;

    }
    public void OnOfTextSoundLeft()
    {
        soundcount--;
        if (soundcount < 0)
        {
            soundcount = 1;
        }
        textSoundButton.text = soundButtonText[soundcount];
        staticsoundcount = soundcount;
    }

    private void OnOffBackGroundMusic()
    {
        if (staticbgcount == 1)
        {
            for (int i = 0; i < backgroundMusic.Length; i++)
            {
                backgroundMusic[i].mute = true;
            }
        }
        else
        {
            for (int i = 0; i < backgroundMusic.Length; i++)
            {
                backgroundMusic[i].mute = false;
            }
        }
    }
    private void OnOffSoundButton()
    {
        if (staticsoundcount == 1)
        {
            soundButton.mute = true;
            if (soundButton2 != null)
            {
                soundButton2.mute = true;
            }
            if(soundEffect.Length!=0)
            {
                for (int i=0;i< soundEffect.Length;i++)
                {
                    soundEffect[i].GetComponentInChildren<AudioSource>().mute = true;
                }    
            }    

        }
        else
        {
            soundButton.mute = false;
            if (soundButton2 != null)
            {
                soundButton2.mute = false;
            }
            if (soundEffect.Length != 0)
            {
                for (int i = 0; i < soundEffect.Length; i++)
                {
                    soundEffect[i].GetComponentInChildren<AudioSource>().mute = false;
                    
                }
            }

        }

    }
}
