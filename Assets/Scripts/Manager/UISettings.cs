using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{

    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderVFX;

    [SerializeField] private AudioSettingsSO settingsSO;

    [SerializeField] private AudioMixer mixer;
    private void Awake()
    {
        settingsSO.LoadSettings();

        sliderMaster.value = settingsSO.masterVolume;
        sliderMusic.value = settingsSO.musicVolume;
        sliderVFX.value = settingsSO.vfxVolume;

        mixer.SetFloat("MasterVolume", sliderMaster.value);
        mixer.SetFloat("MusicVolume", sliderMusic.value);
        mixer.SetFloat("VFXVolume", sliderVFX.value);

        sliderMaster.onValueChanged.AddListener(ChangeMasterVolume);
        sliderMusic.onValueChanged.AddListener(ChangeMusicVolume);
        sliderVFX.onValueChanged.AddListener(ChangeVFXVolume);
    }

    private void OnDestroy()
    {
        sliderMaster.onValueChanged.RemoveAllListeners();
        sliderMusic.onValueChanged.RemoveAllListeners();
        sliderVFX.onValueChanged.RemoveAllListeners();
    }
    private void ChangeMasterVolume(float currentValue)
    {
        settingsSO.SaveMaster(currentValue);
        mixer.SetFloat("MasterVolume", currentValue);
    }

    private void ChangeMusicVolume(float currentValue)
    {
        settingsSO.SaveMusic(currentValue);
        mixer.SetFloat("MusicVolume", currentValue);
    }

    private void ChangeVFXVolume(float currentValue)
    {
        settingsSO.SaveVFX(currentValue);
        mixer.SetFloat("VFXVolume", currentValue);
    }
}
