using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using common.DTOs;
using common.Helpers;
using BRIC.Repositories;
using common.Interface;

namespace BRIC.Services
{
    public class EmployeeService
    {
        IEmployeeReporitory _repository;
        ILogRepository _logRepository;
        IDatabaseSession _dbsession;
        int _empCode;
        public EmployeeService(int empCode)
        {
            _repository = new EmployeeRepository();
            _logRepository = new LogRepository();
            _dbsession = new DatabaseSession();
            _empCode = empCode;
        }

        public DataTable LoadEmployeeList(int empStatus, int empCode)
        {
            var result = _repository.LoadEmployeeList(empStatus);

            result.Columns["emp_code"].ColumnName = "empCode";
            result.Columns["emp_name"].ColumnName = "empName";
            result.Columns["emp_level"].ColumnName = "empLevel";
            result.Columns["emp_cell"].ColumnName = "empCell";
            result.Columns["emp_email"].ColumnName = "empEmail";
            result.Columns["emp_addr"].ColumnName = "empAddr";
            result.Columns["emp_idate"].ColumnName = "empIdate";
            result.Columns["emp_udate"].ColumnName = "empUdate";
            result.Columns["emp_memo"].ColumnName = "emptMemo";
            result.Columns.Add("No", typeof(int));
            result.Columns.Add("empStatus", typeof(string));
            
            int i = 1;
            foreach(DataRow row in result.Rows)
            {
                row["No"] = i;
                row["empStatus"] = StatusHelper.GetText(StatusHelper.Keys.EmployeeStatus, Convert.ToInt32(row["emp_status"]));
                i++;
            }
            result.Columns.Remove("emp_status");
            _logRepository.InsertAccessLog(0, empCode, 940, _dbsession.Connection, _dbsession.Transaction);
            return result;

        }

        public EmployeeDto LoadEmployeeInfo(int code)
        {
            var result = _repository.LoadEmployeeInfo(code);

            EmployeeDto employee = new EmployeeDto
            {
                EmployeeCode = code,
                EmployeeName = result["emp_name"].ToString(),
                EmployeePhon = result["emp_cell"].ToString(),
                EmployeeLevel = result["emp_level"].ToString(),
                EmployeeStatus = Convert.ToInt32(result["emp_status"]),
                EmployeePassword = result["emp_password"].ToString().Trim(),
                Employeememo = result["emp_memo"].ToString(),
                EmployeeIdate = Convert.ToDateTime(result["emp_idate"]),
                EmployeeUdate = Convert.ToDateTime(result["emp_udate"])

            };
            _dbsession.Begin();
            try
            {
                _logRepository.InsertAccessLog(code, _empCode, 940, _dbsession.Connection, _dbsession.Transaction);
                _dbsession.Commin();
            }
            catch(Exception ex)
            {
                _dbsession.Rollback();
                throw new Exception("직원접속 로그 기록 실패\n" + ex.Message);
            }
            finally
            {
                _dbsession.Dispose();
            }
            return employee;
        }

        public void InsertEmployee(EmployeeDto model, int empCode)
        {
            _dbsession.Begin();
            try
            {
                _repository.InsertEmployee(model, _dbsession.Connection, _dbsession.Transaction);
                _logRepository.InsertAccessLog(model.EmployeeCode, empCode, 941, _dbsession.Connection, _dbsession.Transaction);
                _dbsession.Commin();
            }
            catch(Exception ex)
            {
                _dbsession.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbsession.Dispose();
            }
        }

        public void UpdateEmployee(EmployeeDto model, List<LogDto> logs)
        {
            _dbsession.Begin();
            try
            {
                _repository.UpdateEmployee(model, _dbsession.Connection, _dbsession.Transaction);
                _logRepository.InsertLog(logs, LogHelper.Targets.EmployeeLog, _dbsession.Connection, _dbsession.Transaction);
                _logRepository.InsertAccessLog(model.EmployeeCode, _empCode, 942, _dbsession.Connection, _dbsession.Transaction);
                _dbsession.Commin();
            }
            catch(Exception ex)
            {
                _dbsession.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbsession.Dispose();
            }
        }
    }
}
