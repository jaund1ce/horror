using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class InteractableWardrobe : ObjectBase
{
    private Transform wardrobeTransform;
    private Transform originTransform;
    public bool IsLocked;
    private bool isOpened = false;
    private string close = "Close";
    private string open = "Open";
    private float openDistance = 0.2f;
    private float animationDuration = 0.5f;

    protected override void OnEnable()
    {
        base.OnEnable();
        wardrobeTransform = this.transform;
    }

    public override string GetInteractPrompt()
    {
        return isOpened ? close : open;
    }

    public override void OnInteract()
    {
        ToggleWardrobe();
    }

    private void ToggleWardrobe()
    {
        if (IsLocked) return;

        isOpened = !isOpened;

        StopAllCoroutines();
        StartCoroutine(DragWardrobe(isOpened ? openDistance : -openDistance));
    }

    private IEnumerator DragWardrobe(float targetDistance)
    {
        Vector3 startPosition = wardrobeTransform.localPosition;
        Vector3 targetPosition = startPosition + new Vector3(0f, 0f, targetDistance);

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            wardrobeTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        wardrobeTransform.localPosition = targetPosition;
    }
}

