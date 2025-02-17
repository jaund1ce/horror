# Shhh... (쉿!)

## 1. 개요
<br><img src = "https://github.com/user-attachments/assets/faea89d8-f44b-4ac8-b602-a67d495e5987">
- 장르 : 3D, FPS, 호러, 퍼즐, 백룸
- 플랫폼 : PC (Window)
- 배포 방식 : itch.io
- 개발 기간 : 2개월(2024.11.25 ~ 2025.01.21 : 기획 기간 포함)
- 개발 인원 : 4명 (<a href="https://github.com/ParkHyeonDo">박현도</a>, <a href="https://github.com/jaund1ce">이지성</a>, <a href="https://github.com/leehun1997" title="GitHub Profile">이훈</a>, <a href="https://github.com/PRODOINGER">정용화</a>) + 1명(외부 애니메이터)
- 게임 레퍼런스 : Escape The Backroom
  
<details>
<summary> 게임 소개 이미지</summary>
  
<img src = "https://github.com/user-attachments/assets/71e916bb-7790-4fc9-80ef-7209f2fbbcc0">
<img src = "https://github.com/user-attachments/assets/1d92ed69-3c40-4a04-b154-c565caf942cf">

</details>
게임 소개 영상 : https://www.youtube.com/watch?v=a_Fyem_ytXI&t=2s

게임 링크 : https://prodoinger.itch.io/shhh

## 2. 게임 플레이 방법
- 플레이어
  - 이동 : wasd
  - 점프 : space
  - 달리기 : shift
  - 앉기 : ctrl
  - 상호작용 : f
  - 메뉴 : esc
  - 인벤토리 : tab
  - 아이템 사용 : 마우스 좌클릭
  - 퀵 슬롯 : 1 2 3 4(번호)

## 3. 사용한 도구
- 협업 도구
  - Git, Github Desktop
  - Jira
  - Figma
  - Slack, Zep
  - Discord
  - Google Drive
- 개발 도구
  - C#
  - Unity (2022.3.17f1 ver)
  - Visual studio
  - Json (Google Spreed Sheet)
## 4. 게임의 흐름과 사용한 기술
### a. 인게임 기능

<details>
<summary><b>Json을 활용한 데이터 저장 및 로드</b></summary>
<br>Save 버튼 클릭 시 각 저장하여야 하는 컴포넌트별로  하이어라키를 전체 탐색하여 저장될 객체를 찾습니다. 
<br>저장 할 해당 프리팹의 고유 키, 이름, 포지션 등을 딕셔너리로 저장하고 그 딕셔너리를 JsonData로 변환하여 고유의 파일로 저장합니다.  
<br>Load 버튼 클릭 시 해당 씬으로 변경되게 되며, Json으로 저장되어있는 파일을 딕셔너리로 변환 후 맵, 플레이어, 아이템, 적 등 순으로 프리팹을 생성하여 맵에 배치합니다. 
<br> 해당 프리팹에 변경점이 필요한 경우 Instantiate 시에 해당 컴포넌트의 값을 변경하여 생성합니다.

<br><br><img src = "https://github.com/user-attachments/assets/17a74ac7-d261-4b9f-a7a1-9944ad5e7fbf" width="400" height="300">
<img src = "https://github.com/user-attachments/assets/902022ef-bf39-4d73-9151-4907fdf187dc" width="400" height="300">
</details>

