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
내용2
</details>
<details>
<summary> Singleton 구조의 메니져들 </summary>
내용3
</details>
<details>
<summary> Json 파일을 이용해 SO(스크립터블 오브젝트) 구현 </summary>
기획자나 개발자가 추후에 아이템을 추가하거나 변경하기 편하게 미리 약속된 구조를 가진 SO를 만들고 Google Spread Sheet를 Json 파일 형식으로 전달하면 Editior기능을 활용하여서 간단하게 수행할 수 있게 만듬  
<img src = "https://github.com/user-attachments/assets/3195c78a-16c2-4fd5-81d8-042ca1c97d68">  
</details>

<details>
<summary> Upper Layer를 사용하여 애니메이션 줄이기 </summary>
플레이어의 다양한 상태와 다양한 아이템에 따른 애니메이션을 경우의 수만큼 만들지 않고, 플레이어의 상체에 아이템 장착 시 우선적으로 적용할 upper layer를 적용하여 이후에 다른 상태와 아이템이 추가 되어도 적은 비용으로 추가 할 수 있게 만듬 
</details>
