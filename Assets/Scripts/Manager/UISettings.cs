using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{

    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderVFX;

    [SerializeField] private AudioMixer mixer;
    private void Awake()
    {
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
        mixer.SetFloat("MasterVolume", currentValue);
    }

    private void ChangeMusicVolume(float currentValue)
    {
        mixer.SetFloat("MusicVolume", currentValue);
    }

    private void ChangeVFXVolume(float currentValue)
    {
        mixer.SetFloat("VFXVolume", currentValue);
    }
}