<details>
<summary><b> Input System  </b></summary>
<br>Input System 의 구독과 해제 기능을 사용하여 다른 스크립트들이 PlayerController 스크립트에 달린 Input system에 구독을 하는 방식입니다.
<br>한번의 입력으로 여러 수행이 가능하면서도, 특정 행동에서는 사용자가 예상 가능한 수행만 가능하도록 하였고, 
<br>플레이어가 존재하지 않는 등의 특수한 경우, 스스로 input system을 선언하여 사용하고 삭제하는 방식을 사용하였습니다.

  <br>( Shift를 누르면 statemachine을 변경하면서, 다른 스크립트의 값도 변경 // 인벤토리를 이용 중이거나 키패드와 상호작용 중일때 아이템의 사용이 불가능하게 만듦 )

<br><br><img src = "https://github.com/user-attachments/assets/ff9d8743-6ee7-4696-9ac5-1eada93613dc" width="400" height="300">
<img src = "https://github.com/user-attachments/assets/7c1b4873-1054-4730-9c9f-3b92d4ddb21e" width="400" height="300">

<br><br><img src = "https://github.com/user-attachments/assets/5d4c96ad-e88e-4044-bfdf-efd82a98b8d0" width="400" height="300">
<img src = "https://github.com/user-attachments/assets/7ad6b5cf-939c-40b7-b53f-c36b915632bd" width="400" height="300">
</details>

<details>
<summary><b> CinemachineCamera를 활용한 카메라 이동, 연출 , Postprocesisng을 이용한 화면 효과 </b></summary>
<br>CinemachineCamera를 활용하여 priority를 다르게 주는 등의 방식으로, 처음 게임을 플레이할 때 나오는 인트로나 죽을 때 나오는 점프스퀘어 등의 연출을 줍니다.
<br>또한, 현재 버전의 CinemachineCamera 에서는 Postprocesisng 적용 방식이 최신 버전과는 다르기 때문에 volume을 통해서 어안렌즈 등의 원하는 카메라 효과를 넣어주고 스테이지마다 다른 분위기를 연출하였습니다.

<br><br><img src = "https://github.com/user-attachments/assets/9d4d37cd-7f37-4008-8268-76172a53e86e" width="400" height="300">
<img src = "https://github.com/user-attachments/assets/b02de825-4e87-4a15-a695-1db617fbbc00" width="200" height="300">
<img src = "https://github.com/user-attachments/assets/36db8520-bda5-476d-80ea-3c27329c623b" width="200" height="300">
</details>

<details>
<summary><b> FSM 구조의 플레이어와 몬스터들  </b></summary>
<br>FSM은 플레이어와 몬스터들은 상태(State)와 전이(Transition)를 기반으로 동작합니다. 
<br>유한한 상태 집합에서 하나의 상태만 활성 상태로 유지되며, 특정 이벤트에 따라 상태가 전이됩니다. 
<br>상태의 변화는 특정 조건에서만 이루어지기 때문에 버그 발생의 여지가 적고, 이후에 플레이어나 몬스터에게 새로운 상태가 추가되더라도 쉽게 유지보수가 가능합니다.
  
<br><br><img src = "https://github.com/user-attachments/assets/d9698a27-66f7-43ef-9c97-ad5b4ea839ed" width="400" height="300"> 
<img src = "https://github.com/user-attachments/assets/a817495d-fe98-437e-8f65-299addc97581" width="400" height="300"> 
</details>

<details>
<summary><b> 확장성을 고려하여 제작된 다양한 객체 </b></summary>
<br>Items, Enemy, InteractableObjects 등 비슷한 분류로 나누어진 각 객체들은 Interface 또는 부모스크립트 Base 를 상속받아 기능의 독립성을 유지하되, 각 필요한 공통기능을 부여받고 있습니다.  
<br>또한 , 각 객체들이 공통의 부모로부터 상속을 받는경우, 검출이나 비교 등 이 코드적으로 간편해질수 있도록 고려하여 설계하였습니다.

<br><br><img src = "https://github.com/user-attachments/assets/ea207b59-386e-44d7-b5eb-4b95430774ee" width="400" height="300">
<img src = "https://github.com/user-attachments/assets/e4115b57-8804-4b08-98b9-1c691e4f2188" width="400" height="300"> 
</details>

<details>
<summary><b> 게임성을 풍부하게 해주는 다양한 코드의 퍼즐들 </b></summary>
<br>&nbsp; 키패드 퍼즐의 경우, Physics Raycaster 와 Event Trigger 를 활용한 인게임 3D Object 클릭 시스템을 구현하여 키조작을 구현하였습니다.
<br>Interact시 LED부분이 빛 날 수있도록 EMISSION을 컨트롤하거나 코루틴을 추가하여 깜빡이는 텍스트 연출을 추가하였습니다. 
<br>&nbsp; 락픽 퍼즐의 경우, 마우스의 Delta값을 받아 락픽의 각도를 조정합니다. 
<br>마우스 클릭시 유저의 락픽 각도와 정답 각도의 오차를 계산하여 오차가 많이 날 경우 많은 떨림과 함께 락픽이 부러지게 되며,  
<br>오차가 많이 나지 않을 경우 그 값을 정규화 하여 차이가 나는 정도에 따라 문고리 돌아가는 정도를 다르게 주어 유저가 어느정도 정답에 근접한지 식별하기 용이하도록 구현하였습니다.

<br><br><img src = "https://github.com/user-attachments/assets/0bb8fe8b-1b2a-490d-bdfa-c1223d0e2a07" width="400" height="300"> 
<img src = "https://github.com/user-attachments/assets/cd5723ca-42b6-4da7-a4bf-503bd3b9e6cf" width="400" height="300"> 
<br><br><img src = "https://github.com/user-attachments/assets/fea9d418-8a82-404d-aa1f-6aa66d7ffd14" width="400" height="100">
<img src = "https://github.com/user-attachments/assets/f646ae76-fe47-4f93-991d-c81521e95644" width="400" height="100">
</details>

### b. 게임 외의 기능

  
<details>
<summary><b> Singleton 구조를 상속받는 매니져들 </b></summary>  
<br>위의 확장성을 고려한 객체 설계 중 하나로, 기본적인 싱글톤 구조를 만들고 모든 매니저들이 이를 상속받아 필요에 따라서 쉽게 접근이 가능합니다. 
<br>또한, 각 스크립트에서 추가적인 선언이 필요하지 않아 메모리 관리 효율 증가도 기대 할 수 있습니다. 싱글톤을 상속받는 매니져들은 DontDestroyOnLoad 를 활용하여, 각 객체를 유지하고 유지보수를 고려하였습니다.
<br><br>(GameManger, SoundManger 등)

<br><br><img src = "https://github.com/user-attachments/assets/628f51ab-90dd-4020-a6d7-c7d79b8d40c5" width="400" height="300">
</details>

<details>
<summary><b> Json 파일과 Editor 기능을 활용하여 SO(스크립터블 오브젝트) 구현 </b></summary>
<br>기획자나 개발자가 추후에 아이템을 추가하거나 변경하기 편하게 미리 약속된 구조를 가진 SO를 만들고,
<br>Google Spread Sheet를 Json 파일 형식으로 전달해주면 Editior기능을 활용하여 만든 기능을 통해서 정보를 직접 변경하거나 추가할 필요 없이 데이터를 수정 및 저장할 수 있게 만들었습니다. 
<br>나중에 다른 SO 추가가 필요하면, 동일하게 정해진 SO방식를 만들고 그와 동일한 형식의 Json 파일을 전달해준다면 쉽게 해당 정보의 저장 및 수정을 쉽게 구현 할 수 있습니다. 
<br>또한, 추후 자주 변경 될 밸런스적인 레벨디자인 부분은 엑셀로 컨트롤하여 쉽게 수정할수 있도록 접근성을 고려하여 코드도 설계되었습니다.

<br><br><img src = "https://github.com/user-attachments/assets/c42cde95-1507-46ee-948c-e4e1f0b267ab" width="400" height="500"> 
<img src = "https://github.com/user-attachments/assets/3195c78a-16c2-4fd5-81d8-042ca1c97d68" width="400" height="150">  
</details>

<details>
<summary><b> Upper Layer를 사용하여 효율적인 매니메이션 관리 </b></summary>
<br>플레이어의 다양한 상태와 다양한 아이템에 따른 애니메이션을 경우의 수만큼 만들지 않고, 플레이어의 상체에 아이템 장착 시 우선적으로 적용할 upper layer를 적용하여 이후에 다른 상태와 아이템이 추가 되어도 적은 비용으로 추가 할 수 있게 만들었습니다. 

<br><br><img src = "https://github.com/user-attachments/assets/242cc470-bc8d-43e4-a012-f62aec5e02a9" alt="플레이어에 적용된 upper layer" width="400" height="300">
<img src = "https://github.com/user-attachments/assets/5d4c96ad-e88e-4044-bfdf-efd82a98b8d0" width="400" height="300">
</details>

## 5. 기획
<details>
<summary><b> 브레인 스토밍 </b></summary>
<img src = "https://github.com/user-attachments/assets/c7e95460-a396-4d2d-92c5-a698a928328c" width="400" height="300">
  
#### a. 시작
- EscapeTheBackroom이라는 레퍼런스를 찾아 이 게임처럼 만들기로 결정하였습니다.
<br>호러, FPS, 3D를 통합하여 해당 게임으로 결정하였습니다.

#### b. 핵심주제
- 플레이어에게 심리적 압박 및 공포감을 선사 해야 하며 이동을 위해 문서를 찾아 퍼즐을 푸는 방식으로 교체하게 되었습니다.
</details>
<details>
<summary><b>컨텐츠 기획</b></summary>
<br><img src = "https://github.com/user-attachments/assets/27f67100-79ca-4010-ae0f-14ced3a12a02" width="400" height="300">
<img src = "https://github.com/user-attachments/assets/c43c891f-d2ac-4f0e-aee1-3c6b10aa3cda" width="400" height="300">
<br><img src = "https://github.com/user-attachments/assets/ab9fc22a-d026-468d-b66a-b8c05ba0243a" width="400" height="300">
<img src = "https://github.com/user-attachments/assets/e96586d3-0b20-47b0-89e8-fee693aad3a0" width="400" height="300">
<br><img src = "https://github.com/user-attachments/assets/d777e52d-34d1-4b54-9f2d-b4d94ad08e3a" width="400" height="300">
<img src = "https://github.com/user-attachments/assets/40b9fcf0-36c4-4ab3-b963-cfce8fb612c4" width="400" height="300">
<br><img src = "https://github.com/user-attachments/assets/643399ec-0fbc-48b4-8ac6-4a7a21af4472" width="400" height="600">
</details>

## 6. 유저테스트
<details>
<summary><b> 게임이 너무 무서워요! </b></summary>
 "공포게임이면 당연히 무서워야 하는거 아닌가?" 라고 생각하는것이 일반적이라고 생각하였습니다.
문제는 피드백을 받아야하는데 이에 방해가 될정도로 무섭다는것때문에 대부분 게임에 일부만 해보고 무서워서 끄는 사람이 너무 많아 문제가 되었습니다.
<br>UX/UI나 코드적인 문제점은 고치도록 노력 할 수 있으나, 이 점은 마이너한 게임장르 선정에 한계라 여겨 컨셉을 유지하되 조심스럽게 레벨디자인을 변경하는 방식을 채택하였습니다.
</details>
<details>
<summary><b> 문서 읽기가 너무 어려워요! </b></summary>
이게임에 스토리텔링과 키패드 퍼즐을 담당하는 부분인 문서는 SCP재단에서 영감을 받아 보고서의 형식으로 만들어졌습니다.
<br>때문에 폰트가 작고 글이 매우 길어 몬스터 추격을 따돌리며 급하게 읽기에는 부적절한 형태의 정보전달 매개채가 되었습니다.
<br>그래서 기존에 형식을 지키는 선에서 최대한 폰트를 키우고 문장을 요약하여 가독성을 올리는 방향으로 바꿨습니다.
</details>
<details>
<summary><b> 너무 안보여요 </b></summary>
게임을 고전적인 공포게임을 레퍼런스 삼아 어둡고 불편하게 만들었습니다.
<br>다만 저희 팀원들은 게임 제작자의 입장이라 맵을 훤히 꿰고 있어 이를 잘 알고 있으나, 유저 입장에서는 동일한 공간의 연속과 매우 어두운 환경으로 인하여 원할히 플레이하기 어려웠습니다.
<br>이를 수정하기 위해 손전등의 밝기를 강하게 만들고 그래피티 또는 낙서와 같은 형식의 이정표를 맵에 넣게 되었습니다.
</details>
<details>
<summary><b> 그 밖의 피드백 </b></summary>
<br>약 60여개 이상의 다양한 피드백을 수용하여 수정하는 과정을 거쳤습니다.
<br><img src = "https://github.com/user-attachments/assets/86ae1e80-6e8a-43da-a773-b8823acd1cbe" width="400" height="300">
</details>


## 7. 트러블슈팅
<details>
<summary><b> 캐릭터 컨트롤러가 아닌 Collider와 RigidBody를 선택한 이유  </b></summary>
처음에는 보다 자연스럽고 쉽게 원하는 플레이어의 이동을 구현하기 위해서 캐릭터 컨트롤러를 사용했지만,
<br>이후에 문을 열고 닫는 과정에서 문의 Collider와 충돌 시 예상과 다른 비정상적인 움직임을 보여 플레이어 컨트롤러가 아닌 Collider와 RigidBody로 변경해 주었고,
<br>이에 따른 벽이나 물체와 비벼지는 현상은 플레이어 Collider의 Material을 변경하여 해결하였습니다.
</details>

<details>
<summary><b> 플레이어의 애니메이션을 동시에 수정하면서 발생한 문제들 </b></summary>
1. 플레이어가 점프 후 Rigidbody의 Gravity의 영향을 안 받던 문제
<br>플레이어가 점프 후 하강 시 애니메이션을 update와 lateupdate에서 서로 State를 변경하여 문제가 발생했습니다.  
<br>그래서 플레이어의 statemachine 변화를 일반적으로는 update에서만 처리하고, 만약 다른 곳에서 변경하는 경우 방어 코드를 작성하여 동시에 변화시키지 못하게 함으로서 문제를 해결하였습니다.
<br><br>
2. 플레이어가 캐비넷에 들어가거나 나올 때 발소리가 비정상적으로 나오던 문제
<br>플레이어가 순간적으로 낮은 높이를 내려올때도 하강하는 로직을 타서 순간적으로 FallState와 IdleState, WalkState로 변화하면서 생긴 문제로
<br> 1. 의 방법을 적용 후, 하강을 인지하는 velocity.y의 값을 늘려 낮은 턱에서 동일하게 발생할 수 있는 문제를 해결하였습니다.
</details>

<details>
<summary><b> 오브젝트와 플레이어가 충돌해도 충돌인식이 되지 않았던 문제 </b></summary>
<br>Layer를 player로 설정하여 적용시킨 뒤, 충돌 처리 코드 조건문의 조건을 other.gameObject.layer == player 로 설정하였지만 조건문의 조건이 계속해서 충족되지 않았습니다.
<br>해당 문제의 원인은 other.gameObject.layer 는 int 값으로 계산되고 player는 비트연산자에 의해 2진수로 값이 들어오기 때문이였습니다.
<br>other.gameObject.layer값을 2진수로 변화해여 문제를 해결하였습니다.
<br><img src = "https://github.com/user-attachments/assets/0000391f-fa0f-46bc-b8a0-3cb2ae2fc884" width="400" height="200">
</details>

<details>
<summary><b> UIManager가 프리팹 내부UI를 참조하도록 설정했지만, 실제로 Hierarchy에 배치된 오브젝트를 제대로 찾지 못하는 문제 </b></summary>
해당 문제는 프리팹 내부 오브젝트를 참조하게 설정을 해놓으면 실제 인게임에선 Hierarchy에 배치된 오브젝트를 찾는것이 아니라,
<br>프리팹 내부의 오브젝트를 참조하고있어 실질적으로 작동하지 않는 오류가 있었습니다.
<br>그리하여 씬이 로드될때 해당 UI의 자식 오브젝트를 찾아 메서드 호출하는 방식으로 변경하였습니다.
<br>해당 문제는 에러 코드로도 잡히지 않고 코드가 진행되기에 주의가 필요한 부분이기에 충분히 리마인드 하게 되었습니다.
<br><img src = "https://github.com/user-attachments/assets/6bb52074-bf4f-4f1d-87fe-7332415fb94b" width="400" height="200">

</details>

<details>
<summary><b> 여러 개의 LayOut Group으로 인한 레이아웃 정렬 문제  </b></summary>
Vertical, Hoeizontal Layout Group을 하나의 UI에서 동시에 사용하게 되면 정렬에 문제가 생겨서 사용자가 예상한 모습으로 나오지 않을 수 있습니다.
<br>이런 경우에는 LayouyRebuild.ForceRebuildLayoutImmediate() 를 통해서 강제로 재정렬을 통해 문제를 해결 할 수 있습니다.
<br>그렇기에 문제가 발생하는 UI의 조건을 달아서 해당 기능을 호출해줌으로서 문제를 해결하였습니다.
<br><br><img src = "https://github.com/user-attachments/assets/b4e9de06-4ad2-4bde-b112-478b3dd0bed9" width="800" height="100">
</details>

<details>
<summary><b> 문서 스폰 형식 변경 </b></summary>
기존에 플레이어가 문서에 대한 고유 int 값을 갖고있고,
<br>문서를 먹을때마다 그 값이 올라가면서 해당하는 int 값의 문서를 UI로 보여주는 형식으로 코드가 구성되어 있었습니다.
<br><br>하지만 위 방식은, Save&Load 를 구현한 시점에서 문서는 맵에 종속되어있는 오브젝트였기에
<br>세이브 데이터와는 별개로 무조건 스폰되게 되어있었고,
<br>그에따라 저장과 불러오기를 반복하면 무한히 문서 대량습득이 가능했습니다.
<br>일명 아이템 복사 버그를 막기 위해 문서도 스폰방식을 변경하며,
<br>그에따른 ID값 부여 및 해당 ID에 대한 독립적인 문서 UI 제공으로 변경하였습니다.

<br><br><img src = "https://github.com/user-attachments/assets/109f1fa2-a9eb-4bd3-bc6f-ef68154ffaa1" width="400" height="300">
<img src = "https://github.com/user-attachments/assets/b0123dac-47c4-47d0-be95-298e5dd9fd71" width="400" height="300">
<br><img src = "https://github.com/user-attachments/assets/6450702e-4e0a-47b8-96ce-a196190b2610" width="400" height="300">
<img src = "https://github.com/user-attachments/assets/9bab203d-5da1-45a1-bd6e-3179b3e6b285" width="400" height="300">
</details>
