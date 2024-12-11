using UHFPS.Runtime;
using Unity.VisualScripting;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    private Player player;
    public Condition health;
    public Condition stamina;
    public Condition aggro;

    private void Start()
    {
        player = MainGameManager.Instance.Player;
    }

    private void Update()
    {
        health.GetPercentage(player.health.GetHPPercentage());
        stamina.GetPercentage(player.health.GetStaminaPercentage());
    }
}