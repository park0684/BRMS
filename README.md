

## 📌프로젝트명 : BRMS

> 프로젝트 목적 :
> 
> 제품 판매를 위한 제품, 공급사, 회원 정보와 제폼의 매입, 판매 내역을 관리가 필요하며,<br>
> 제품명의 한글/영문과 판매가의 한화/달러 병행 표기 등 일부 추가적인 정보 표시 필요로 인해 자체적인 관리 시스템 구축이 요구.<br>
> 이에 요청사항에 대한 정보 입력이 가능한 별도 Back office용 프로그램 제작을 위해 프로젝트를 진행.

## 📌 제작 언어

<h3 align="left">Languages and Tools:</h3>
<p align="left"><a href="https://www.w3schools.com/cs/" target="_blank" rel="noreferrer"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" alt="csharp" width="40" height="40"/> </a> <a href="https://www.microsoft.com/en-us/sql-server" target="_blank" rel="noreferrer"> <img src="https://www.svgrepo.com/show/303229/microsoft-sql-server-logo.svg" alt="mssql" width="40" height="40"/> </a> </p>

> C# WinForm
> MSSQL
> 
> 
> <div style="margin-left: 20px;">
<details>
<summary>셈플데이터 지원 :</summary>

>DB 폴더내 BRMS_Sapmple
</details>

## 📌 주요기능

### 제품, 공급사, 매입, 판매, 회원의 정보를 중심으로 각종 정보 등록 및 조회 가능

### 1.기초관리
<div style="margin-left: 20px;">
<details>
<summary>제품관리</summary>

