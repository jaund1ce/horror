using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum eAssetType //������(Enumeration)���� ���� ���鿡 �̸��� �ٿ��ִ� �ڷ���
{
    Prefab,       // ������ �ڻ�, <GameObject>
    Thumbnail,    // ����� �̹��� <Sprite>
    UI,           // UI ���� �ڻ� <GameObject>
    Data,         // ������ ���� ���� <TextAsset>
    SO,           // ScriptableObject <ScriptableObject>
    Sound         // ���� ���� <AudioClip>
}

public enum eCategoryType
{
    None,         // �з� ����
    Item,         // ������ �ڻ�
    Enemy,          // NPC ���� �ڻ�
    Stage,        // �������� �ڻ�
    Player,    // ĳ���� ���� �ڻ�
    Maps,         // �� ������
    Model,         // 3D ��
    Paper
}

public class ResourceManager : mainSingleton<ResourceManager> // Singleton<T>�� ��� ����, Instance ���� ����
{
    private Dictionary<string, object> assetPool = new Dictionary<string, object>(); // HDD, SSD�� Resource�������� RAM(�޸�)���� ���ӿ��� ��� �� ���ϵ��� Dictionary�ڷᱸ���� assetPool�̶�� ����(�ʵ�)���� �����ϱ� ���ؼ�.
                                                                                     // Dictionary�ڷᱸ���� string(�̸�,����)�� �ĺ��ڷ� ����Ͽ� RAM�� object(��� ������ Ÿ���� �⺻ Ŭ����, ��� ����(Value Type)�� ������(Reference Type)�� �Ϲ������� object�� ���, C#���� ��� �����ʹ� object�� ��޵� �� ����)�� �����ϰų� ���.
                                                                                     // �Ź� ���Ͽ��� �ҷ����°� �ƴ� �ʿ��� �����͸� assetPool�̶�� �̸����� �̸� ������ �ξ� ������ ���� �� �� ����(Caching,ĳ��)
                                                                                     // �Ź� ���Ͽ��� �ҷ��´ٸ� �� ����(HDD,SSD)���� �ߺ� �ڻ�(RAM)���� �ҷ� �� ���� �ִµ�, �̸� ���� �� ���� �ֽ��ϴ�.
                                                                                     // Dictionary<string, object> �ڷ����� assetPool ������ ����, assetPool�� ���� �� Dictionary<string, object>() ��ü�� ����(�ּҸ� ����Ų��)�Ѵ�.
                                                                                     // assetPool(�ּҷ��� �ּ�), Dictionary<string, object>(�ּҷ�, string:�ĺ���, object:�޸𸮿� ���� �� ��ü�� �޸� �ּ�)
                                                                                     // assetPool ---> Dictionary<string, object> (�ּҷ�)
                                                                                     //                ������ "PlayerPrefab_Prefab/Character" ---> GameObject(RAM�� �ε�� ��ü)
                                                                                     //                ������ "BackgroundMusic_Sound"       ---> AudioClip(RAM�� �ε�� ��ü)
    public T LoadAsset<T>(string key, eAssetType assetType, eCategoryType categoryType = eCategoryType.None) // ��� : Resources/eAssetType�� �ϳ�/eCategoryType �� �ϳ�(��� ���� ����)/���� �̸�
                                                                                                             // T(�������� �ڷ������� ��ȭ ����, ȣ��������� ��������) : ������ �ڻ� ����(GameObject, AudioClip, TextAsset, Sprite, ScriptableObject ��)���� ���(enum eAssetType ����)
                                                                                                             // ���� : LoadAsset<TextAsset>("ItemSO", eAssetType.Data);
                                                                                                             // Resources/Data/ItemSO ������ TextAsset�� ��������  Dictionary�ڷᱸ���� assetPool(RAM,�޸𸮿� ��ġ��)�� "ItemSO_Data" ��� ���� Ű�� ���� �ڻ����� �����Ѵ�. 
    {
        T handle = default; // ȣ�� �� �� �Էµ� �ڷ����� ���� handle�̶�� ���������� �����ϰ� default(0,false,null)�� �Ҵ��Ѵ�.

        var typeStr = $"{assetType}{(categoryType == eCategoryType.None ? "" : $"/{categoryType}")}"; // �������� categortType�� �о�(==), (?��:,���׿����ڸ� �����) �� ���빰�� eCategoryType.None�� ����(true) �� ""(�ƹ��͵� ����)�� ��ȯ�մϴ�.
                                                                                                      // eCategoryType�� None�� �ƴ�(false) ��(���� enum eCategoryType����) $"/{categoryType}"�� ��ȯ�մϴ�.
                                                                                                      // $(���ڿ� ����)�� ������� �������� typeStr�� �������� assetType�� categortType�� ���빰�� ���� �������� ���� �� �� �ֽ��ϴ�.
                                                                                                      // var�� �������� typeStr�� �ڷ����� = ������ ���� ���� ���� �߷��Ͽ� ����(������ $,���ڿ� ������ ����ϴ� string) 

        if (!assetPool.ContainsKey(key + "_" + typeStr)) // ���� Merhod�� LoadAsset<TextAsset>("ItemSO", eAssetType.Data); �̷��� ȣ��Ǿ��� �� key = ItemSO, typeStr = Data ��  "ItemSO_Data" ��� ����
                                                         // assetPool�� �����ϴ� Dictionary<string, object>�� �����ϴٸ� ContainsKey�� ���� true�� ��ȯ������ !�� ���� false�� �������� ���´�. if�� ����X
                                                         // �������� �ʴ´ٸ� false�� ��ȯ�Ͽ� !�� ���� true�� �������� ���� if���� �����Ѵ�.
        {
            var obj = Resources.Load($"{typeStr}/{key}", typeof(T)); // Resources �������� ������ ����� ������ ������ ������ �ڻ����� RAM(�޸�)�� �ε�. ��, �޸𸮿� ��ü�� ����
                                                                     // $(���ڿ� ����)�� ������� �ڻ����� ��� �� ������ ��θ� typeStr, key ���������� ���� �������� ���� �� �ִ�.
                                                                     // ��� : Resources/{typeStr}/{key}, ���� : T�� ���� ����(typeof(TextAsset), typeof(GameObject)) 
                                                                     // var�� typeof(T)�� ���� ���� �� ������ �´� �ڷ����� ���� obj��� ���������� �޸𸮿� ���� �� ��ü�� ����

            if (obj == null)    // �������� obj�� �о��� ��(==) null �̶�� true, �ƴϸ� false�� �������� ������
                                // Resources/{typeStr}/{key} ��� �� �´� ������ ���ٸ�, ���� �� ��ü�� ���� �������� obj�� null�� ���� �Ѵ�.
                return default; // if���� �����ϸ� Method�� �����ϰ� default(0,false,null)�� Method�� ȣ�� �� ������ ��ȯ�Ѵ�.

            assetPool.Add(key + "_" + typeStr, obj); //assetPool�� �����ϴ� Dictionary<string, object>�� string�ڷ����� key_typeStr�ĺ��ڸ� ���� object�ڷ����� obj������(���� �޸𸮿� ���� �� ��ü�� �����ϴ� ����)�� �߰��Ѵ�.
        }

        handle = (T)assetPool[key + "_" + typeStr]; // assetPool�� �����ϴ� Dictionary<string, object>���� string�ڷ����� key + "_" + typeStr�̶�� �ĺ��ڸ� ����Ͽ� �˻�
                                                    // �˻� ���  Dictionary<string, object>�� object �ڷ������� ���� �� Resources.Load�� �ε�� ��ü�� ������ obj�� ��ȯ
                                                    // object �ڷ����� ȣ�� �� ������ �ڷ���(T)���� ���� ��ȯ�Ͽ� handle�� �Ҵ��Ѵ�.
        return handle;                              // handle�� Method�� ȣ�� �� ������ ��ȯ�Ѵ�.
    }

