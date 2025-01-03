using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBlink : MonoBehaviour
{
    private TextMeshProUGUI text; 
    [field : SerializeField] private Color startColor; 
    [field : SerializeField] private Color endColor ; 
    private float duration = 2.0f; 

    private float lerpTime = 0;
    private bool isReversing = false; 

    void Start()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>(); 
        }
    }

    void Update()
    {
        lerpTime += (isReversing ? -1 : 1) * Time.deltaTime / duration;

        if (lerpTime > 1)
        {
            lerpTime = 1;
            isReversing = true;
        }
        else if (lerpTime < 0)
        {
            lerpTime = 0;
            isReversing = false; 
        }


        text.color = Color.Lerp(startColor, endColor, lerpTime);
    }
}
