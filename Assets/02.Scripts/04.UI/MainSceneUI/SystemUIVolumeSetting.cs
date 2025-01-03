using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AudioSourceType
{
    BGM = 0,
    HEARTHBEAT,
    BREATHE,
    STEP,
    ENVIROMENT
}

public class SystemUIVolumeSetting : MonoBehaviour
{
    [SerializeField]private AudioSourceType audioSourceType;
    [SerializeField]private Slider VolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        VolumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void OnEnable()
    {
        VolumeSlider.value = SoundManger.Instance.GetVolume(audioSourceType);
    }

    private void ChangeVolume(float amount)
    {
        SoundManger.Instance.AdjustSoundVolume(audioSourceType, amount);
    }
}
