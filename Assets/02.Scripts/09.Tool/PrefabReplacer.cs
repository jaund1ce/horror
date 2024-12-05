#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PrefabReplacer : MonoBehaviour
{
    [Header("Objects to Replace")]
    public GameObject[] objectsToReplace; // ��ü�� ������Ʈ��
    public GameObject prefabToReplaceWith; // �� ������
    public string savePath = "C:\\Users\\wjddy\\OneDrive\\����\\GitHub\\horror\\Assets\\03.Prefabs\\07.Changed"; // ������ ���� ���

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

            // ���� ������Ʈ�� Transform ������ ��������
            Vector3 position = obj.transform.position;
            Quaternion rotation = obj.transform.rotation;
            Vector3 scale = obj.transform.localScale;
            Transform parent = obj.transform.parent;

            // ���� ������Ʈ ����
            DestroyImmediate(obj);

            // �� ������ ���� �� ��ġ/ȸ��/������ ����
            GameObject newObject = Instantiate(prefabToReplaceWith, position, rotation, parent);
            newObject.transform.localScale = scale;
            newObject.name = prefabToReplaceWith.name + "_" + i; // ������ �̸� �ο�

#if UNITY_EDITOR
            // ������ ��η� ������ ����
            string prefabPath = savePath + newObject.name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(newObject, prefabPath);
            Debug.Log($"Saved prefab at {prefabPath}");
#endif
        }

        Debug.Log("All objects replaced and saved as unique prefabs.");
    }
}
