using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.Interface;
using BRIC.Entry;
using BRIC.Repositories;
using common.Model;
using common.DTOs;
using BRIC.Services;
using common.Helpers;
using System.Reflection;

namespace BRIC.Presenters
{
    public class ProductDetailPresenter
    {
        IProductDetailView _view;
        ProductDetailModel _model;
        ProductDetailModel _original;
        List<LogDto> _logs;
        IProductRepository _repository;
        ProductService _service;
        Action<string, int> _openPopup;
        Action<string, int, Delegate> _openPopupWithCallback;
        //IViewFactory _viewFactory;
        public ProductDetailPresenter(IProductDetailView view, int code, Action<string, int> openPopup, Action<string, int, Delegate> popupWithCallback)
        {
            _view = view;
            //_model = new ProductDetailModel();
            _repository = new ProductRepository();
            _service = new ProductService(_repository, UserSession.CurrentUser.EmployeeCode);
            _openPopup = openPopup;
            _openPopupWithCallback = popupWithCallback;
            _view.CategorySelectEvent += SelectCategory;
            _view.CloseFormEvent += CloseForm;
            _view.SaveEvent += SaveProduct;
            _view.SupplierSelectEvent += SelectSupplier;
            _view.KrwPriceChangeEvent += (s, e) => { _model.UpdatePriceKrw(_view.PdtPriceKrw); UpdateView(); }; //PriceChangedUsd;
            _view.UsdPriceChangeEvent += (s, e) => { _model.UpdatePriceUsd(_view.PdtPriceUsd); UpdateView(); };//PriceChangeKrw;
            _view.ProfitRateChageEvent += (s, e) => { _model.UpdateProfitRate(_view.PdtProfitRate); UpdateView(); }; //ChangedProfitRate;
            CheckIsNewItem(code);
            SetViewTabpage(code);
            _view.ShowForm();
        }

        /// <summary>
        /// 판매가, 이익율 뷰 변경
        /// </summary>
        private void UpdateView()
        {
            _view.PdtPriceKrw = _model.PdtPriceKrw;
            _view.PdtPriceUsd = _model.PdtPriceUsd;
            _view.PdtProfitRate = _model.ProfitRate;
        }

        /// <summary>
        /// 제품 코드가 0일 경우 새로운 제품으로 IsNew = true로 변경 후 
        /// 분류와 공급사는 모두 1로 적용
        /// 추후 기본 분류와 기본 공급사를 설정하는 config를 만든 후 기본 분류와 공급사 적용되도록 수정 필요
        /// </summary>
        /// <param name="code"></param>
        private void CheckIsNewItem(int code)
        {
            
            if( code == 0 )
            {
                var newProduct = new ProductDetailDto
                {
                    CategoryTop = 1,
                    CategoryMid = 1,
                    CategoryBot = 1,
                    PdtSupplier = 1,
                    Exchange = _service.LoadExchange()
                };

                _model = ProductDetailModel.FromDto(newProduct);
                _model.IsNew = true;
                

                var (catKr, catEn) = _service.GetCategoryName(1,1,1);
                _view.CategoryNameEn = catEn;
                _view.CategoryNameKr = catKr;
                
                _view.PdtSupplier = _service.GetSupplierName(1);
            }
            else
            {
                //제품 코드가 있을 경우 제품 정보 조회
                LoadProductInfo(code);
            }
        }

        private void SetViewTabpage(int code)
        {
            if (code == 0)
                _view.SetTabcontrol(true);
            else
            {
                var purComuns = new Dictionary<string, string>
                {
                    {"purDate", "매입일"},
                    {"purType", "구분"},
                    {"purQty",  "매입량"},
                    {"purAmount",   "매입액"},
                    {"purBprice",   "매입단가"},
                    {"purSupplier", "공급사"},
                };
                    var saleColumns = new Dictionary<string, string>
                {
                    {"saleDate", "판매일"},
                    {"saleType", "유형"},
                    {"saleAmount", "판매액"},
                    {"saleSprice", "판매단가"},
                    {"saleBprice", "판매원가"},
                    {"saleQty", "판매수량"},
                    {"saleCustomer", "고객"},
                    {"saleCode", "판매코드"},
                };

                    var logColumns = new Dictionary<string, string>
                {
                    {"logParam", "파라메터"},
                    {"logType", "작업내역"},
                    {"logBefore", "변경전"},
                    {"logAfter", "변경후"},
                    {"logEmpName", "작업자명"},
                    {"logEmp", "직원코드"},
                    {"logDate", "변경일"}
                };

                    var colums = new List<Dictionary<string, string>>
                {
                    purComuns,
                    saleColumns,
                    logColumns
                };
                _view.SetTabcontrol(false, colums);
            }
            
        }

        /// <summary>
        /// 공급사 클릭시 공급사 검색창을 띄운 후 공급사 선택
        /// 선택된 공급사로 지정 공급사 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectSupplier(object sender, EventArgs e)
        {
            try
            {
                _openPopupWithCallback?.Invoke("SupplierSearch", 0,new Action<int> (code =>
                {
                    _model.PdtSupplier = code;
                    _view.PdtSupplier = _service.GetSupplierName(code);
                }));
            }
            catch (Exception ex)
            {
                _view.ShowMessage("공급사 지정 실패\n" + ex.Message);
            }
        }