> - 제품명, 제품코드를 검색어로 조회도 가능하지만 제품 등록 및 기간 조건을 지정하여 조회가 가능합니다<br/>
    ![Image](https://github.com/user-attachments/assets/ae8e3119-fa61-4727-8bbb-3d664b747078)
> - 좌측 상단의 분류를 지정할 경우 지정된 분류의 상품들만 조회가 가능합니다<br/>
> - 조회된 제품을 더블클릭 하면 선택된 제품의 제품코드, 제품명, 공급사, 분류, 매입가, 판매가 설정이 가능합니다.<br/>
판매가는 설정된 환율 연동으로 자동 한화 또는 달러로 전환됩니다.<br/>
![Image](https://github.com/user-attachments/assets/c09796de-1b1f-4421-97be-ad2f0e964932)
> - 새로운 제품을 등록 할 경우 제품코드의 중복 여부를 확인 후 등록 합니다.
![Image](https://github.com/user-attachments/assets/50ed954d-22d4-48e1-8bab-67880159696d)
> - 새 제품 등록 시 공급사와 분류 지정은 필수 입니다. 
> - 제품등록정보에서는 제품의 정보뿐 아니라 매입, 판매, 변경 로그 조회가 가능합니다.
![Image](https://github.com/user-attachments/assets/65e59257-9171-46f5-83de-7a10e65960ee)
</details>
</div>

<details>
<summary>분류</summary>
  
> - 제품의 분류 지정을 위한 분류 정보를 추가 수정 할 수 있으며<br/>
> 제품 정보 및 판매 조회 시 분류를 지정하사용 가능
<div>
  <img src="https://github.com/user-attachments/assets/2319a7bc-1d64-458b-9874-cbd866a253de" alt="Image 1" width="200"/>
  <img src="https://github.com/user-attachments/assets/8d4cb6c9-aece-4e23-b773-71f052d42a0f" alt="Image 2" width="200"/>
  <img src="https://github.com/user-attachments/assets/2c63cba8-ec8b-487a-a997-40e8781df3c7" alt="Image 3" width="200"/>
</div>
</details>

<details>
<summary>직원관리</summary>

> - 업무를 위한 직원을 등록하거나 정보를 수정 할 수 있는 메뉴입니다.
> - 직원 등록 및 권한 부여를 통해 업무에 따른 정보 접근 및 제어를 제한 할 수 있습니다.<br/>
> - 직원 권한은 조회와 등록/수정, 출력, 엑셀저장 등 4가지 권한을 각 메뉴마다 지정이 가능합니다.
> - ![Image](https://github.com/user-attachments/assets/908c058e-50f3-4989-9420-c85635c40fc1)
> - 직원코드는 채번코드로 별도 지정하지 못하고 자동으로 등록 순서에 따라 부여됩니다.
> - 직원 비밀번호는 암호화 처리 되며, 분실시 확인은 불가능하므로 수정을 통해 초기화 할 수 있습니다.
</details>

<details>
<summary>직원접속로그</summary> 

> - 정보 조회 또는 수정 시 시간과 행위 내용을 조회 할 수 있는 메뉴입니다.
> - 조회 메뉴에서 전체 조회 등의 경우 상세정보를 확인 할 수 없으나<br/>
> - 특정 제품, 회원, 거래, 매입 등의 채번코드 확인이 가능한 상세내역을 조회 할 경우<br/> 그 조회 대상도 확인이 가능합니다.
</details>

### 2.공급사 
<details>
<summary>공급사 관리</summary>

> - 거래중인 공급사 정보를 등록하는 메뉴로,<br/>
> - 매입, 발주, 결제시 공급사 정보 연동 필요로 필수 등록 항목 입니다.
</details>

<details>
<summary>매입</summary>

> - 각 공급사별로 매입 또는 반품으로 전표 등록이 가능하며,<br/>
> - 등록된 매입 제품은 각 제품의 제고에 즉시 반영 됩니다<br/>
> - 매입금액은 제품의 매입단가에 반영이 우선이나 개별로 변경하여 입력이 가능하며,<br/>
> - 합산된 매입액은 해당 공급사의 미수잔액에 반영이 됩니다.
> - ![Image](https://github.com/user-attachments/assets/c683c666-6da0-4b90-9260-9f4ea3dfc226)
</details>
<details>
<summary>발주</summary>

> - 각 공급사별로 매입 또는 반품으로 전표 등록이 가능하며,<br/> 
> - 등록된 매입 제품은 각 재고 및 공급사 미수 잔액에 영향을 주지 않습니다.
> - 입고완료 등 상태를 변경 할 수 있습니다.
</details>

<details>
<summary>결제관리</summary>

> - 각 공급사별로 미수잔액 조회 및 결제 등록하는 메뉴입니다<br/>
> - 결제유형은 현금, 계좌이체, 카드, 어음이 있으며<br/>
> - 할인 및 쿠폰과 같은 공급사의 지원 사항등도 반영이 가능합니다
> - ![Image](https://github.com/user-attachments/assets/709d868f-bf6e-4ef9-8bda-400fa9ac3883)
</details>

### 3.판매관리
<details>
<summary>판매현황</summary>

> - 제품 판매 현황을 공급사별, 분류별, 제품별, 일자별로 조회 할 수 있는 메뉴입니다.<br/>
> - 판매된 수량과 금액이 합산되어 표시되며, 이익은 판매분에 대한 이익율로 계산됩니다.<br/>
> - 공급사별, 분류별, 제품별은 일자별 조회 체크 시 각항목이 일자별로 구분되어 조회가 됩니다.
> - ![Image](https://github.com/user-attachments/assets/8cc195d2-5696-460d-a334-aef0aef3afca)
</details>

<details>
<summary>판매내역</summary>

> - 판매 등록한 거래건을 조회를 할 수 있는 메뉴로<br/>
> - 각 거래금액에 대해 상세히 조회 가능 합니다.<br/>
> ![Image](https://github.com/user-attachments/assets/aa9e8a0c-5c26-409d-84dd-fb60199a31b1)
> - 거래건 선택 시 거래 상세내역에서 제품 및 수량, 결제, 배송지 등의 상세 정보 확인 가능 합니다.<br/>
![Image](https://github.com/user-attachments/assets/8aaefa4d-5a3e-4f0c-b03f-b8102c124bce)
> - 거래 등록 버튼 클릭 시 판매등록이 가능합니다.<br/>
>   >판매등록<br/>    
>   > - 판매 등록은 회원 지정과 비회원으로 등록이 가능하며, <br/>회원이 선택된 상태에서 배송지 등록시 회원의 주소가 기본으로 설정되나 수정도 가능합니다<br/>
>   > - 결제는 현금,카드,계좌이체,포인트로 가능 하나<br/>카드 결제의 경우 실제 카드 승인이 아닌 카드 매출로 기록 합니다.<br/>
>   > - 판매등록 시 입력된 제품의 수량 만큼 재고에 즉시 반영됩니다.
>   >   ![Image](https://github.com/user-attachments/assets/eb9196b7-feab-41c9-bf82-6aa2c97a19c7)
>   
> - 판매내역은 수정이 불가능하나, 거래건을 조회 후 반품처리를 할 수 있습니다.<br/>
> - 반품은 기존 거래건을 삭제하는 것이 아닌 새로운 반품 거래건을 등록 합니다.
</details>

<details>
<summary>주문서</summary>

> - 메일 등 비대변 견적 또는 주문의 경우 주문서 등록을 통해 접수시 사용 합니다.
> - 주문수량과 판매 가능 수량을 별도로 입력 할 수 있으며,<br/>
> 상황에 따라 판매 금액이 달라질 수 있으로 제안액이라는 부분으로 실제 판매 금액을 조정 처리 할 수 있습니다.<br/>
![Image](https://github.com/user-attachments/assets/3d701b26-a881-42e4-b3be-0b8ed79a68a6)
> - 주문,판매,취소 선택을 통해 주문서 상태를 변경 할 수 있으며,<br/>
> 판매 상태로 등록 할 경우 제안 수량과 금액으로 판매 등록 메뉴를 즉시 실행하여 등록 할 수 있습니다<br/>
![Image](https://github.com/user-attachments/assets/8198eac9-51bf-4e79-963a-52a537045155)
</details>

<details>
<summary>일결산</summary>

> - 제품의 판매, 매입, 재고 정보를 일별로 기록하여,<br/> 이후 상품 정보가 수정되어도 일정산 시점의 정보로 매출 및 수불 등 현황 조회가 가능합나다.<br/><br/>
![Image](https://github.com/user-attachments/assets/5c90a311-063c-4b9f-af1a-9efd878d1b60)
>   > 주요 항목 설명
>   > - 기초재고 : 해당일자 시작시 보유하고 시작하는 재고입니다.<br/> 기초 재고액은 기초량 X 매입단가를 기준으로 작성됩니다.<br/>
>   > - 기말재고 : 해당일자 종료시 보유하고 이는 재고입니다.<br/> 생성시점의 현재고를 기준으로 역산하여 재고량을 계산하며 기말량 X 매입단가를 기준으로 작성됩니다.<br/>
>   > - 장부상재고 : 기초재고 + 매입 - 판매 수량으로<br/> 실제로 보유 하고 있어야 할 재고입니다.<br/>
>   > - 재고로스 : 기말재고와 장부상재고액이 차이가 발생 할 경우 표시됩니다.<br/>
>   > - 매출원가 : 이익율 계산을 위한 원가 입니다.<br/> 최종매입 원가법을 적용하여 기초재고 + 매입량 - 기말재고로 계산됩니다.<br/>
> - 매입,판매 정보를 직접 조회 하는 것이 아니므로 서버부하를 최소화 할 수 있으나,<br/> 제품별의 경우 분류 또는 제품이 지정되지 않을 경우 조회 시간이 오래 걸릴 수 있습니다.<br/>
> - 일정산은 현재날짜를 기준으로 전일까지만 작성 가능하며,<br/> 이미 작성한 날짜로 매입,판매 변경 사항이 있을 경우 재생성 대상이 됩니다.<br/>
> - 재고의 경우 직전날짜의 기말재고를 반영하나,<br/> 이전 날짜의 결산 정보가 없다면 현재고를 기준으로 매입과 판매분을 역산하여 생성합니다.<br/>
> - 일결산은 일자별, 제품별, 분류별로 각 다른 메뉴에서 조회가 가능합니다.
</details>

### 4.회원관리

<details>
<summary>회원</summary>

> - 회원을 등록하거나 상세정보를 조회 할 수 있는 메뉴입니다.
> - 검색 또는 등록/수정일자, 판매일자 지정을 통해 조회 할 수 있습니다.
> - 회원의 정보중 전화번호는 개인정보보호를 위해 암호화 처리되었으며<br/> 일괄 조회시에는 전화번호 가운데 2자리가 **로 표시됩니다.<br/>
> ![Image](https://github.com/user-attachments/assets/b52aaaf7-ac11-4e36-8d5b-971d83271495)
> - 회원 더블 클릭을 통해 상세정보를 조회 할 수 있으며, 이때 전화번호는 복호화되어 전화번호 모두 조회가 가능합니다.
> - 회원 정보 상세내역에서 주문과 거래건을 조회 할 수 있는 탭이 있어 개별 조회가 가능합니다.
> ![Image](https://github.com/user-attachments/assets/da1ea8d0-7d80-409c-afea-a0a7efc4a854)
> - 국가 지정은 D/B에 기록되어 있는 정보를 기준으로 반영되며, 새 회원등록 시 기본적으로 대한민국으로 지정되어 있어 별도 수정이 필요합니다.
</details>
<details>
<summary>포인트 내역 </summary>

> - 회원의 포인트 적립, 사용 정보를 일괄 조회 하는 메뉴입니다.
> - 판매 또는 반품 거래시 적립과 사용된 포인트를 따로 표시하며,<br/>
> - 회원정보와 거래 선택을 하여 조회 할 수 있습니다.
> ![Image](https://github.com/user-attachments/assets/f07c1b16-4a50-45ac-bf07-9aa36ad863ec)
</details>
