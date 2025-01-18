# Shhh... (쉿!)

## 1. 개요
- 장르 : 3D, FPS, 호러, 퍼즐, 백룸
- 플랫폼 : PC (Window)
- 배포 방식 : itch.io
- 개발 기간 : 2개월(2024.11.25 ~ 2025.01.21 : 기획 기간 포함)
- 개발 인원 : 4명 (박현도, 이지성, 이훈, 정용화) + 1명(외부 애니메이터)
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
#### 인게임 기능

<details>
<summary> Json을 활용한 데이터 저장 및 로드  </summary>  
<br>Save 버튼 클릭 시 각 저장하여야 하는 컴포넌트별로  하이어라키를 전체 탐색하여 저장될 객체를 찾습니다. 
<br>저장 할 해당 프리팹의 고유 키, 이름, 포지션 등을 딕셔너리로 저장하고 그 딕셔너리를 JsonData로 변환하여 고유의 파일로 저장합니다.  
<br>Load 버튼 클릭 시 해당 씬으로 변경되게 되며, Json으로 저장되어있는 파일을 딕셔너리로 변환 후 맵, 플레이어, 아이템, 적 등 순으로 프리팹을 생성하여 맵에 배치합니다. 
<br> 해당 프리팹에 변경점이 필요한 경우 Instantiate 시에 해당 컴포넌트의 값을 변경하여 생성합니다.
</details>

<details>
<summary> Input System  </summary>
<br>Input System 의 구독과 해제 기능을 사용하여 다른 스크립트들이 PlayerController 스크립트에 달린 Input system에 구독을 하는 방식입니다.
<br>한번의 입력으로 여러 수행이 가능하면서도, 특정 행동에서는 사용자가 예상 가능한 수행만 가능하도록 하였고, 
<br>플레이어가 존재하지 않는 등의 특수한 경우, 스스로 input system을 선언하여 사용하고 삭제하는 방식을 사용하였습니다.

  <br>( Shift를 누르면 statemachine을 변경하면서, 다른 스크립트의 값도 변경 // 인벤토리를 이용 중이거나 키패드와 상호작용 중일때 아이템의 사용이 불가능하게 만듦 )
</details>

<details>
<summary> CinemachineCamera를 활용한 카메라 이동, 연출 , Postprocesisng을 이용한 화면 효과 </summary>
<br>CinemachineCamera를 활용하여 priority를 다르게 주는 등의 방식으로, 처음 게임을 플레이할 때 나오는 인트로나 죽을 때 나오는 점프스퀘어 등의 연출을 줍니다.
<br>또한, 현재 버전의 CinemachineCamera 에서는 Postprocesisng 적용 방식이 최신 버전과는 다르기 때문에 volume을 통해서 어안렌즈 등의 원하는 카메라 효과를 넣어주고 스테이지마다 다른 분위기를 연출하였습니다.
</details>

<details>
<summary> FSM 구조의 플레이어와 몬스터들  </summary>
<br>FSM은 플레이어와 몬스터들은 상태(State)와 전이(Transition)를 기반으로 동작합니다. 
<br>유한한 상태 집합에서 하나의 상태만 활성 상태로 유지되며, 특정 이벤트에 따라 상태가 전이됩니다. 
<br>상태의 변화는 특정 조건에서만 이루어지기 때문에 버그 발생의 여지가 적고, 이후에 플레이어나 몬스터에게 새로운 상태가 추가되더라도 쉽게 유지보수가 가능합니다.
    <br>  <br>
<img src = "https://github.com/user-attachments/assets/d9698a27-66f7-43ef-9c97-ad5b4ea839ed"> 
</details>

<details>
<summary> 확장성을 고려하여 제작된 다양한 객체 </summary>
<br>Items, Enemy, InteractableObjects 등 비슷한 분류로 나누어진 각 객체들은 Interface 또는 부모스크립트 Base 를 상속받아 기능의 독립성을 유지하되, 각 필요한 공통기능을 부여받고 있습니다.  
<br>또한 , 각 객체들이 공통의 부모로부터 상속을 받는경우, 검출이나 비교 등 이 코드적으로 간편해질수 있도록 고려하여 설계하였습니다.

</details>

<details>
<summary> 게임성을 풍부하게 해주는 다양한 코드의 퍼즐들 </summary>
<br>&nbsp; 키패드 퍼즐의 경우, Physics Raycaster 와 Event Trigger 를 활용한 인게임 3D Object 클릭 시스템을 구현하여 키조작을 구현하였습니다.
<br>Interact시 LED부분이 빛 날 수있도록 EMISSION을 컨트롤하거나 코루틴을 추가하여 깜빡이는 텍스트 연출을 추가하였습니다. 
<br>&nbsp; 락픽 퍼즐의 경우, 마우스의 Delta값을 받아 락픽의 각도를 조정합니다. 
<br>마우스 클릭시 유저의 락픽 각도와 정답 각도의 오차를 계산하여 오차가 많이 날 경우 많은 떨림과 함께 락픽이 부러지게 되며,  
<br>오차가 많이 나지 않을 경우 그 값을 정규화 하여 차이가 나는 정도에 따라 문고리 돌아가는 정도를 다르게 주어 유저가 어느정도 정답에 근접한지 식별하기 용이하도록 구현하였습니다.
</details>

#### 게임 외의 기능

  
<details>
<summary> Singleton 구조를 상속받는 매니져들 </summary>  
<br>위의 확장성을 고려한 객체 설계 중 하나로, 기본적인 싱글톤 구조를 만들고 모든 매니저들이 이를 상속받아 필요에 따라서 쉽게 접근이 가능합니다. 
<br>또한, 각 스크립트에서 추가적인 선언이 필요하지 않아 메모리 관리 효율 증가도 기대 할 수 있습니다. 싱글톤을 상속받는 매니져들은 DontDestroyOnLoad 를 활용하여, 각 객체를 유지하고 유지보수를 고려하였습니다.
<br><br>(GameManger, SoundManger 등)
</details>

<details>
<summary> Json 파일과 Editor 기능을 활용하여 SO(스크립터블 오브젝트) 구현 </summary>
<br>기획자나 개발자가 추후에 아이템을 추가하거나 변경하기 편하게 미리 약속된 구조를 가진 SO를 만들고,
<br>Google Spread Sheet를 Json 파일 형식으로 전달해주면 Editior기능을 활용하여 만든 기능을 통해서 정보를 직접 변경하거나 추가할 필요 없이 데이터를 수정 및 저장할 수 있게 만들었습니다. 
<br>나중에 다른 SO 추가가 필요하면, 동일하게 정해진 SO방식를 만들고 그와 동일한 형식의 Json 파일을 전달해준다면 쉽게 해당 정보의 저장 및 수정을 쉽게 구현 할 수 있습니다. 
<br>또한, 추후 자주 변경 될 밸런스적인 레벨디자인 부분은 엑셀로 컨트롤하여 쉽게 수정할수 있도록 접근성을 고려하여 코드도 설계되었습니다.
    <br>  <br>
<img src = "https://github.com/user-attachments/assets/3195c78a-16c2-4fd5-81d8-042ca1c97d68">  
</details>

<details>
<summary> Upper Layer를 사용하여 효율적인 매니메이션 관리 </summary>
<br>플레이어의 다양한 상태와 다양한 아이템에 따른 애니메이션을 경우의 수만큼 만들지 않고, 플레이어의 상체에 아이템 장착 시 우선적으로 적용할 upper layer를 적용하여 이후에 다른 상태와 아이템이 추가 되어도 적은 비용으로 추가 할 수 있게 만들었습니다. 
</details>
