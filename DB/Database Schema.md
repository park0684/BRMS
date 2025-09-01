# 📘 Database Schema
---
## ✍ 다이어그램
<details> <summary> 상세보기 </summary>
</details>

## 🧱 데이터베이스 테이블 목록

<details><summary> 상세보기 </summary>
    
  <details><summary>📄 <strong>`accesslog` 테이블</strong>
  
  > 직원 접속 로그</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 |
  |---|---|---|---|---|---|
  | acslog_type | int | ❌ | | | 로그 유형 |
  | acslog_emp | int | ❌ | | FK → empoyee | 접속 직원 |
  | acslog_param | int | ❌ | | | 대상 파라미터 |
  | acslog_date | int | ❌ | | | 접속 시간 |
  ---
  </details>
  
  <details><summary>📄 <strong>`accpermission` 테이블</strong>
  
  > 직원 권한 설정</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 |
  |---|---|---|---|---|---|
  | acper_emp | int | ✅ | FK | → employee | 직원코드 |
  | acper_permission | int | ✅ | | | 권한 유형 |
  | acper_status | int | ✅ | | | 상태 |
  | acper_idate | datetime | ❌ | | | 등록 일자 |
  | acper_udate | datetime | ❌ | | | 수정 일자 |
  ---
  </details>
   
  <details><summary>📄 <strong>`category` 테이블</strong>
  
  > 제품 분류
  </summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 |
  |---|---|---|---|---|---|
  | cat_code | int | ❌ | | PK | 분류코드 |
  | cat_top | int | ❌ | | | 대분류 번호 |
  | cat_mid | int | ❌ | | | 중분류 번호 |
  | cat_bot | int | ❌ | | | 소분류 번호 |
  | cat_name_kr | int | ❌ | | | 분류명(한글) |
  | cat_name_en | int | ❌ | | | 분류명(영문) |
  | cat_idate | int | ❌ | | | 등록 일자 |
  | cat_udate | int | ✅ | | | 수정 일자 |
  | cat_status | int | ✅ | | | 상태 |
  ---
  </details>
  </br>
  <details><summary>📄 <strong>`closingbalance` 테이블</strong>
  
  > 전기이월 잔액</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | --- |
  | cb_sup | int | ❌ | **PK** | | 공급사 코드 | 복합 PK (cb_sup, cb_date) |
  | cb_date | char(6) | ❌ | **PK** | | 기준 년월(YYYYMM) | 복합 PK |
  | cb_balance | int | ✅ | | | 잔액 | |
  | cb_idate | datetime | ✅ | | | 생성 일시 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`config` 테이블</strong>
  
  > 환경설정 정보</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | --- |
  | cf_code | int | ❌ | **PK** | | 설정 코드 | |
  | cf_name | nvarchar(50) | ✅ | | | 설정명 | |
  | cf_value | int | ❌ | | | 설정 값 | 시퀀스/카운터로 사용 |
  | cf_strvalue | nvarchar(100) | ✅ | | | 문자열 값 | |
  | cf_edate | datetime | ✅ | | | 수정 일시 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`configlog` 테이블</strong>
  
  > 환경설정 수정 로그 테이블</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | cfl_type | int | ✅ | | | 로그 유형 | |
  | cfl_emp | int | ✅ | | | 직원 코드 | |
  | cfl_before | nvarchar(100) | ✅ | | | 변경 전 | |
  | cfl_after | nvarchar(100) | ✅ | | | 변경 후 | |
  | cfl_date | datetime | ✅ | | | 변경 일시 | |
  | cfl_param | int | ✅ | | | 대상 식별자 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`country` 테이블</strong>
  
  >국가 코드 정보
  </summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | ctry_code | int | ❌ | **PK** | | 국가 코드 | |
  | ctry_name | nvarchar(50) | ❌ | | | 국가명 | |
  | ctry_ems | int | ✅ | | | EMS 배송여부 | |
  | ctry_interpackair | int | ✅ | | | 항공 배송여부 | |
  | ctry_interpackship | int | ✅ | | | 선박 배송여부 | |
  | ctry_udate | datetime | ✅ | | | 수정 일시 | |
  | ctry_2code | varchar(4) | ✅ | | | 국가단축코드(2) | |
  | ctry_3code | varchar(6) | ✅ | | | 국가단축코드(3) | |
  ---
  </details>
   
  <details><summary>📄 <strong>`customer` 테이블</strong>
  
  >고객 정보
  </summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | cust_code | int | ❌ | **PK** | | 고객 코드 | |
  | cust_name | nvarchar(50) | ❌ | | | 고객명 | |
  | cust_cell | varchar(20) | ✅ | | | 휴대전화 | |
  | cust_email | varchar(50) | ✅ | | | 이메일 | |
  | cust_addr | varchar(100) | ✅ | | | 주소 | |
  | cust_grade | int | ✅ | | | 등급 | |
  | cust_status | int | ✅ | | | 상태 | |
  | cust_idate | datetime | ✅ | | | 등록 일시 | |
  | cust_udate | datetime | ✅ | | | 수정 일시 | |
  | cust_lastsaledate | datetime | ✅ | | | 최종 구매일 | |
  | cust_tell | varchar(20) | ✅ | | | 전화 | |
  | cust_country | int | ✅ | FK | country(ctry_code) | 국가 | |
  | cust_memo | varchar(200) | ✅ | | | 메모 | |
  | cust_point | int | ✅ | | | 포인트 | |
  | cust_key1 | varchar(30) | ✅ | | | 암호화값1| |
  | cust_key2 | varchar(30) | ✅ | | | 암호화값2 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`customerlog` 테이블</strong>
  
  >고객정보 수정 로그</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | custlog_type | int | ✅ | | | 유형 | |
  | custlog_before | varchar(50) | ✅ | | | 변경 전 | |
  | custlog_after | varchar(50) | ✅ | | | 변경 후 | |
  | custlog_param | int | ✅ | | | 대상 코드 | |
  | custlog_emp | int | ✅ | | | 작업자 | |
  | custlog_date | datetime | ✅ | | | 일시 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`custorder` 테이블</strong>
  
  >고객주문
  </summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | cord_code | int | ❌ | **PK** | | 주문 코드 | |
  | cord_date | datetime | ❌ | | | 주문일 | |
  | cord_cust | int | ✅ | FK | customer(cust_code) | 고객 코드 | |
  | cord_status | int | ✅ | | | 상태 | |
  | cord_bprice | float | ✅ | | | 매입가 합계 | |
  | cord_amount_krw | int | ✅ | | | 결제 KRW | |
  | cord_amount_usd | float | ✅ | | | 결제 USD | |
  | cord_staff | int | ✅ | | | 담당자 | |
  | cord_idate | datetime | ✅ | | | 등록일 | |
  | cord_udate | datetime | ✅ | | | 수정일 | |
  | cord_memo | nvarchar(255) | ✅ | | | 메모 | |
  | cord_sdate | date | ✅ | | | 배송(출고)일 | |
  | cord_exchange | int | ✅ | | | 환율 | |
  | cord_address | nvarchar(100) | ✅ | | | 배송지 | |
  | cord_country | int | ✅ | FK | country(ctry_code) | 국가 | |
  | cord_shipping | int | ✅ | | | 배송수단 | |
  | cord_fee | float | ✅ | | | 수수료 | |
  | cord_sales | int | ✅ | | | 매출 코드 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`custorderdetail` 테이블</strong>
  
  >고객주문 상세 내역</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | --- |
  | cordd_code | int | ❌ | **PK** | | 주문 코드 | 복합 PK (cordd_code, cordd_seq) |
  | cordd_seq | int | ❌ | **PK** | | 순번 | 복합 PK |
  | cordd_pdt | int | ✅ | | | 상품 코드 | |
  | cordd_bprice | int | ✅ | | | 매입가 | |
  | cordd_orderqty | int | ✅ | | | 주문수량 | |
  | cordd_sprice | int | ✅ | | | 판매가 | |
  | cordd_offerkrw | float | ✅ | | | 제안가 KRW | |
  | cordd_offeruds | float | ✅ | | | 제안가 USD | |
  | cordd_amountusd | float | ✅ | | | 금액 USD | |
  | cordd_offerqty | int | ✅ | | | 제안 수량 | |
  | cordd_feeapply | int | ✅ | | | 수수료 적용 | |
  | cordd_status | int | ✅ | | | 상태 | |
  | cordd_memo | nvarchar(100) | ✅ | | | 메모 | |
  | cordd_pdtnumber | nvarchar(50) | ✅ | | | 상품번호 텍스트 | |
  | cordd_amountkrw | float | ✅ | | | 금액 KRW | |
  ---
  </details>
   
  <details><summary>📄 <strong>`custorderlog` 테이블</strong>
  
  >고객주문 수정 로그</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | cordlog_type | int | ✅ | | | 유형 | |
  | cordlog_befor | varchar(50) | ✅ | | | 변경 전 | |
  | cordlog_after | varchar(50) | ✅ | | | 변경 후 | |
  | cordlog_param | int | ✅ | | | 대상 코드 | |
  | cordlog_emp | int | ✅ | | | 작업자 | |
  | cordlog_date | datetime | ✅ | | | 일시 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`dailyreport` 테이블</strong>
  
  >일결산 상품 정보 테이블</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | --- |
  | dayr_date | int | ❌ | **PK** | | 기준일(YYYYMMDD 등) | 복합 PK (dayr_date, dayr_pdt) |
  | dayr_pdt | int | ❌ | **PK** | | 상품 코드 | 복합 PK |
  | dayr_sprice | int | ✅ | | | 판매가 | |
  | dayr_bprice | float | ✅ | | | 매입가 | |
  | dayr_beginstock | int | ✅ | | | 기초재고 | |
  | dayr_endstock | int | ✅ | | | 기말재고 | |
  | dayr_purqty | int | ✅ | | | 매입수량 | |
  | dayr_purAmount | float | ✅ | | | 매입금액 | |
  | dayr_saleqty | int | ✅ | | | 판매수량 | |
  | dayr_saleAmount | int | ✅ | | | 판매금액 | |
  | dayr_bstockamount | int | ✅ | | | 기초재고금액 | |
  | dayr_estockamount | int | ✅ | | | 기말재고금액 | |
  | dayr_top | int | ✅ | | | 분류-대 | |
  | dayr_mid | int | ✅ | | | 분류-중 | |
  | dayr_bot | int | ✅ | | | 분류-소 | |
  | dayr_sup | int | ✅ | | | 공급사 | |
  | dayr_ledgerstock | int | ✅ | | | 장부재고 | |
  | dayr_loststock | int | ✅ | | | 분실재고 | |
  | dayr_taxable | int | ✅ | | | 과세 | |
  | dayr_taxfree | int | ✅ | | | 면세 | |
  | dayr_paycash | float | ✅ | | | 현금결제 | |
  | dayr_paycard | float | ✅ | | | 카드결제 | |
  | dayr_payaccount | float | ✅ | | | 계좌이체 | |
  | dayr_paypoint | float | ✅ | | | 포인트 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`delivery` 테이블</strong>
  
  >배달정보 </summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | --- |
  | del_code | int | ❌ | **PK** | | 배송 코드 | |
  | del_cust | int | ✅ | FK | customer(cust_code) | 고객 코드 | |
  | del_country | int | ❌ | | | 국가 코드 | |
  | del_addr | varchar(200) | ❌ | | | 주소 | |
  | del_recipient | varchar(100) | ❌ | | | 수령인 | |
  | del_tel | varchar(50) | ❌ | | | 연락처 | |
  | del_invoice | varchar(50) | ✅ | | | 운송장번호 | |
  | del_idate | datetime | ✅ | | | 등록일 | |
  | del_udate | datetime | ✅ | | | 수정일 | |
  | del_salecode | int | ✅ | FK | sales(sale_code) | 매출 코드 | |
  | del_status | int | ✅ | | | 상태 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`emplog` 테이블</strong>
  
  >직원 수정 로그</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | emplog_type | int | ✅ | | | 유형 | |
  | emplog_before | varchar(50) | ✅ | | | 변경 전 | |
  | emplog_after | varchar(50) | ✅ | | | 변경 후 | |
  | emplog_param | int | ✅ | | | 대상 | |
  | emplog_emp | int | ✅ | | | 작업자 | |
  | emplog_date | datetime | ✅ | | | 일시 | |
  | emplog_param2 | int | ✅ | | | 보조 파라미터 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`employee` 테이블</strong>
  
  >직원정보</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | emp_code | int | ❌ | **PK** | | 직원 코드 | |
  | emp_name | nvarchar(50) | ❌ | | | 이름 | |
  | emp_password | varchar(255) | ✅ | | | 비밀번호 해시 | |
  | emp_level | nvarchar(30) | ❌ | | | 직급/레벨 | |
  | emp_cell | nvarchar(20) | ✅ | | | 휴대전화 | |
  | emp_email | nvarchar(50) | ✅ | | | 이메일 | |
  | emp_addr | nvarchar(100) | ✅ | | | 주소 | |
  | emp_status | int | ❌ | | | 상태 | |
  | emp_idate | datetime | ✅ | | | 입사/등록일 | |
  | emp_udate | datetime | ✅ | | | 수정일 | |
  | emp_memo | nvarchar(200) | ✅ | | | 메모 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`payment` 테이블</strong>
  
  >공급사 결제 내역</summary>
  
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | pay_code | int | ❌ | **PK** | | 지급 코드 | |
  | pay_sup | int | ❌ | **IDX_payment**, FK | supplier(sup_code) | 공급사 | |
  | pay_purcode | int | ✅ | | | 매입 코드 | |
  | pay_paycash | int | ✅ | | | 현금 | |
  | pay_accounttransfer | int | ✅ | | | 계좌이체 | |
  | pay_paycredit | int | ✅ | | | 카드 | |
  | pay_idate | datetime | ✅ | | | 등록일 | |
  | pay_udate | datetime | ✅ | | | 수정일 | |
  | pay_date | datetime | ❌ | | | 지급일 | |
  | pay_paynote | int | ✅ | | | 어음 | |
  | pay_DC | int | ✅ | | | DC | |
  | pay_coupone | int | ✅ | | | 쿠폰 | |
  | pay_supsiby | int | ✅ | | | 상계 | |
  | pay_etc | int | ✅ | | | 기타 | |
  | pay_bank | varchar(20) | ✅ | | | 은행 | |
  | pay_account | varchar(20) | ✅ | | | 계좌 | |
  | pay_depasitor | varchar(20) | ✅ | | | 예금주 | |
  | pay_memo | varchar(100) | ✅ | | | 메모 | |
  | pay_type | int | ❌ | | | 지급 유형 | |
  | pay_status | int | ❌ | | | 상태 | |
  | pay_emp | int | ✅ | | | 처리 직원 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`paymentlog` 테이블</strong>
  
  >공급사 결제 내역 수정 로그</summary>
  
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | paylog_type | int | ✅ | | | 유형 | |
  | paylog_before | varchar(50) | ✅ | | | 변경 전 | |
  | paylog_after | varchar(50) | ✅ | | | 변경 후 | |
  | paylog_param | int | ✅ | | | 대상 코드 | |
  | paylog_emp | int | ✅ | | | 작업자 | |
  | paylog_date | datetime | ✅ | | | 일시 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`pointhistory` 테이블</strong>
  
  >회원포인트 변경내역</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | ph_type | int | ✅ | | | 유형 | |
  | ph_cust | int | ✅ | | | 고객 코드 | |
  | ph_param | int | ✅ | | | 관련 코드 | |
  | ph_point | int | ✅ | | | 변동 포인트 | |
  | ph_previous | int | ✅ | | | 이전 포인트 | |
  | ph_date | datetime | ✅ | | | 일시 | |
  | ph_seq | int | ✅ | | | 일련번호 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`product` 테이블</strong>
  
  >제품정보 </summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | --- |
  | pdt_code | int | ❌ | **PK** | | 상품 코드 | |
  | pdt_name_kr | nvarchar(50) | ❌ | | | 상품명(국문) | |
  | pdt_name_en | nvarchar(50) | ✅ | | | 상품명(영문) | |
  | pdt_number | nvarchar(30) | ✅ | | | 품번 | |
  | pdt_spec | nvarchar(30) | ✅ | | | 규격 | |
  | pdt_top | int | ✅ | **IDX_product** | | 분류-대 | 다중 컬럼 인덱스 |
  | pdt_mid | int | ✅ | **IDX_product** | | 분류-중 | 다중 컬럼 인덱스 |
  | pdt_bot | int | ✅ | **IDX_product** | | 분류-소 | 다중 컬럼 인덱스 |
  | pdt_stock | int | ✅ | | | 재고 | |
  | pdt_status | int | ✅ | | | 상태 | |
  | pdt_bprice | int | ✅ | | | 매입가 | |
  | pdt_sprice_krw | int | ✅ | | | 판매가(KRW) | |
  | pdt_sprice_usd | decimal(18,2) | ✅ | | | 판매가(USD) | |
  | pdt_weight | decimal(18,2) | ✅ | | | 중량 | |
  | pdt_width | decimal(18,2) | ✅ | | | 가로 | |
  | pdt_length | decimal(18,2) | ✅ | | | 세로 | |
  | pdt_height | decimal(18,2) | ✅ | | | 높이 | |
  | pdt_idate | datetime | ✅ | | | 등록일 | |
  | pdt_udate | datetime | ✅ | | | 수정일 | |
  | pdt_tax | int | ❌ | | | 과세구분 | |
  | pdt_sup | int | ✅ | **IDX_product1**, FK | supplier(sup_code) | 공급사 | |
  | pdt_point | int | ✅ | | | 적립 포인트 | |
  | pdt_memo | varchar(100) | ✅ | | | 메모 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`productlog` 테이블</strong>
  
  > 제품변경 로그</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | pdtlog_type | int | ✅ | | | 유형 | |
  | pdtlog_before | varchar(50) | ✅ | | | 변경 전 | |
  | pdtlog_after | varchar(50) | ✅ | | | 변경 후 | |
  | pdtlog_param | int | ✅ | | | 대상 코드 | |
  | pdtlog_emp | int | ✅ | | | 작업자 | |
  | pdtlog_date | datetime | ✅ | | | 일시 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`catpurchaseegory` 테이블</strong>
  
  >매입전표</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | pur_code | int | ❌ | **PK** | | 매입 코드 | |
  | pur_date | datetime | ✅ | **IDX_purchase1** | | 매입일 | |
  | pur_idate | datetime | ✅ | | | 등록일 | |
  | pur_udate | datetime | ✅ | | | 수정일 | |
  | pur_sup | int | ✅ | **IDX_purchase**, FK | supplier(sup_code) | 공급사 | |
  | pur_amount | float | ✅ | | | 매입금액 | |
  | pur_payment | float | ✅ | | | 지급금액 | |
  | pur_type | int | ✅ | | | 유형 | |
  | pur_note | varchar(50) | ✅ | | | 비고 | |
  | pur_taxable | int | ❌ | | | 과세 | |
  | pur_taxfree | int | ❌ | | | 면세 | |
  | pur_emp | int | ✅ | FK | employee(emp_code) | 담당자 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`purchaselog` 테이블</strong>
  
  > 매입 수정 로그</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | purlog_type | int | ✅ | | | 유형 | |
  | purlog_before | varchar(50) | ✅ | | | 변경 전 | |
  | purlog_after | varchar(50) | ✅ | | | 변경 후 | |
  | purlog_param | int | ✅ | | | 대상 코드 | |
  | purlog_emp | int | ✅ | | | 작업자 | |
  | purlog_date | datetime | ✅ | | | 일시 | |
  | purlog_param2 | int | ✅ | | | 보조 파라미터 | |
  | purlog_order | int | ✅ | | | 정렬/순번 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`purdetail` 테이블</strong></summary>
  
  >매입상세내역
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | --- |
  | purd_code | int | ❌ | **IDX_purdetail**, FK | purchase(pur_code) | 매입 코드 | |
  | purd_purchase | int | ✅ | | | (예비)매입 코드 | |
  | purd_pdt | int | ✅ | **IDX_purdetail1**, FK | product(pdt_code) | 상품 코드 | |
  | purd_qty | int | ✅ | | | 수량 | |
  | purd_bprice | float | ✅ | | | 매입가 | |
  | purd_amount | float | ✅ | | | 금액 | |
  | purd_sprice | int | ✅ | | | 판매가 | |
  | purd_memo | nvarchar(50) | ✅ | | | 메모 | |
  | purd_status | int | ✅ | | | 상태 | |
  | purd_idate | datetime | ✅ | | | 등록일 | |
  | purd_udate | datetime | ✅ | | | 수정일 | |
  | purd_seq | int | ❌ | | | 순번 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`purorder` 테이블</strong>
  
  >발주전표</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | pord_code | int | ❌ | **PK** | | 발주 코드 | |
  | pord_sup | int | ✅ | **IDX_purorder**, FK | supplier(sup_code) | 공급사 | |
  | pord_date | datetime | ✅ | **IDX_purorder1** | | 발주일 | |
  | pord_arrivaldate | datetime | ✅ | | | 입고예정일 | |
  | pord_Amount | int | ✅ | | | 발주금액 | |
  | pord_idate | datetime | ✅ | | | 등록일 | |
  | pord_udate | datetime | ✅ | | | 수정일 | |
  | pord_type | int | ✅ | | | 유형 | |
  | pord_note | varchar(100) | ✅ | | | 비고 | |
  | pord_status | int | ✅ | | | 상태 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`purorderdetail` 테이블</strong>
  
  >발주상세내역</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | pordd_code | int | ❌ | **IDX_purorderdetail**, FK | purorder(pord_code) | 발주 코드 | |
  | pordd_pdt | int | ✅ | **IDX_purorderdetail1**, FK | product(pdt_code) | 상품 코드 | |
  | pordd_qty | int | ✅ | | | 수량 | |
  | pordd_bprice | float | ✅ | | | 매입가 | |
  | pordd_sprice | int | ✅ | | | 판매가 | |
  | pordd_amount | int | ✅ | | | 금액 | |
  | pordd_idate | datetime | ✅ | | | 등록일 | |
  | pordd_udate | datetime | ✅ | | | 수정일 | |
  | pordd_seq | int | ✅ | | | 순번 | |
  | pordd_status | int | ✅ | | | 상태 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`saledetail` 테이블</strong>
  
  >판매상세내역</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | saled_code | int | ❌ | **IDX_saledetail**, FK | sales(sale_code) | 매출 코드 | |
  | saled_pdt | int | ❌ | **IDX_saledetail1**, FK | product(pdt_code) | 상품 코드 | |
  | saled_bprice | int | ✅ | | | 매입가 | |
  | saled_sprice_krw | int | ✅ | | | 판매가(KRW) | |
  | saled_sprice_usd | float | ✅ | | | 판매가(USD) | |
  | saled_dc | int | ✅ | | | 할인 | |
  | saled_qty | int | ❌ | | | 수량 | |
  | saled_amount_krw | int | ✅ | | | 금액(KRW) | |
  | saled_amount_usd | float | ✅ | | | 금액(USD) | |
  | saled_tax | int | ✅ | | | 세액 | |
  | saled_point | int | ✅ | | | 포인트 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`salepay` 테이블</strong>
  
  >판매결제내역</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | spay_code | int | ❌ | **PK** | | 결제 코드 | |
  | spay_cash_krw | int | ✅ | | | 현금(KRW) | |
  | spay_cash_use | float | ✅ | | | 현금(외화) | |
  | spay_account_krw | int | ✅ | | | 계좌(KRW) | |
  | spay_account_usd | float | ✅ | | | 계좌(USD) | |
  | spay_credit_krw | int | ✅ | | | 카드(KRW) | |
  | spay_credit_usd | float | ✅ | | | 카드(USD) | |
  | spay_point_krw | int | ✅ | | | 포인트(KRW) | |
  | spay_point_usd | float | ✅ | | | 포인트(USD) | |
  | spay_exchenge | int | ✅ | | | 환율 | |
  | spay_salecode | int | ✅ | FK | sales(sale_code) | 매출 코드 | |
  | spay_idate | datetime | ✅ | | | 등록일 | |
  | spay_udate | datetime | ✅ | | | 수정일 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`sales` 테이블</strong>
  
  >판매내역</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | --- |
  | sale_code | int | ❌ | **PK** | | 매출 코드 | |
  | sale_date | datetime | ✅ | | | 거래일 | |
  | sale_cust | int | ✅ | **IDX_sales** | | 고객 코드 | FK 미선언(인덱스만 존재) |
  | sale_type | int | ✅ | | | 유형 | |
  | sale_bprice | float | ✅ | | | 매입가 합계 | |
  | sale_sprice_krw | int | ✅ | | | 매출액(KRW) | |
  | sale_sprice_usd | float | ✅ | | | 매출액(USD) | |
  | sale_dc | int | ✅ | | | 할인 | |
  | sale_tax | float | ✅ | | | 세액 | |
  | sale_reward | int | ✅ | | | 적립금 | |
  | sale_origine | int | ✅ | | | 원거래 | |
  | sale_udate | datetime | ✅ | | | 수정일 | |
  | sale_delivery | int | ✅ | | | 배송 방식 | |
  | sale_delfee | int | ✅ | | | 배송비 | |
  | sale_emp | int | ✅ | FK | employee(emp_code) | 담당자 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`supplier` 테이블</strong>
  
  >공급사 정보</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | -- |
  | sup_code | int | ❌ | **PK** | | 공급사 코드 | |
  | sup_name | varchar(50) | ❌ | | | 상호 | |
  | sup_bzno | varchar(20) | ✅ | | | 사업자번호 | |
  | sup_bztype | varchar(30) | ✅ | | | 업태 | |
  | sup_industry | varchar(30) | ✅ | | | 업종 | |
  | sup_tel | varchar(20) | ✅ | | | 전화 | |
  | sup_fax | varchar(20) | ✅ | | | 팩스 | |
  | sup_manager | varchar(30) | ✅ | | | 담당자 | |
  | sup_cel | varchar(20) | ✅ | | | 담당자 휴대폰 | |
  | sup_ceoname | varchar(30) | ✅ | | | 대표자 | |
  | sup_ceotel | char(20) | ✅ | | | 대표 전화 | |
  | sup_email | varchar(50) | ✅ | | | 이메일 | |
  | sup_url | varchar(100) | ✅ | | | 홈페이지 | |
  | sup_status | int | ✅ | | | 상태 | |
  | sup_memo | varchar(50) | ✅ | | | 메모 | |
  | sup_bank | varchar(10) | ✅ | | | 은행 | |
  | sup_account | varchar(20) | ✅ | | | 계좌 | |
  | sup_accname | varchar(30) | ✅ | | | 예금주 | |
  | sup_idate | datetime | ✅ | | | 등록일 | |
  | sup_address | varchar(100) | ✅ | | | 주소 | |
  | sup_paytype | int | ❌ | | | 결제 조건 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`supplierlog` 테이블</strong>
  
  >공급사 변경 로그</summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | ----- | ----- | ----- | ----- | ----- | ----- | ----- |
  | suplog_type | int | ✅ | | | 유형 | |
  | suplog_before | varchar(50) | ✅ | | | 변경 전 | |
  | suplog_after | varchar(50) | ✅ | | | 변경 후 | |
  | suplog_param | int | ✅ | | | 대상 코드 | |
  | suplog_emp | int | ✅ | | | 작업자 | |
  | suplog_date | datetime | ✅ | | | 일시 | |
  ---
  </details>
   
  <details><summary>📄 <strong>`worktable` 테이블</strong>
  
  >일결산 작업 기록
  </summary>
  
  | 칼럼명 | 자료형 | NULL 여부 | PK/Index | 참조 관계 | 설명 | 비고 |
  | --- | --- | --- | --- | --- | --- | --- |
  | work_date | int | ❌ | **PK** | | 작업 기준일 | 복합 PK (work_date, work_type) |
  | work_type | int | ❌ | **PK** | | 작업 유형 | 복합 PK |
  | work_exestart | datetime | ✅ | | | 시작 일시 | |
  | work_exeend | datetime | ✅ | | | 종료 일시 | |
  | work_emp | int | ✅ | | | 실행 직원 | |
  | work_saleupdate | int | ✅ | | | 매출 갱신 수 | |
  | work_purupdate | int | ✅ | | | 매입 갱신 수 | |
  | work_param | int | ✅ | | | 파라미터 | |
  </details>
