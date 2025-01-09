using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : SceneBase
{
    public GameObject targetObject; // 비활성화할 오브젝트

    public float delay = 26f;        // 지연 시간
    public float delay2 = 18f;       // 음악 재생 지연 시간
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ActivateObject", delay);
        Invoke("ActivateSound", delay2);
    }


    void ActivateObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false); // 오브젝트 비활성화
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UIManager.Instance.Show<EndUI>();
        }
    }

    void ActivateSound()
    {
        SoundManger.Instance.ChangeBGMSound(4);
    }

    void ResetSound()
    {
        SoundManger.Instance.ResetAllSounds();
    }
}
