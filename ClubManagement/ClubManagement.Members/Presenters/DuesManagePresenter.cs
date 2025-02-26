using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Members.Models;
using ClubManagement.Members.Repositories;
using ClubManagement.Members.Views;
using ClubManagement.Common.Hlepers;

namespace ClubManagement.Members.Presenters
{
    public class DuesManagePresenter
    {
        IDuesManageView _view;
        IDuesRepsotiry _repository;
        DuesModel _model;

        public DuesManagePresenter(IDuesManageView view , IDuesRepsotiry repsotiry )
        {
            this._view = view;
            this._repository = repsotiry;
            _model = new DuesModel();
            _view.MemberSearchEvent += LoadMember;
            _view.StatementSearchEvent += LoadState;
            _view.StatementAddEvent += AddState;
            _view.StatementEditEvent += EditState;
        }

        private void EditState(object sender, EventArgs e)
        {
            int code = _view.GetStateMentCode.Value;
            IStatementDetailView view = new StatementDetailView();
            IDuesRepsotiry repository = new DuesRepsotiry();
            StatementDetailPresenter presenter = new StatementDetailPresenter(view, repository);
            presenter.LoadStatement(code);
            view.ShowForm();
            LoadStatementList();
        }

        private void AddState(object sender, EventArgs e)
        {
            IStatementDetailView view = new StatementDetailView();
            IDuesRepsotiry repository = new DuesRepsotiry();
            new StatementDetailPresenter(view, repository);
            view.ShowForm();
            LoadStatementList();
        }

        private void LoadState(object sender, EventArgs e)
        {
            LoadStatementList();
        }

        private void LoadMember(object sender, EventArgs e)
        {
            LoadMemberList();
        }

        private void LoadMemberList()
        {
            //모델 속성 설정
            _model.SearchWord = _view.SearchWord;
            _model.FromDate = _view.FromDate;
            _model.ToDate = _view.ToDate;
            _model.SecessInclude = _view.SecessInclude;

            //회원 목록 호출 및 설정
            DataTable result = _repository.GetMemberList(_model);
            result.Columns.Add("No");
            result.Columns.Add("nonPayment");
            result.Columns["mem_code"].ColumnName = "code";
            result.Columns["mem_name"].ColumnName = "name";
            result.Columns["totaldues"].ColumnName = "totalDues";
            int i = 1;
            foreach (DataRow row in result.Rows)
            {
                row["No"] = i++;
                row["name"] = $"{row["name"].ToString().Trim()}({row["mem_birth"].ToString().Trim()})";
                int dueCount = Convert.ToInt32(row["totalDues"]);
                int paycount = Convert.ToInt32(row["payment"]);
                int free = Convert.ToInt32(row["free"]);
                int nonPay = dueCount - paycount - free;
                row["nonPayment"] = nonPay < 0 ? 0 : nonPay;
            }

            //전체 행 추가
            DataRow newRow = result.NewRow();
            newRow["No"] = 0;
            newRow["code"] = 0;
            newRow["name"] = "[전 체]";
            newRow["payment"] = 0;
            newRow["nonPayment"] = 0;
            result.Rows.InsertAt(newRow, 0);

            //결과 DataGridView에 Binding
            
            _view.SetMemberListBinding(result);
        }
        private void LoadStatementList()
        {
            //모델 속성 설정
            _model.SearchWord = _view.SearchWord;
            _model.FromDate = _view.FromDate;
            _model.ToDate = _view.ToDate;
            _model.SecessInclude = _view.SecessInclude;
            _model.MemberCode = _view.GetMemberCode;
            int memCode = (int)_model.MemberCode;
            int balance = 0;
            DataTable result;
            DataRow resultRow;

            if (memCode == 0)
            {
                //설정 기간 이전 잔액 확인
                resultRow = _repository.GetBalance(_model);
                balance = Convert.ToInt32(resultRow["deus"]) - Convert.ToInt32(resultRow["payment"]);
            }

            //설정 기간내 회비 입출금 내역 조회
            result = _repository.GetStateList(_model);

            //DataTable 컬럼 설정
            result.Columns.Add("No");
            result.Columns.Add("balance");
            result.Columns.Add("type");
            result.Columns["du_code"].ColumnName = "code";
            result.Columns["du_date"].ColumnName = "date";
            result.Columns["du_detail"].ColumnName = "statement";
            result.Columns["du_apply"].ColumnName = "applay";
            result.Columns["deposit"].ColumnName = "deposit";
            result.Columns["whthdrawal"].ColumnName = "withdrawal";
            if (memCode == 0)
            {
                DataRow newRow = result.NewRow();
                newRow["No"] = 0;
                //newRow["code"] = "";
                //newRow["Date"] = "";
                //newRow["type"] = "";
                //newRow["statement"] = "";
                //newRow["applay"] = "";
                //newRow["deposit"] = "";
                //newRow["withdrawal"] = "";
                newRow["balance"] = balance;
                int beforeBalance = balance;
                int i = 1;
                foreach (DataRow row in result.Rows)
                {
                    row["No"] = i++;
                    row["type"] = MemberHelper.GetDuesType(Convert.ToInt32(row["du_type"]));
                    int deposit = Convert.ToInt32(row["deposit"]);
                    int withdrawal = Convert.ToInt32(row["withdrawal"]);
                    beforeBalance = beforeBalance + deposit - withdrawal;
                    row["balance"] = beforeBalance;
                }
                result.Rows.InsertAt(newRow, 0);
            }
            {
                int i = 1;
                foreach (DataRow row in result.Rows)
                {
                    row["No"] = i++;
                }
            }
            
            _view.SetStateListBinding(result);
        }
    }
}
