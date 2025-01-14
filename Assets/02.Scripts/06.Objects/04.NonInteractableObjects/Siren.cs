using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Siren : MonoBehaviour
{
    [SerializeField]private Color accessColor = new Color(0, 190, 0);
    [SerializeField]private Color denieColor = new Color(190, 0, 0);
    [SerializeField]private float blinkTime = 15f;
    [SerializeField]private AudioClip audioClip;
    private AudioSource audioSource;
    private string emissionKeyword = "_EMISSION";
    private string emissionColorKeyword = "_EmissionColor";
    private MeshRenderer sirenRenderer;
    private Coroutine blinkCoroutine;

    private void Start()
    {
        sirenRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        sirenRenderer.material.EnableKeyword(emissionKeyword);
        sirenRenderer.material.SetColor(emissionColorKeyword, accessColor);
    }

    public void Access() 
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        sirenRenderer.material.EnableKeyword(emissionKeyword);
        sirenRenderer.material.SetColor(emissionColorKeyword, accessColor);
    }

    public void Denie() 
    {
        if (blinkCoroutine == null)
        {
            foreach (EnemyAI enemy in MainGameManager.Instance.Enemy) 
            {
                enemy.GetAggroGage(20f);
            }
            audioSource.PlayOneShot(audioClip);
            blinkCoroutine = StartCoroutine(BlinkSiren(blinkTime));
        }
        

    }

    private IEnumerator BlinkSiren(float blinkTime)
    {
        float checkTime = 0f;
        bool isOn = false;

        while (checkTime < blinkTime)
        {
            if (isOn)
            {
                sirenRenderer.material.EnableKeyword(emissionKeyword);
                sirenRenderer.material.SetColor(emissionColorKeyword, denieColor);
            }
            else
            {
                sirenRenderer.material.DisableKeyword(emissionKeyword);
            }

            isOn = !isOn; 

            yield return new WaitForSeconds(0.5f);

            checkTime += 0.5f;
        }

        sirenRenderer.material.EnableKeyword(emissionKeyword);
        sirenRenderer.material.SetColor(emissionColorKeyword, accessColor);
        blinkCoroutine = null;

    }


}
