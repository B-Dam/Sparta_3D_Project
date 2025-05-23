프로젝트 README

개요

이 프로젝트는 Unity 3D 기반으로 만들어진 샘플 게임입니다. 주요 기능은 다음과 같습니다:

플레이어 이동 & 점프: Input System으로 WASD/화살표 이동 및 Space 점프 구현

체력 시스템: HealthSystem과 UI를 연계한 실시간 체력바

데미지 피드백: 캔버스 오버레이를 이용한 화면 깜빡임 연출

환경 상호작용: Raycast & Interaction(F 키)으로 오브젝트 조사 및 상호작용

아이템 & 인벤토리:

ItemData ScriptableObject로 데이터 관리

Inventory 싱글톤으로 획득·사용·버리기

InventoryUI로 슬롯 기반 UI 표시

점프대 구현: JumpPad 스크립트 부착된 오브젝트 밟으면 높이 점프

주요 스크립트 구조

스크립트

설명

PlayerController

3D 이동, 점프, 카메라 회전, 커서 잠금/해제

HealthSystem

체력 관리, OnHealthChanged 이벤트로 UI 갱신

ImageIndicator

데미지 시 화면 붉은 깜빡임

InteractableObject

조사용 & 아이템 획득 처리 (IInteractable 구현)

ObjectInteractor

Raycast 조사, Interaction 액션(F 키) 연결, UI 토글

Inventory

싱글톤 인벤토리: Add/Use/Remove 메서드

InventoryUI

인벤토리 UI 생성·갱신, 슬롯 클릭 시 상세보기

JumpPad

트리거로 점프 임펄스 적용

사용법

실행: Play 모드 진입

이동/점프: WASD/화살표 키로 이동, Space로 점프

조사: F 키로 조사 가능한 오브젝트 상호작용

아이템 획득: 조사 대상에 itemData 할당 시 한 개씩 인벤토리에 추가

인벤토리: Tab 키로 인벤토리 열고, Use/Drop 버튼으로 아이템 사용 및 버리기

점프대: JumpPad가 붙은 오브젝트 밟으면 자동 점프

Unity 버전: 2022.3.17f1 이상

필수 패키지: Input System, TextMeshPro

작성일: 2025-05-23
