using UnityEngine;
using System.Collections;

public class RandomRotationOnDestroy : MonoBehaviour
{
    // 인스펙터에서 설정할 대상 오브젝트들 (최대 11개)
    public GameObject[] targetObjects = new GameObject[11];


    private void OnDestroy()
    {

        // 모든 타겟 오브젝트에 대해 처리
        foreach (GameObject targetObject in targetObjects)
        {
            // targetObject가 유효한지 확인
            if (targetObject != null)
            {
                // 2초 지연 후 회전값 설정을 MainGameManager에서 실행
                MainGameManager.Instance.StartCoroutine(SetRandomRotationWithDelay(targetObject));
            }
            else
            {
                Debug.LogWarning("targetObject가 설정되지 않았습니다.");
            }
        }
    }

    // 2초 지연 후 랜덤 회전을 설정하는 코루틴
    private IEnumerator SetRandomRotationWithDelay(GameObject targetObject)
    {
        yield return new WaitForSeconds(2f);
        if (targetObject != null)
        {
            SoundManger.Instance.MakeEnviornmentSound("Statue_Moving");
            // 90, 180, 270 중 하나를 랜덤으로 선택
            int[] rotationValues = { 90, 180, 270 };
            int randomIndex = Random.Range(0, rotationValues.Length);
            float randomYRotation = rotationValues[randomIndex];

            // targetObject의 회전값 설정
            Vector3 currentRotation = targetObject.transform.eulerAngles;
            targetObject.transform.eulerAngles = new Vector3(currentRotation.x, randomYRotation, currentRotation.z);
        }
    }
}
