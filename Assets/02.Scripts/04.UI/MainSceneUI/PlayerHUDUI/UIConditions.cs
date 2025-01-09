using UHFPS.Runtime;
using Unity.VisualScripting;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    private PlayerConditionController playerConditionController;
    public Condition DamagePrompt;
    public CreatureAI creatureAI;
    public GameObject RecPoint;

    private float duration = 2f;
    private float lastCheckTime;
    private bool onoff = true;

    private void Start()
    {
        playerConditionController = MainGameManager.Instance.Player.PlayerConditionController;
        creatureAI = FindAnyObjectByType<CreatureAI>();
        MainGameManager.Instance.Player.HPChange += ChangeDamagePrompt;
    }

    private void FixedUpdate()
    {
        if (Time.time - lastCheckTime < duration) return;

        onoff = !onoff;
        lastCheckTime = Time.time;
        RecPoint.SetActive(onoff);
    }

    public void ChangeDamagePrompt()
    {
        DamagePrompt.GetAPercentage(playerConditionController.GetHPPercentage());
    }
}