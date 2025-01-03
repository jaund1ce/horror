using System.Collections;
using System.Collections.Generic;
using TMPro;
using UHFPS.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class ImageBlink : MonoBehaviour
{
    private Image image;
    private AudioSource audioSource;
    [field: SerializeField] private Color startColor;
    [field: SerializeField] private Color endColor;
    [field: SerializeField] private AudioClip blinkLightsound;
    private float blinkInterval = 0.3f; // √÷º“ ±Ù∫˝¿” ∞£∞›
    private float nextBlinkTime = 0;
    private float soundCheckTime;
    private float soundTime = 0.7f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        ScheduleNextBlink();
    }

    void Update()
    {
        if (Time.time >= nextBlinkTime)
        {
            BlinkRandomly();
            ScheduleNextBlink();
        }
    }

    private void BlinkRandomly()
    {
        if (Random.value > 0.5f)
        {
            image.color = startColor;
            int i = Random.Range(0,2);
            soundCheckTime += Time.deltaTime;
            if (i == 1 || soundCheckTime > soundTime) 
            {
                audioSource.PlayOneShot(blinkLightsound);
                soundCheckTime = 0;
            } 
        }
        else
        {
            image.color = endColor;
        }
    }

    private void ScheduleNextBlink()
    {
        nextBlinkTime = Time.time + Random.Range(blinkInterval, blinkInterval * 2);
    }
}