        /// <summary>
        /// 확인 버튼 클릭 이벤트
        /// 모델의 IsNew가 True일 경우 새로운 제품 등록, False 일 경우 제품 수정 진행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveProduct(object sender, EventArgs e)
        {
            try
            {
                _model.PdtNameKr = _view.PdtNameKr;
                _model.PdtNameEn = _view.PdtNameEn;
                _model.PdtNumber = _view.PdtNumber;
                _model.PdtWidth = _view.PdtWidth;
                _model.Pdtlength = _view.Pdtlength;
                _model.PdtHigth = _view.PdtHigth;
                _model.PdtWeigth = _view.PdtWeigth;
                _model.PdtStatus = _view.PdtStatus;
                _model.PdtTax = _view.PdtTax;
                _model.PdtMemo = _view.PdtMemo;


                var productDtop = _model.ToDto();

                if (_model.IsNew)
                    _service.InsertProduct(productDtop);
                else
                {
                    //UpdateProductInfo();
                    CompareModel();
                    _service.UpdateProdcut(productDtop, _logs);
                }
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessage("제품 정보 입력 오류 : " + ex.Message);
            }
        }

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }

        /// <summary>
        /// 분류 지정 이벤트
        /// 클릭 시 분류 선택창 활성화 후 CallBack으로 대,중, 소분류 값을 받아 적용
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCategory(object sender, EventArgs e)
        {
            try
            {
                //_openPopup?.Invoke("CategoryBoard", 1);
                _openPopupWithCallback?.Invoke("CategoryBoard", 1, new Action<int, int, int> ((top, mid, bot) =>
                {
                    _model.CategoryTop = top;
                    _model.CategoryMid = mid;
                    _model.CategoryBot = bot;
                    var (catKr, catEn) =  _service.GetCategoryName(top, mid, bot);
                    _view.CategoryNameKr = catKr;
                    _view.CategoryNameEn = catEn;

                }));
            }
            catch (Exception ex)
            {
                _view.ShowMessage("분류 지정 실패\n" + ex.Message);
            }
        }

        /// <summary>
        /// 검색된 제품 정보 조회 후 View에 적용
        /// 팝업 메뉴 활성시 code값이 있을 경우 CheckIsNewItem 메소드를 통해 실행
        /// </summary>
        /// <param name="code"></param>
        public void LoadProductInfo(int code)
        {
            try
            {
                //제품 내역 조회 후 _model에 반영
                var productDto = _service.LoadProductInfo(code);

                //환율 정보는 product테이블이 아닌 conifig 테이블에 있어 호출 후 Dto에 반영
                //ProductModel내 Exchange는 Private Set으로 수정 불가
                productDto.Exchange = _service.LoadExchange();
                _model = ProductDetailModel.FromDto(productDto);

                //IsNew false 반영
                _model.IsNew = false;

                //분류 번호로 분류명 전체 정보 한글과 영문 수신
                var (catKr, catEn) = _service.GetCategoryName(_model.CategoryTop, _model.CategoryMid, _model.CategoryBot);

                //ivew 필드에 조회된 내역 등록
                _view.CategoryNameKr = catKr;
                _view.CategoryNameEn = catEn;

                _view.PdtSupplier = _service.GetSupplierName(_model.PdtSupplier);
                _view.PdtNameKr = _model.PdtNameKr;
                _view.PdtNameEn = _model.PdtNameEn;
                _view.PdtNumber = _model.PdtNumber;
                _view.PdtBprice = _model.PdtBprice;
                _view.PdtPriceKrw = _model.PdtPriceKrw;
                _view.PdtPriceUsd = _model.PdtPriceUsd;
                _view.PdtProfitRate = _model.ProfitRate;
                _view.PdtWidth = _model.PdtWidth;
                _view.Pdtlength = _model.Pdtlength;
                _view.PdtHigth = _model.PdtHigth;
                _view.PdtWeigth = _model.PdtWeigth;
                _view.PdtStatus = _model.PdtStatus;
                _view.PdtTax = _model.PdtTax;
                _view.stock = _model.Pdtstock;
                _view.PdtMemo = _model.PdtMemo;

                _original = ProductDetailModel.FromDto(CloneHelper.DeepCopy(productDto));
            }
            catch(Exception ex)
            {
                _view.ShowMessage("제품정보 조회 실패\n" + ex.Message);
            }
        }

        /// <summary>
        /// 제품 수정 시 수정 내역 로그를 기록하기 위해 수정내역을 _lost에 등록
        /// </summary>
        private void CompareModel()
        {
            var originalDto = _original.ToDto();
            var EditDtop = _model.ToDto();
            _logs = _service.CompareModel<ProductDetailDto>(originalDto, EditDtop);
            foreach(var log in _logs)
            {
                log.EmployeeCode = UserSession.CurrentUser.EmployeeCode;
                log.Param = _model.Code;
            }
        }


    }
}
