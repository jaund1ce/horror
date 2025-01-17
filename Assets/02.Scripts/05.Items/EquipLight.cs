using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipLight : EquipItemBase
{
    private Light handLight;
    private bool usable = false;
    private float batteryCapacity;
    public bool onFlash = false;   

    protected override void Start()
    {
        base.Start();
        handLight = MainGameManager.Instance.Player.gameObject.GetComponentInChildren<Light>();
        ChangeLightIntencity();
        handLight.enabled = false;
        batteryCapacity = MainGameManager.Instance.Player.PlayerConditionController.BatteryCapacity;
        onFlash = MainGameManager.Instance.Player.PlayerConditionController.OnFlash;
    }

    private void ChangeLightIntencity()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MainScene")
        {
            handLight.intensity = 10;
        }
        else if (sceneName == "MainScene2")
        {
            handLight.intensity = 200;
        }
    }

    public override void OnUseInput()
    {
        if (inventoryData == null) return;
        if (!usable) return;

        usable = false;
        Invoke("OnUse", 0.5f);
    }


    public override void OnUse()
    {
        if (!OnUsing)
        {
            if(batteryCapacity <= 0)
            {
                return;
            }
            SoundManger.Instance.MakeEnviornmentSound("Flashlight_On");
            handLight.enabled = true;
            OnUsing = true;
            onFlash = true;
            Debug.Log(batteryCapacity);
        }
        else
        {
            SoundManger.Instance.MakeEnviornmentSound("Flashlight_Off");
            handLight.enabled = false;
            OnUsing = false;
            onFlash = false;
            Debug.Log(batteryCapacity);
        }
        usable = true;
    }
}

