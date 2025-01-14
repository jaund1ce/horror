using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemUIMouseSencitivitySetting : MonoBehaviour
{
    [SerializeField] private Slider mousesencitivitySlider;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        mousesencitivitySlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void OnEnable()
    {
        player = MainGameManager.Instance.Player;

        mousesencitivitySlider.value = player.Input.GetRotateSencitivity();
    }

    private void ChangeVolume(float amount)
    {
        player.Input.ChangeRotateSencitivity(amount);
    }
}
