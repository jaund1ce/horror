#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PrefabReplacer : MonoBehaviour
{
    [Header("Objects to Replace")]
    public GameObject[] objectsToReplace; // 대체할 오브젝트들
    public GameObject prefabToReplaceWith; // 새 프리팹
    public string savePath = "C:\\Users\\wjddy\\OneDrive\\문서\\GitHub\\horror\\Assets\\03.Prefabs\\07.Changed"; // 프리팹 저장 경로

    public void ReplaceAndSave()
    {
        if (prefabToReplaceWith == null || objectsToReplace == null || objectsToReplace.Length == 0)
        {
            Debug.LogError("Prefab or objects to replace are not assigned!");
            return;
        }

        for (int i = 0; i < objectsToReplace.Length; i++)
        {
            GameObject obj = objectsToReplace[i];
            if (obj == null)
                continue;

            // 기존 오브젝트의 Transform 데이터 가져오기
            Vector3 position = obj.transform.position;
            Quaternion rotation = obj.transform.rotation;
            Vector3 scale = obj.transform.localScale;
            Transform parent = obj.transform.parent;

            // 기존 오브젝트 삭제
            DestroyImmediate(obj);

            // 새 프리팹 생성 및 위치/회전/스케일 설정
            GameObject newObject = Instantiate(prefabToReplaceWith, position, rotation, parent);
            newObject.transform.localScale = scale;
            newObject.name = prefabToReplaceWith.name + "_" + i; // 고유한 이름 부여

#if UNITY_EDITOR
            // 고유한 경로로 프리팹 저장
            string prefabPath = savePath + newObject.name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(newObject, prefabPath);
            Debug.Log($"Saved prefab at {prefabPath}");
#endif
        }

        Debug.Log("All objects replaced and saved as unique prefabs.");
    }
}
