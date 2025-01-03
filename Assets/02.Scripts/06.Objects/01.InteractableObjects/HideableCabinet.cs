using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HideableCabinet : ObjectBase
{
    public Transform cabinetDoor; // 캐비닛 문 Transform
    public Collider cabinetDoorCollider; // 캐비닛 문 Collider
    public float doorOpenAngle = 90f; // 문 열림 각도
    public float doorSpeed = 2f; // 문 열림 속도

    [SerializeField]private bool isDoorOpen = false; // 문이 열렸는지 여부

    protected override void OnEnable()
    {
        base.OnEnable();
        //CombineAllCollider();
    }

    private IEnumerator OpenDoor()
    {
        SoundManger.Instance.MakeEnviormentSound("CabinetOpen", 1f);
        isDoorOpen = true;

        Quaternion initialRotation = cabinetDoor.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);

        float elapsedTime = 0f;
        while (elapsedTime < 1f / doorSpeed)
        {
            elapsedTime += Time.deltaTime * doorSpeed;
            cabinetDoor.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
            yield return null;
        }

        cabinetDoor.localRotation = targetRotation;

        Debug.Log("Cabinet door opened.");
    }

    private IEnumerator CloseDoor()
    {
        SoundManger.Instance.MakeEnviormentSound("CabinetClose", 1f);
        isDoorOpen = false;

        Quaternion initialRotation = cabinetDoor.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);

        float elapsedTime = 0f;
        while (elapsedTime < 1f / doorSpeed)
        {
            elapsedTime += Time.deltaTime * doorSpeed;
            cabinetDoor.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
            yield return null;
        }

        cabinetDoor.localRotation = targetRotation;

        Debug.Log("Cabinet door closed.");
    }

    public override void OnInteract()
    {
        if (!isDoorOpen)
        {
            StartCoroutine(OpenDoor());
        }
        else
        {
            StartCoroutine(CloseDoor());
        }
    }

    public override string GetInteractPrompt()
    {
        if (!isDoorOpen) return "Open";
        else if (isDoorOpen) return "Close";
        else return "Err";
    }

    private void CombineAllCollider()
    {
        Bounds combineBounds = new Bounds();

        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            if (combineBounds.size == Vector3.zero)
            {
                combineBounds = collider.bounds;
            }
            else
            {
                combineBounds.Encapsulate(collider.bounds);
            }
        }
    }
}