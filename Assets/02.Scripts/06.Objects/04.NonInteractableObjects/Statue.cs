using UnityEngine;
using System.Collections;

public class Statue : MonoBehaviour
{
    private bool isRotating = false;

    private void OnEnable()
    {
        // MainGameManager���� OnObjectDestroyed �̺�Ʈ�� ����
        OnDestroyPaper.OnObjectDestroyed += HandleObjectDestroyed;
    }

    private void OnDisable()
    {
        // ���� ����
        OnDestroyPaper.OnObjectDestroyed -= HandleObjectDestroyed;
    }

    // ������Ʈ�� �ı��Ǿ��� �� ����
    private void HandleObjectDestroyed(GameObject destroyedObject)
    {
            StartCoroutine(SetRandomRotationWithDelay());
    }

    // 2�� ���� �� ���� ȸ���� �����ϴ� �ڷ�ƾ
    private IEnumerator SetRandomRotationWithDelay()
    {

        if (isRotating)
        yield break;

        isRotating = true;
        yield return new WaitForSeconds(2f);

        if (this != null)
        {
            SoundManger.Instance.MakeEnviornmentSound("Statue_Moving");

            // 90, 180, 270 �� �ϳ��� �������� ����
            int[] rotationValues = { 90, 180, 270 };
            int randomIndex = Random.Range(0, rotationValues.Length);
            float randomYRotation = rotationValues[randomIndex];

            // targetObject�� ȸ���� ����
            Vector3 currentRotation = transform.eulerAngles;
            transform.eulerAngles = new Vector3(currentRotation.x, randomYRotation, currentRotation.z);
        }

        isRotating = false;
    }
}
