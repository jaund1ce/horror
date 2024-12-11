using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public Image uiBar; //ÀÌ¹ÌÁö
    
    public void GetPercentage(float percentage)
    {
        uiBar.fillAmount = percentage;
    }
}