# Monkey 콘솔 애플리케이션 구현

요구사항:

- 사용 가능한 모든 원숭이를 나열하고, 이름으로 특정 원숭이의 세부 정보를 가져오고, 무작위 원숭이를 선택할 수 있는 C# 콘솔 앱을 만드세요.
- 앱은 Monkey 모델 클래스를 사용하고 시각적 매력을 위해 ASCII 아트를 포함해야 합니다.

세부 구현 가이드:

1. 프로젝트 구조
   - `MyMonkeyApp` 콘솔 프로젝트 안에 `Models/Monkey.cs`, `Services/MonkeyRepository.cs`, `Program.cs`를 생성합니다.
   - `Monkey` 모델은 `Name`, `Location`, `Population` 속성을 가집니다.

2. 기능 요구사항
   - `GetMonkeys()` : 모든 원숭이 목록을 반환합니다.
   - `GetMonkeyByName(string name)` : 이름으로 원숭이를 조회합니다.
   - `GetRandomMonkey()` : 목록에서 무작위 원숭이를 선택합니다.

3. UI 요구사항
   - 메뉴 기반 콘솔 UI (예: 1) List, 2) Details, 3) Random, 4) Exit)
   - 원숭이 정보 출력 시 간단한 ASCII 아트(원숭이) 포함

4. 체크리스트
   - [ ] `Models/Monkey.cs` 생성
   - [ ] `Services/MonkeyRepository.cs` 생성 및 구현 (GetMonkeys, GetMonkeyByName, GetRandomMonkey)
   - [ ] `Program.cs`에 메뉴 UI 구현
   - [ ] ASCII 아트 포함
   - [ ] 간단한 README 업데이트
   - [ ] 단위 테스트(선택적)

추가 메모:
- 레이블: enhancement, good first issue
- 예상 소요 시간: 1-2 시간 (간단 구현)