</details>

## ⚙️ 저장 프로시저 목록

<details>
<summary>상세 보기</summary>

## 🔹 usp_UpdateConfig [환경설정 채번 코드 수정]

각 항목의 최신 채번 코드/문자값을 갱신합니다. 신규 등록 직후 공통으로 호출됩니다.

```

CREATE PROCEDURE [dbo].[usp_UpdateConfig]
    @code int, 
    @value int,
    @str varchar(50)
AS
BEGIN 
    SET NOCOUNT ON;
    UPDATE config
       SET cf_value  = @value,
           cf_strvalue = @str,
           cf_edate  = GETDATE()
     WHERE cf_code  = @code;
END
```
---
## 🔹 usp_InsertEmpAccessLog [사원 접근 로그 기록]

화면/기능 접근 등 사원 행위를 accesslog에 적재합니다.
```
CREATE PROCEDURE [dbo].[usp_InsertEmpAccessLog]
    @logType int,
    @empCode int,
    @parameter int
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO accesslog(acslog_type, acslog_emp, acslog_param, acslog_date)
    VALUES(@logType, @empCode, @parameter, GETDATE());
END
```
---
## 🔹 usp_InsertEmployee [사원 등록]

신규 사원을 등록하고, 설정코드(cf_code=16)의 채번을 갱신합니다.
```
CREATE PROCEDURE [dbo].[usp_InsertEmployee]
    @empName varchar(50) ,
    @empPassword varchar(50),
    @empLevel varchar(20),
    @empCell varchar(20),
    @empEmain varchar(200),
    @empAddr varchar(250),
    @empStatus int,
    @empMemo varchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @empCode int;
    SET @empCode = (SELECT MAX(ISNULL(cf_value,0)) + 1 FROM config WHERE cf_code = 16);

    INSERT INTO employee
        (emp_code, emp_name, emp_password, emp_level, emp_cell, emp_addr,
         emp_status, emp_idate, emp_udate, emp_memo)
    VALUES
        (@empCode, @empName, @empPassword, @empLevel, @empCell, @empAddr,
         @empStatus, GETDATE(), GETDATE(), @empMemo);

    EXEC usp_UpdateConfig @code = 16, @value = @empCode, @str = '';
END
```
---
## 🔹 usp_InsertLog [범용 로그 일괄 입력]

