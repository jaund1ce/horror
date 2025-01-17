using UnityEngine;
using System.Collections;

public class RandomRotationOnDestroy : MonoBehaviour
{
    // �ν����Ϳ��� ������ ��� ������Ʈ�� (�ִ� 11��)
    public GameObject[] targetObjects = new GameObject[11];


    private void OnDestroy()
    {

        // ��� Ÿ�� ������Ʈ�� ���� ó��
        foreach (GameObject targetObject in targetObjects)
        {
            // targetObject�� ��ȿ���� Ȯ��
            if (targetObject != null)
            {
                // 2�� ���� �� ȸ���� ������ MainGameManager���� ����
                MainGameManager.Instance.StartCoroutine(SetRandomRotationWithDelay(targetObject));
            }
            else
            {
                Debug.LogWarning("targetObject�� �������� �ʾҽ��ϴ�.");
            }
        }
    }

    // 2�� ���� �� ���� ȸ���� �����ϴ� �ڷ�ƾ
    private IEnumerator SetRandomRotationWithDelay(GameObject targetObject)
    {
        yield return new WaitForSeconds(2f);
        if (targetObject != null)
        {
            SoundManger.Instance.MakeEnviornmentSound("Statue_Moving");
            // 90, 180, 270 �� �ϳ��� �������� ����
            int[] rotationValues = { 90, 180, 270 };
            int randomIndex = Random.Range(0, rotationValues.Length);
            float randomYRotation = rotationValues[randomIndex];

            // targetObject�� ȸ���� ����
            Vector3 currentRotation = targetObject.transform.eulerAngles;
            targetObject.transform.eulerAngles = new Vector3(currentRotation.x, randomYRotation, currentRotation.z);
        }
    }
}
