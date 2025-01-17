using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum eAssetType //열거형(Enumeration)으로 여러 값들에 이름을 붙여주는 자료형
{
    Prefab,       // 프리팹 자산, <GameObject>
    Thumbnail,    // 썸네일 이미지 <Sprite>
    UI,           // UI 관련 자산 <GameObject>
    Data,         // 데이터 관련 파일 <TextAsset>
    SO,           // ScriptableObject <ScriptableObject>
    Sound         // 사운드 파일 <AudioClip>
}

public enum eCategoryType
{
    None,         // 분류 없음
    Item,         // 아이템 자산
    Enemy,          // NPC 관련 자산
    Stage,        // 스테이지 자산
    Player,    // 캐릭터 관련 자산
    Maps,         // 맵 데이터
    Model,         // 3D 모델
    Paper
}

public class ResourceManager : mainSingleton<ResourceManager> // Singleton<T>을 상속 받음, Instance 생성 가능
{
    private Dictionary<string, object> assetPool = new Dictionary<string, object>(); // HDD, SSD의 Resource폴더에서 RAM(메모리)으로 게임에서 사용 할 파일들을 Dictionary자료구조로 assetPool이라는 변수(필드)으로 저장하기 위해서.
                                                                                     // Dictionary자료구조는 string(이름,문자)을 식별자로 사용하여 RAM에 object(모든 데이터 타입의 기본 클래스, 모든 값형(Value Type)과 참조형(Reference Type)은 암묵적으로 object를 상속, C#에서 모든 데이터는 object로 취급될 수 있음)를 저장하거나 사용.
                                                                                     // 매번 파일에서 불러오는게 아닌 필요한 데이터를 assetPool이라는 이름으로 미리 저장해 두어 빠르게 동작 할 수 있음(Caching,캐싱)
                                                                                     // 매번 파일에서 불러온다면 한 파일(HDD,SSD)에서 중복 자산(RAM)으로 불러 올 수가 있는데, 이를 방지 할 수도 있습니다.
                                                                                     // Dictionary<string, object> 자료형인 assetPool 변수를 선언, assetPool은 생성 된 Dictionary<string, object>() 객체를 참조(주소를 가리킨다)한다.
                                                                                     // assetPool(주소록의 주소), Dictionary<string, object>(주소록, string:식별자, object:메모리에 생성 된 개체의 메모리 주소)
                                                                                     // assetPool ---> Dictionary<string, object> (주소록)
                                                                                     //                └── "PlayerPrefab_Prefab/Character" ---> GameObject(RAM에 로드된 객체)
                                                                                     //                └── "BackgroundMusic_Sound"       ---> AudioClip(RAM에 로드된 객체)
    public T LoadAsset<T>(string key, eAssetType assetType, eCategoryType categoryType = eCategoryType.None) // 경로 : Resources/eAssetType중 하나/eCategoryType 중 하나(경로 생략 가능)/파일 이름
                                                                                                             // T(여러가지 자료형으로 변화 가능, 호출시점에서 결정가능) : 지금은 자산 유형(GameObject, AudioClip, TextAsset, Sprite, ScriptableObject 등)으로 사용(enum eAssetType 참고)
                                                                                                             // 예시 : LoadAsset<TextAsset>("ItemSO", eAssetType.Data);
                                                                                                             // Resources/Data/ItemSO 파일을 TextAsset의 유형으로  Dictionary자료구조의 assetPool(RAM,메모리에 위치함)에 "ItemSO_Data" 라는 고유 키를 가진 자산으로 저장한다. 
    {
        T handle = default; // 호출 된 후 입력된 자료형을 가진 handle이라는 지역변수를 선언하고 default(0,false,null)를 할당한다.

        var typeStr = $"{assetType}{(categoryType == eCategoryType.None ? "" : $"/{categoryType}")}"; // 지역변수 categortType을 읽어(==), (?와:,삼항연산자를 사용해) 그 내용물이 eCategoryType.None가 맞을(true) 때 ""(아무것도 없음)을 반환합니다.
                                                                                                      // eCategoryType이 None이 아닐(false) 때(위의 enum eCategoryType참고) $"/{categoryType}"을 반환합니다.
                                                                                                      // $(문자열 보간)의 사용으로 지역변수 typeStr을 지역변수 assetType과 categortType의 내용물에 따라 동적으로 생성 할 수 있습니다.
                                                                                                      // var로 지역변수 typeStr의 자료형을 = 다음에 오는 값을 보고 추론하여 결정(지금은 $,문자열 보간을 사용하니 string) 

        if (!assetPool.ContainsKey(key + "_" + typeStr)) // 현재 Merhod가 LoadAsset<TextAsset>("ItemSO", eAssetType.Data); 이렇게 호출되었을 때 key = ItemSO, typeStr = Data 로  "ItemSO_Data" 라는 것이
                                                         // assetPool이 참조하는 Dictionary<string, object>에 존재하다면 ContainsKey를 통해 true을 반환하지만 !를 통해 false가 조건으로 나온다. if문 동작X
                                                         // 존재하지 않는다면 false을 반환하여 !를 통해 true가 조건으로 나와 if문이 동작한다.
        {
            var obj = Resources.Load($"{typeStr}/{key}", typeof(T)); // Resources 폴더에서 정해진 경로의 파일을 정해진 유형의 자산으로 RAM(메모리)에 로드. 즉, 메모리에 객체를 생성
                                                                     // $(문자열 보간)의 사용으로 자산으로 사용 될 파일의 경로를 typeStr, key 지역변수에 따라 동적으로 넣을 수 있다.
                                                                     // 경로 : Resources/{typeStr}/{key}, 유형 : T에 따라 지정(typeof(TextAsset), typeof(GameObject)) 
                                                                     // var로 typeof(T)를 통해 지정 된 유형에 맞는 자료형을 가진 obj라는 지역변수로 메모리에 생성 된 객체를 참조

            if (obj == null)    // 지역변수 obj를 읽었을 때(==) null 이라면 true, 아니면 false를 조건으로 가진다
                                // Resources/{typeStr}/{key} 경로 상에 맞는 파일이 없다면, 참조 할 객체가 없는 지역변수 obj는 null을 참조 한다.
                return default; // if문이 동작하면 Method를 종료하고 default(0,false,null)를 Method가 호출 된 곳으로 반환한다.

            assetPool.Add(key + "_" + typeStr, obj); //assetPool이 참조하는 Dictionary<string, object>에 string자료형의 key_typeStr식별자를 가진 object자료형의 obj참조값(실제 메모리에 생성 된 객체를 참조하는 변수)을 추가한다.
        }

        handle = (T)assetPool[key + "_" + typeStr]; // assetPool이 참조하는 Dictionary<string, object>에서 string자료형의 key + "_" + typeStr이라는 식별자를 사용하여 검색
                                                    // 검색 결과  Dictionary<string, object>에 object 자료형으로 저장 된 Resources.Load로 로드된 객체의 참조값 obj을 반환
                                                    // object 자료형을 호출 시 지정된 자료형(T)으로 강제 변환하여 handle에 할당한다.
        return handle;                              // handle을 Method가 호출 된 곳으로 반환한다.
    }