전달받은 테이블 타입(dbo.logInfo) 데이터를 대상 로그 테이블에 일괄 적재합니다. @target 값으로 목적 테이블을 구분합니다.
```
CREATE PROCEDURE [dbo].[usp_InsertLog]
    @target varchar(50),
    @logs dbo.logInfo READONLY
AS
BEGIN
    SET NOCOUNT ON;

    IF @target = 'ProductLog'
    BEGIN
        INSERT INTO productlog(pdtlog_type, pdtlog_before, pdtlog_after, pdtlog_param, pdtlog_emp, pdtlog_date)
        SELECT logType, beforeValue, afterValue, empCode, logParam, GETDATE() FROM @logs;
    END
    ELSE IF @target = 'SupplierLog'
    BEGIN
        INSERT INTO supplierlog(suplog_type, suplog_before, suplog_after, suplog_param, suplog_emp, suplog_date)
        SELECT logType, beforeValue, afterValue, empCode, logParam, GETDATE() FROM @logs;
    END
    ELSE IF @target = 'PaymentLog'
    BEGIN
        INSERT INTo paymentlog(paylog_type, paylog_before, paylog_after, paylog_param, paylog_emp, paylog_date)
        SELECT logType, beforeValue, afterValue, empCode, logParam, GETDATE() FROM @logs;
    END
    ELSE IF @target = 'CustomerLog'
    BEGIN
        INSERT INTO customerlog(custlog_type, custlog_before, custlog_after, custlog_param, custlog_emp, custlog_date)
        SELECT logType, beforeValue, afterValue, empCode, logParam, GETDATE() FROM @logs;
    END
    ELSE IF @target = 'AccessLog'
    BEGIN
        INSERT INTO accesslog(acslog_type, acslog_emp, acslog_param, acslog_date)
        SELECT logType, empCode, logParam, GETDATE() FROM @logs;
    END
    ELSE IF @target = 'EmployeeLog'
    BEGIN
        INSERT INTO emplog(emplog_type, emplog_before, emplog_after, emplog_param, emplog_emp, emplog_date)
        SELECT logType, beforeValue, afterValue, empCode, logParam, GETDATE() FROM @logs;
    END 
END
```
---
## 🔹 usp_InsertProduct [상품 등록]

