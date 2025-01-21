using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public Image uiBar; //¿ÃπÃ¡ˆ
    
    public void GetPercentage(float percentage)
    {
        uiBar.fillAmount = percentage;
    }

    public void GetAPercentage(float percentage)
    {
        Color currentColor = uiBar.color;
        currentColor.a = (1f - percentage)/2;

        uiBar.color = currentColor;
    }
}