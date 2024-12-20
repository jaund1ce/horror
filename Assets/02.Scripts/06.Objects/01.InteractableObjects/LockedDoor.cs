using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : ObjectBase
{
    public Transform hinge; // �� ����
    public float openAngle = -90f; // �� ������ ����
    public float closeAngle = 0f; // �� ������ ����
    public float openSpeed = 5f; // �� ���� �ӵ�
    private bool isOpened = false; // ���� ���ȴ��� ����
    public bool IsLocked = true;

    public event Action isOpen;

    protected override void OnEnable()
    {
        base.OnEnable();
        hinge = this.transform;
    }
    private void Start()
    {
        IsLocked = ObjectSO.IsLocked;
    }

    public override void OnInteract()
    {
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        //��乮 Interact �� Sound �߰�
        if (IsLocked) return;
        
        isOpened = !isOpened;
        if (isOpened == true)
        {
            isOpen?.Invoke();
        }

        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpened ? openAngle : closeAngle));
    }

    private IEnumerator RotateDoor(float targetAngle)
    {
        Debug.Log("������ �ڷ�ƾ ����");
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
        return isOpened ? "Close" : "Open";
    }

}