신규 상품을 등록하고, 설정코드(cf_code=15)의 채번을 갱신합니다.
```
CREATE PROCEDURE [dbo].[usp_InsertProduct]
    @pdtNumber  varchar(20),
    @pdtNameKr  varchar(50),
    @pdtNameEn  varchar(40),
    @pdtSup     int, 
    @pdtTop     int,
    @pdtMid     int,
    @pdtBot     int,
    @pdtStatus  int,
    @pdtBprice  decimal,
    @pdtPriceKrw int,
    @pdtPriceUsd decimal,
    @pdtWeight  decimal,
    @pdtWidth   decimal,
    @pdtLength  decimal,
    @pdtHeight  decimal,
    @pdtTax     int
AS 
BEGIN 
    SET NOCOUNT ON;

    DECLARE @pdtCode int;
    SET @pdtCode = (SELECT ISNULL(cf_value, 0) + 1 FROM config WHERE cf_code = 15);

    INSERT INTO product
        (pdt_code, pdt_number, pdt_name_kr, pdt_name_en, pdt_sup,
         pdt_top, pdt_mid, pdt_bot, pdt_status, pdt_bprice,
         pdt_sprice_krw, pdt_sprice_usd, pdt_weight, pdt_width,
         pdt_length, pdt_height, pdt_tax, pdt_stock, pdt_idate, pdt_udate)
    VALUES
        (@pdtCode, @pdtNumber, @pdtNameKr, @pdtNameEn, @pdtSup,
         @pdtTop, @pdtMid, @pdtBot, @pdtStatus, @pdtBprice,
         @pdtPriceKrw, @pdtPriceUsd, @pdtWeight, @pdtWidth,
         @pdtLength, @pdtHeight, @pdtTax, 0, GETDATE(), GETDATE());

    EXEC usp_UpdateConfig @code = 15, @value = @pdtCode, @str = '';
END
```
🔹 usp_UpdateEmployee [직원 정보 수정]

