using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageBlink : MonoBehaviour
{
    private Image image;
    [field: SerializeField] private Color startColor;
    [field: SerializeField] private Color endColor;
    private float duration = 2.0f;

    private float lerpTime = 0;
    private bool isReversing = false;

    void Start()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
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


        image.color = Color.Lerp(startColor, endColor, lerpTime);
    }
}
