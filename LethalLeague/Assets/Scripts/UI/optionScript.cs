using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class optionScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropDown;
    Resolution[] resolutions;

    public void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropDown.ClearOptions();  
        List<string> options = new List<string>();

        int indexCurrentResolution = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                    {
                        indexCurrentResolution = i;
                    }
        }     

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = indexCurrentResolution;
        resolutionDropDown.RefreshShownValue();
    }
    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void setSoundEffect(float volume)
    {
        audioMixer.SetFloat("SoundEffect", volume);
    }

    public void setFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void setResolution(int indexResolution)
    {
        Resolution resolution = resolutions[indexResolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
