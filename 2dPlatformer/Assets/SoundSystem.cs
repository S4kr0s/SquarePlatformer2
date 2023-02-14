using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private static bool alreadyInstantiated = false;

    void Awake()
    {
        if (!alreadyInstantiated)
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
            audioSource = this.gameObject.GetComponent<AudioSource>();
            ChangeAudioLevel(PlayerPrefs.GetFloat("audioVolumeGeneral", 0.5f));
            alreadyInstantiated = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void ChangeAudioLevel(Slider slider)
    {
        ChangeAudioLevel(slider.value / 100);
    }

    private void ChangeAudioLevel(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat("audioVolumeGeneral", value);
    }

    public static void SetSliderValue(Slider slider)
    {
        slider.value = PlayerPrefs.GetFloat("audioVolumeGeneral", 0.5f) * 100;
    } 
}
