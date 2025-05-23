# 🎮 Unity 3D Sample Game Project

Unity 3D 기반으로 제작된 샘플 게임 프로젝트입니다.  
플레이어 조작, 체력 시스템, 인터랙션, 인벤토리 등 다양한 핵심 기능이 포함되어 있습니다.

---

## 📌 주요 기능

- **플레이어 이동 & 점프**  
  Input System 기반으로 WASD/화살표 키 이동 및 Space로 점프

- **체력 시스템**  
  실시간 체력 UI 연동 (HealthSystem + UI 바인딩)

- **데미지 피드백**  
  화면 깜빡임 효과로 피격 연출 제공 (ImageIndicator)

- **환경 상호작용**  
  Raycast + F 키로 오브젝트 조사 및 상호작용 가능

- **아이템 & 인벤토리**  
  - ScriptableObject 기반 아이템 데이터 관리  
  - 싱글톤 인벤토리 시스템 (Add/Use/Drop 지원)  
  - 슬롯 기반 인벤토리 UI 구성

- **점프대 (JumpPad)**  
  오브젝트 위에 올라가면 자동으로 높게 점프하는 기능

---

## 🧩 스크립트 구조

| 스크립트명         | 설명 |
|-------------------|------|
| `PlayerController` | 3D 이동, 점프, 카메라 회전, 커서 잠금/해제 처리 |
| `HealthSystem`     | 체력 계산 및 이벤트로 UI 갱신 |
| `ImageIndicator`   | 데미지 시 붉은 화면 깜빡임 효과 |
| `InteractableObject` | IInteractable 구현, 조사 및 아이템 획득 처리 |
| `ObjectInteractor` | Raycast 조사 및 F 키 입력 처리 |
| `Inventory`        | 싱글톤 인벤토리 관리 클래스 |
| `InventoryUI`      | 인벤토리 UI 생성 및 업데이트 |
| `JumpPad`          | 트리거 접촉 시 점프 임펄스 적용 |

---

## 🕹 사용 방법

- **실행**: Unity에서 Play 모드 진입
- **이동/점프**: WASD 또는 화살표 키로 이동, Space로 점프
- **조사/상호작용**: F 키로 인터랙션
- **아이템 획득**: `itemData` 지정된 오브젝트 조사 시 인벤토리에 자동 추가
- **인벤토리 열기**: Tab 키  
  → 아이템 클릭 시 사용 / 버리기 가능
- **점프대 사용**: JumpPad 오브젝트 위에 올라서면 자동 점프

---

## ⚙️ 개발 정보

- **Unity 버전**: `2022.3.17f1` 이상
- **필수 패키지**:
  - `Input System`
  - `TextMeshPro`

---

## 📅 작성일

2025년 5월 23일

---

💡 더 많은 Unity 튜토리얼 및 샘플 자료는 [GPTOnline.ai](https://gptonline.ai/ko/)에서 확인하세요!
