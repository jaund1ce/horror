using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : ObjectBase
{
    [field:SerializeField]private Transform hinge; // 문 힌지
    private float openAngle = -90f; // 문 열리는 각도
    private float closeAngle = 0f; // 문 닫히는 각도
    private float openSpeed = 5f; // 문 열림 속도
    [HideInInspector] public bool IsLocked = true;
    [HideInInspector] public bool IsOpened = false; // 문이 열렸는지 여부
    private AudioSource audioSource;
    [SerializeField] private AudioClip openSound; // 문이 열릴 때의 소리
    [SerializeField] private AudioClip closeSound; // 문이 닫힐 때의 소리
    [SerializeField] private AudioClip lockSound; // 문이 닫힐 때의 소리

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
        //잠긴문 Interact 시 Sound 추가
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

