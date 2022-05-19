using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseMenuVolume : MonoBehaviour
{
    public pauseMenu pauseMenu;

    private bool isShowing = false;

    public GameObject pointer;

    public GameObject labelMusic;
    public GameObject labelSFX;

    public GameObject sliderMusic;
    public GameObject sliderSFX;

    private GameObject selectedLabel;

    public float musicVolume;
    public float musicPercent;

    public float sfxVolume;
    public float sfxPercent;

    public float halfAmplitude = 1f;
    public float maxAmplitude = 4f;

    public GameObject selectSound;

    private FMOD.Studio.VCA musicVCA;
    private FMOD.Studio.VCA sfxVCA;

    // Start is called before the first frame update
    void Start()
    {
        selectedLabel = labelMusic;

        musicPercent = PlayerPrefs.GetFloat("musicPercent", .5f);
        musicVolume = PlayerPrefs.GetFloat("musicVoume", getMusic());
        sliderMusic.GetComponent<Image>().fillAmount = musicPercent;

        sfxPercent = PlayerPrefs.GetFloat("sfxPercent", .5f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", getSFX());
        sliderSFX.GetComponent<Image>().fillAmount = sfxPercent;

        musicVCA = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        sfxVCA = FMODUnity.RuntimeManager.GetVCA("vca:/SFX");

        // set initial amplitude in case default value isn't .5 for whatever reason
        musicPercent = sliderMusic.GetComponent<Image>().fillAmount;
        musicVolume = getMusic();
        musicVCA.setVolume(musicVolume);
        sfxPercent = sliderSFX.GetComponent<Image>().fillAmount;
        sfxVolume = getSFX();
        sfxVCA.setVolume(sfxVolume);
    }

    // Update is called once per frame
    void Update()
    {
        isShowing = pauseMenu.pauseMenuItemsShowing[1];

        if (isShowing)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                toggleSelectedLabel();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                editSelectedLabel(0.05f);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                editSelectedLabel(-0.05f);
            }

            realignPointer();

            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.GetComponent<pauseMenu>().goBack();
            }
        }
    }

    void toggleSelectedLabel()
    {
        if (selectedLabel == labelMusic)
        {
            selectedLabel = labelSFX;
        }
        else if (selectedLabel == labelSFX)
        {
            selectedLabel = labelMusic;
        }

        selectSound.GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }

    void editSelectedLabel(float change)
    {
        if (selectedLabel == labelMusic)
        {
            sliderMusic.GetComponent<Image>().fillAmount += change;
            musicPercent = sliderMusic.GetComponent<Image>().fillAmount;
            musicVolume = getMusic();

            PlayerPrefs.SetFloat("musicPercent", musicPercent);
            PlayerPrefs.SetFloat("musicVolume", musicVolume);

            musicVCA.setVolume(musicVolume);
        }
        else if (selectedLabel == labelSFX)
        {
            sliderSFX.GetComponent<Image>().fillAmount += change;
            sfxPercent = sliderSFX.GetComponent<Image>().fillAmount;
            sfxVolume = getSFX();

            PlayerPrefs.SetFloat("sfxPercent", sfxPercent);
            PlayerPrefs.SetFloat("sfxVolume", sfxVolume);

            sfxVCA.setVolume(sfxVolume);
        }

        selectSound.GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }

    void realignPointer()
    {
        Vector3 pointerPos = selectedLabel.transform.position;

        pointerPos[0] -= 12;

        pointer.GetComponent<RectTransform>().position = pointerPos;

    }

    void movePointerOffscreen()
    {
        Vector3 pointerPos = new Vector3(-Screen.width * 2, -Screen.height * 2, 0);

        pointer.GetComponent<RectTransform>().position = pointerPos;
    }

    // Range 0 to maxAmplitude, with the halfway point being halfAmplitude
    float getMusic()
    {
        if (musicPercent <= .5) return (musicPercent * 2) * halfAmplitude;
        else return ((musicPercent - 0.5f) * 2) * (maxAmplitude - 1) + halfAmplitude;
    }

    // Range 0 to maxAmplitude, with the halfway point being halfAmplitude
    float getSFX()
    {
        if (sfxPercent <= .5) return (sfxPercent * 2) * halfAmplitude;
        else return ((sfxPercent - 0.5f) * 2) * (maxAmplitude - 1) + halfAmplitude;
    }
}
