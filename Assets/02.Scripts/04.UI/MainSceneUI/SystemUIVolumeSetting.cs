using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AudioSourceType
{
    MASTER = 0,
    BGM,
    HEARTHBEAT,
    BREATHE,
    STEP,
    ENVIROMENT
}

public class SystemUIVolumeSetting : MonoBehaviour
{
    [SerializeField]private AudioSourceType audioSourceType;
    [SerializeField]private Slider VolumeSlider;

    private void Start()
    {
        if (VolumeSlider != null) VolumeSlider.value = SoundManger.Instance.GetVolume(audioSourceType);
    }

    public void ChangeVolume(float amount)
    {
        SoundManger.Instance.AdjustSoundVolume(audioSourceType, amount);
    }
}
