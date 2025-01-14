using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGlass : MonoBehaviour
{
    [SerializeField]private LayerMask player;
    AudioSource audioSource;
    [SerializeField]AudioClip clip;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {

      //if (other.TryGetComponent(out PlayerConditionController))
        if (1 <<other.gameObject.layer == player)
        {
            audioSource.PlayOneShot(clip);
            foreach (EnemyAI enemy in MainGameManager.Instance.Enemy)
            {

                enemy.GetAggroGage(5f);
            }
        }
        
    }
}