    public async Task<T> LoadAsyncAsset<T>(string key, eAssetType assetType, eCategoryType categoryType = eCategoryType.None) //�񵿱� ���(���� �帧�� ������ �ʰ� �ٸ� �۾��� ��� ������ �� ����), ȣ�� �� �� await Ű���带 ���, ȣ�� Mathod���� async�� ����
    {
        T handle = default;

        var typeStr = $"{assetType}{(categoryType == eCategoryType.None ? "" : $"/{categoryType}")}";

        if (!assetPool.ContainsKey(key + "_" + typeStr))
        {
            ResourceRequest op = Resources.LoadAsync($"{typeStr}/{key}", typeof(T)); //�񵿱�� $"{typeStr}/{key}"����� ������ T�ڷ����� �ڻ����� ����ϱ� ���� �޸𸮿�  ResourceRequest�ڷ����� ��ü(�Ӽ� : isDone, asset ���)�� ��ȯ�Ѵ�.

            while (!op.isDone) // ResourceRequest�� �Ϸ� �� ���� �������� Ȯ��(�񵿱� �۾��� �۾� �Ϸ� �� �ݹ��� �������� ����, isDone�� ������� ������ �۾��� �Ϸ�Ǳ� ���� ����� ���������� ������ null�� ��ȯ�ȴ�.)
            {
                await Task.Yield(); // ���� �񵿱� �۾��� ���� ���������� �ѱ��, ���� ������� �ٸ� �۾��� ó��
            }

            var obj = op.asset; //�񵿱� �۾��� ���(asset)�� �������� obj�� �Ҵ��Ѵ�.

            if (obj == null)
                return default;

            assetPool.Add(key + "_" + typeStr, obj);
        }

        handle = (T)assetPool[key + "_" + typeStr];

        return handle;
    }
}
