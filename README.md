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
  - Zira
  - Figma
- 개발 도구
  - C#
  - Unity (2022.3.17f1 ver)
  - Visual studio
  - Json
## 4. 사용한 기술
<details>
<summary> Singleton 구조의 매니져들 </summary>
내용1
</details>

<details>
<summary> FSM 구조의 플레이어와 몬스터들  </summary>
상태(State)와 전이(Transition)를 기반으로 동작합니다. FSM은 유한한 상태 집합에서 하나의 상태만 활성 상태로 유지되며, 특정 이벤트에 따라 상태가 전이됩니다.
<img src = "https://github.com/user-attachments/assets/d9698a27-66f7-43ef-9c97-ad5b4ea839ed"> 
</details>

<details>
<summary> Input System  </summary>
Input System 의 구독과 구독 해제 기능을 사용하여서 한번의 입력으로 여러 수행이 가능하면서도, 특정 행동에서는 사용자가 예상 가능한 수행만 가능하도록 변경하였습니다.   

  ( Shift를 누르면 statemachine을 변경하면서, 다른 스크립트의 값도 변경 // 인벤토리를 이용 중이거나 키패드와 상호작용 중일때 아이템의 사용이 불가능하게 만듬 )
</details>

<details>
<summary> CinemachineCamera를 활용한 카메라 이동, 연출 , Postprocesisng을 이용한 화면 효과 </summary>
CinemachineCamera를 활용하여서 특정 상황에 priority를 다르게 주는 등의 방식으로 연출을 주고, 현재 버전의 CinemachineCamera에서는 Postprocesisng 적용 방식이 최신 버전과는 다르기 때문에 volume을 통해서 원하는 카메라 효과를 넣어주고 스테이지마다 다른 분위기를 연출하였습니다.
</details>

<details>
<summary> Json 파일과 Editor 기능을 활용하여 SO(스크립터블 오브젝트) 구현 </summary>
기획자나 개발자가 추후에 아이템을 추가하거나 변경하기 편하게 미리 약속된 구조를 가진 SO를 만들고 Google Spread Sheet를 Json 파일 형식으로 전달하면 Editior기능을 활용하여서 간단하게 수행할 수 있게 만듬  
<img src = "https://github.com/user-attachments/assets/3195c78a-16c2-4fd5-81d8-042ca1c97d68">  
</details>

<details>
<summary> Upper Layer를 사용하여 애니메이션 줄이기 </summary>
플레이어의 다양한 상태와 다양한 아이템에 따른 애니메이션을 경우의 수만큼 만들지 않고, 플레이어의 상체에 아이템 장착 시 우선적으로 적용할 upper layer를 적용하여 이후에 다른 상태와 아이템이 추가 되어도 적은 비용으로 추가 할 수 있게 만듬 
</details>
