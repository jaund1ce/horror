using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public Image uiBar; //�̹���
    
    public void GetPercentage(float percentage)
    {
        uiBar.fillAmount = percentage;
    }
}