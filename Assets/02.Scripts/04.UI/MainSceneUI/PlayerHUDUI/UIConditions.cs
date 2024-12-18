using UHFPS.Runtime;
using Unity.VisualScripting;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    private PlayerConditionController playerConditionController;
    public Condition health;
    public Condition stamina;
    public Condition aggro;
    public CreatureAI creatureAI;

    private void Start()
    {
        playerConditionController = MainGameManager.Instance.Player.playerConditionController;
        creatureAI = FindAnyObjectByType<CreatureAI>();
    }

    private void Update()
    {
        health.GetPercentage(playerConditionController.GetHPPercentage());
        stamina.GetPercentage(playerConditionController.GetStaminaPercentage());
        aggro.GetPercentage(creatureAI.aggroGage/100f);
    }
}