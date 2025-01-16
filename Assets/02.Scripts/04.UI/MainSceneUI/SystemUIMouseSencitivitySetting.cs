using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemUIMouseSencitivitySetting : MonoBehaviour
{
    [SerializeField] private Slider mousesencitivitySlider;
    private Player player;

    void Start()
    {
        if (mousesencitivitySlider == null) return;
        mousesencitivitySlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void OnEnable()
    {
        if (mousesencitivitySlider == null) return;
        if (player == null) player = MainGameManager.Instance.Player;
        else return;
        mousesencitivitySlider.value = player.Input.GetRotateSencitivity();
    }

    public void ChangeVolume(float amount)
    {
        if(player == null)
        {
            MainGameManager.Instance.temMouseSensitivity = amount;
            return;
        }

        player.Input.ChangeRotateSencitivity(amount);
    }
}
