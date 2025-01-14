using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : ObjectBase
{
    [field:SerializeField]private Transform hinge; // �� ����
    private float openAngle = -90f; // �� ������ ����
    private float closeAngle = 0f; // �� ������ ����
    private float openSpeed = 5f; // �� ���� �ӵ�
    [HideInInspector] public bool IsLocked = true;
    [HideInInspector] public bool IsOpened = false; // ���� ���ȴ��� ����
    private AudioSource audioSource;
    [SerializeField] private AudioClip openSound; // ���� ���� ���� �Ҹ�
    [SerializeField] private AudioClip closeSound; // ���� ���� ���� �Ҹ�
    [SerializeField] private AudioClip lockSound; // ���� ���� ���� �Ҹ�

    public event Action isOpen;

    protected override void OnEnable()
    {
        base.OnEnable();
        hinge = this.transform;
    }
    private void Start()
    {
        IsLocked = ObjectSO.IsLocked;
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnInteract()
    {
        ToggleDoor();
    }

    public void ToggleDoor()
    {
        //��乮 Interact �� Sound �߰�
        if (IsLocked)
        {
            PlaySound(lockSound);
            return;
        }

        IsOpened = !IsOpened;
        if (IsOpened == true)
        {
            PlaySound(openSound);
            isOpen?.Invoke();
        }
        else
        {
            PlaySound(closeSound);
        }

        StopAllCoroutines();
        StartCoroutine(RotateDoor(IsOpened ? openAngle : closeAngle));
    }

    private IEnumerator RotateDoor(float targetAngle)
    {
        float currentAngle = hinge.localEulerAngles.y;
        if (currentAngle > 180) currentAngle -= 360;

        while (Mathf.Abs(currentAngle - targetAngle) > 0.1f)
        {
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * openSpeed);
            hinge.localEulerAngles = new Vector3(0, currentAngle, 0);
            yield return null;
        }

        hinge.localEulerAngles = new Vector3(0, targetAngle, 0);
    }

    public override string GetInteractPrompt()
    {
        if (IsLocked) return "Locked";
        return IsOpened ? "Close" : "Open";
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

}

