using UHFPS.Runtime;
using Unity.VisualScripting;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    private PlayerConditionController playerConditionController;
    public Condition health;
    public Condition stamina;
    public Condition aggro;
    public Condition DamagePrompt;
    public CreatureAI creatureAI;

    private void Start()
    {
        playerConditionController = MainGameManager.Instance.Player.PlayerConditionController;
        creatureAI = FindAnyObjectByType<CreatureAI>();
    }

    private void Update()
    {
        health.GetPercentage(playerConditionController.GetHPPercentage());
        stamina.GetPercentage(playerConditionController.GetStaminaPercentage());
        aggro.GetPercentage(creatureAI.AggroGage/100f);
        DamagePrompt.GetAPercentage(playerConditionController.GetHPPercentage());
    }
}