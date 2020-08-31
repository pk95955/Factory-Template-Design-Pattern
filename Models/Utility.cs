using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using ExcelDataReader;

namespace AttendanceImport_Factory_Template.Models
{ 
    /// <summary>
    /// utility to read Excel File through Excel Reader
    /// </summary>
    public class Utility
    {
   
        public static List<AttendanceModel> ConvertXLtoData(System.IO.Stream fileData)
        {
            
            DataTable dt = new DataTable();
            using (OleDbCommand cmd = new OleDbCommand())
            {

            }
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fileData);
            var result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });
            List<AttendanceModel> listAttendanceData = new List<AttendanceModel>();
            foreach (DataRow item in result.Tables[0].Rows)
            {
                listAttendanceData.Add(new AttendanceModel()
                {
                    empNo = item["empNo"].ToString(),
                        empName = item["empName"].ToString(),
                     attendanceDate = Convert.ToDateTime(item["attendanceDate"]),
                    inTime = item["InTime"].ToString(),
                       ouTime = item["outTime"].ToString()
                });
                //AttendanceModel attModel = new AttendanceModel
                //{
                //    empNo = item["empNo"].ToString(),
                //    empName = item["empName"].ToString(),
                //    attendanceDate = Convert.ToDateTime(item["endanceDate"]),
                //    inTime = item["InTime"].ToString(),
                //    ouTime = item["outTime"].ToString()
                //};
              //  listAttendanceModel.Add(attModel);
               
            }
            return listAttendanceData;
        }
        
    }
}