using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipLight : EquipItemBase
{
    private Light handLight;
    private bool usable = false;
    private float batteryCapacity;
    public bool onFlash;
    private bool isCoroutineStarted;

    private Coroutine batteryWarningCoroutine; // 배터리 경고용 Coroutine
    private PlayerConditionController playerConditionController;

    protected override void Start()
    {
        base.Start();
        handLight = MainGameManager.Instance.Player.gameObject.GetComponentInChildren<Light>();
        ChangeLightIntencity();
        handLight.enabled = false;
        playerConditionController = MainGameManager.Instance.Player.PlayerConditionController;
    }

    public void Update()
    {
        

        batteryCapacity = playerConditionController.BatteryCapacity;
        if (playerConditionController.OnFlash == true)
        {
            if (batteryCapacity <= 5 && !isCoroutineStarted )
            {
                batteryWarningCoroutine = StartCoroutine(BatteryWarning());
            }
        }

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
        OnUsing = true;
        Invoke("OnUse", 0.5f);
    }


    public override void OnUse()
    {
        if (!playerConditionController.OnFlash)
        {
            if(batteryCapacity <= 0)
            {
                return;
            }
            SoundManger.Instance.MakeEnviornmentSound("Flashlight_On");
            handLight.enabled = true;
            playerConditionController.OnFlash = true;

            Debug.Log(batteryCapacity);
        }
        else
        {
            SoundManger.Instance.MakeEnviornmentSound("Flashlight_Off");
            handLight.enabled = false;
            playerConditionController.OnFlash = false;

            if (batteryWarningCoroutine != null)
            {
                StopCoroutine(BatteryWarning());
                isCoroutineStarted = false;
            }
            Debug.Log(batteryCapacity);
        }
        usable = true;
        OnUsing = false;
    }

    private IEnumerator BatteryWarning()
    {
        isCoroutineStarted = true;
        while (batteryCapacity > 0 && batteryCapacity <= 5)
        {
            handLight.enabled = !handLight.enabled; // 손전등 상태 토글
            yield return new WaitForSeconds(0.5f); // 1초 간격 대기
        }

        // 배터리 상태가 벗어나면 손전등 끔
        handLight.enabled = false;
        OnUsing = false;
        playerConditionController.OnFlash = false;
        isCoroutineStarted = false;
    }
}

