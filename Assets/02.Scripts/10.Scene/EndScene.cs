using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : SceneBase
{
    public GameObject targetObject; // ��Ȱ��ȭ�� ������Ʈ

    public float delay = 26f;        // ���� �ð�
    public float delay2 = 18f;       // ���� ��� ���� �ð�
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
            targetObject.SetActive(false); // ������Ʈ ��Ȱ��ȭ
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
