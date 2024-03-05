# The_Dokdo
 
## 🎉프로젝트 소개
Visual Studio와 Unity 를 이용해서 C#으로 제작한 프로그램입니다.

## 게임 컨셉
The_Dokdo : 대한민국을 지키는 자랑스러운 해군인 주인공,
어느날 자신의 함대가 침몰하였고, 일어나보니 
모르는 섬에 도착했는데,
그 섬이 조선시대의 독도였다. 식인종과 동물들로부터 살아남기

<br>


## 📅개발 기간
* 2024년 2월 27일 ~ 2024년 3월 05일

### 멤버구성
* 김혜림(타칭 갓팀장) - 데이터( 로드 및 저장), 문서정리 , PM
* 박지훈b - UI 
* 차동민 - Monster
* 진보경 - Player
* 이재헌 - Inventory, Item


### 🏟️개발 환경
* Visual Studio 2022
* C#
* .NET 8.0
* Windows
* NuGet 패키지
* Unity

## 주요기능
* 구현기능 - 게임 맵 생성 및 배치, 플레이어 생성 , 적의 움직임과 스폰, 몬스터 공격 로직 , 적의 체력 및 공격력, 자원관리, 
* 게임 진행 상태 표시, 게임 저장 및 불러오기 다양한 몬스터 유형, 재화시스템, 그래픽 향상, 레벨 디자인

* Base : 우리 에셋 정말 너무 너무 이뻐요 :)
  
## CS 구성 
  
### Data 
* InfoManager : GameData 관리 ,GameLoad, Player,Item,Monster Data 
* GameManager : GameManager
* DataManager : GameData 관리
- GameSave
- Player,Item,Monster Data 
* StartSceneManager : Start 메소드에서 InfoManager 객체를 로드하고 인스턴스화하는 작업을 수행


### Monster
* Bear : 곰의 ai, 스태이터스, 애니메이션 관리
* Native : 원주민의 ai, 스테이터스, 애니메이션 관리
* Rabbit : 토끼의 ai, 스테이터스, 애니메이션 관리
* Zombie : 좀비의 ai, 스테이터스, 애니메이션 관리
* MonsterManager :  토끼와 곰 같은 동물형 몬스터의 랜덤 스폰 및 생성 주기 관리
* MonsterData : 각각의 몬스터 클래스에서 현재 위치값과 회전값과 체력정보를 인스턴스화 해주는 틀과같은 역할.
* MonsterDataManager : 몬스터가 생성되고 삭제될때 중앙에서 몬스터의 정보를 등록하고 해제하는 역할

### <Inventory>
* Inventory : 인벤토리 버튼 관리
* ItemSlotUI : 아이템슬롯 관리

### <Player>
* AnimatorController : 플레이어의 애니메이션 관리
* DamageIndicator : 플레이어가 데미지를 받을때 발생하는 피격 효과 관리
* PlayerConditions : 플레이어의 컨디션 관리
* PlayerController : 플레이어 이동 및 점프 관리
* PlayerManager : 플레이어의 데이터 관리


-----------------------------------------------------------------------------

## <U.I>
* DayNightCycle : 맵의 밤 낮구현
* DungeonSceneLoad1Zone : 1구역 던전 입장
* DungeonSceneLoad2Zone : 2구역 던전 입장
* DungeonSceneLoad3Zone : 3구역 던전 입장
* EditorScripting : 가상벽 이미지화
* FrameSetting : 게임 프레임 60으로 설정
* LoadingSceneManager :  게임 씬 전환 시 게임 로딩화면 불러오기
* MainSceneLoad  : 메인 화면 불러오기
* OptionUI : 인게임 esc 누르면 나오는 옵션 창
* UseShop : 상인npc 랑 거래
* PurchaseInfoAlarm :  상인npc 거래 알림 설정
* StartSceneLoad : 게임 시작 화면 불러오기
* StartSceneUI : 게임 시작 화면에있는 ui설정


### 장르 : 3D 공포 스릴러 서바이벌

### 스토리 요약 :
대한민국을 지키는 자랑스러운 해군인 주인공,
어느날 자신의 함대가 침몰하였고, 일어나보니 
모르는 섬에 도착했는데,
그 섬이 조선시대의 독도였다. 식인종과 동물들로부터 살아남기

### 연출 : 
* 3d를 이용한 맵 구현 및 아이템, 던전 등을 이용한 서바이벌

### 몬스터 : 
* 동물
* 거칠 조(粗), 소리 음(音),슬플 비(悲) (좀비)
* 원주민(식인종)

### 플레이 방식 :
1인칭 시점으로 플레이어가
무기와 음식 등을 이용하여
알 수 없는 이 섬에서
식인종과 동물로부터 살아남기

## 자세한  정보는 팀 노션에서 

