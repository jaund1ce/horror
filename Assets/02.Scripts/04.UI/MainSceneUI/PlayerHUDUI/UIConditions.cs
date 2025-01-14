using UHFPS.Runtime;
using Unity.VisualScripting;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    private PlayerConditionController playerConditionController;
    private Player player;
    public Condition DamagePrompt;
    public CreatureAI creatureAI;
    public GameObject RecPoint;

    private float duration = 2f;
    private float lastCheckTime;
    private bool onoff = true;

    private void Start()
    {
        player = MainGameManager.Instance.Player;
        playerConditionController = player.PlayerConditionController;
        player.HPChange += ChangeDamagedPrompt;

        creatureAI = FindAnyObjectByType<CreatureAI>();
    }

    private void FixedUpdate()
    {
        if (Time.time - lastCheckTime < duration) return;

        onoff = !onoff;
        lastCheckTime = Time.time;
        RecPoint.SetActive(onoff);
    }

    private void ChangeDamagedPrompt()
    {
        DamagePrompt.GetAPercentage(playerConditionController.GetHPPercentage());
    }
}