직원 정보를 갱신합니다.

```
CREATE PROCEDURE [dbo].[usp_UpdateEmployee]
    @empCode int,
    @empName varchar(50) ,
    @empPassword varchar(50),
    @empLevel varchar(20),
    @empCell varchar(20),
    @empEmain varchar(200),
    @empAddr varchar(250),
    @empStatus int,
    @empMemo varchar(50)
AS
BEGIN
    SET NOCOUNT ON;

	UPDATE employee 
	SET emp_name = @empName,
	emp_password =  @empPassword,
	emp_level =  @empLevel,	 
	emp_cell =  @empCell,
	emp_addr =  @empAddr,
	emp_status =  @empStatus,
	emp_udate =  GETDATE(),
	emp_memo =  @empMemo
	WHERE emp_code = @empCode;
END
```
---

## 🔹 usp_UpdateProduct [상품 정보 수정]

상품 기본 정보를 갱신합니다.
```
CREATE PROCEDURE [dbo].[usp_UpdateProduct]
    @pdtCode   int,
    @pdtNumber varchar(20),
    @pdtNameKr varchar(50),
    @pdtNameEn varchar(40),
    @pdtSup    int, 
    @pdtTop    int,
    @pdtMid    int,
    @pdtBot    int,
    @pdtStatus int,
    @pdtBprice decimal,
    @pdtPriceKrw int,
    @pdtPriceUsd decimal,
    @pdtWeight decimal,
    @pdtWidth  decimal,
    @pdtLength decimal,
    @pdtHeight decimal,
    @pdtTax    int
AS 
BEGIN 
    SET NOCOUNT ON;

    UPDATE product
       SET pdt_number     = @pdtNumber,
           pdt_name_kr    = @pdtNameKr, 
           pdt_name_en    = @pdtNameEn, 
           pdt_sup        = @pdtSup,
           pdt_top        = @pdtTop, 
           pdt_mid        = @pdtMid, 
           pdt_bot        = @pdtBot, 
           pdt_status     = @pdtStatus, 
           pdt_bprice     = @pdtBprice, 
           pdt_sprice_krw = @pdtPriceKrw, 
           pdt_sprice_usd = @pdtPriceUsd, 
           pdt_weight     = @pdtWeight, 
           pdt_width      = @pdtWidth, 
           pdt_length     = @pdtLength, 
           pdt_height     = @pdtHeight, 
           pdt_tax        = @pdtTax, 
           pdt_udate      = GETDATE()
     WHERE pdt_code       = @pdtCode;
END
```
---
</details>

## 🧩 사용자 정의 테이블 형식 (TVP)

<details>
  <summary>상세보기</summary>
  
  ### 🔹 loginfo 
> 각종 로그를 기록 할 때 사용됩니다.

**🛠️ 사용 프로시저:**  
- `usp_InsertLog`

**📋 컬럼 구성:**

| 컬럼명 | 데이터형 | NULL 여부 | 설명 |
|--------|-----------|------------|------|
| logtype | int | ❌ | 로그 타입 |
| beforeValue | varchar(50) | ✅ | 변경 전 데이터 |
| afterValue | varchar(50) | ✅ | 변경 후 데이터 |
| logParam | int | ❌ | 적용 파라미터 |
| empCode | int | ❌ | 직원코드 |

</details>
