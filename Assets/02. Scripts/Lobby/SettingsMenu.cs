using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    public Dropdown qualityDropdown;

    public Slider masterSlider, BGMSlider, VoiceSlider;
    private void Start()
    {
        qualityDropdown.value = QualitySettings.GetQualityLevel();
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);


        float masterResult;
        float BGMResult;
        float VoiceResult;
        audioMixer.GetFloat("MasterVolume", out masterResult);
        audioMixer.GetFloat("BGMVolume", out BGMResult);
        audioMixer.GetFloat("VoiceVolume", out VoiceResult);
        masterSlider.value = masterResult;
        BGMSlider.value = BGMResult;
        VoiceSlider.value = VoiceResult;
    }
    public void MicSetting()
    {

    }
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", volume);
    }

    public void SetVoiceVolume(float volume)
    {
        audioMixer.SetFloat("VoiceVolume", volume);
    }

    public void ClickSettingButton()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }
}
