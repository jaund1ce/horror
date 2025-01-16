using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipLight : EquipItemBase
{
    private Light handLight;
    private bool usable = false;

    protected override void Start()
    {
        base.Start();
        handLight = MainGameManager.Instance.Player.gameObject.GetComponentInChildren<Light>();
        ChangeLightIntencity();
        handLight.enabled = false;
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
            SoundManger.Instance.MakeEnviornmentSound("Flashlight_On");
            handLight.enabled = true;
            OnUsing = true;
        }
        else
        {
            SoundManger.Instance.MakeEnviornmentSound("Flashlight_Off");
            handLight.enabled = false;
            OnUsing = false;
        }
        usable = true;
    }
}