    public async Task<T> LoadAsyncAsset<T>(string key, eAssetType assetType, eCategoryType categoryType = eCategoryType.None) //비동기 방식(실행 흐름을 멈추지 않고 다른 작업을 계속 진행할 수 있음), 호출 될 떄 await 키워드를 사용, 호출 Mathod에도 async로 선언
    {
        T handle = default;

        var typeStr = $"{assetType}{(categoryType == eCategoryType.None ? "" : $"/{categoryType}")}";

        if (!assetPool.ContainsKey(key + "_" + typeStr))
        {
            ResourceRequest op = Resources.LoadAsync($"{typeStr}/{key}", typeof(T)); //비동기로 $"{typeStr}/{key}"경로의 파일을 T자료형의 자산으로 사용하기 위해 메모리에  ResourceRequest자료형의 객체(속성 : isDone, asset 등등)로 반환한다.

            while (!op.isDone) // ResourceRequest가 완료 된 것을 수동으로 확인(비동기 작업은 작업 완료 후 콜백을 제공하지 않음, isDone을 사용하지 않으면 작업이 완료되기 전에 결과를 가져오려는 문제로 null이 반환된다.)
            {
                await Task.Yield(); // 현재 비동기 작업을 다음 프레임으로 넘기고, 메인 스레드는 다른 작업을 처리
            }

            var obj = op.asset; //비동기 작업의 결과(asset)를 지역변수 obj에 할당한다.

            if (obj == null)
                return default;

            assetPool.Add(key + "_" + typeStr, obj);
        }

        handle = (T)assetPool[key + "_" + typeStr];

        return handle;
    }
}
