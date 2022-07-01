using DemoApp.DAL.Model;
using DemoApp.DAL.Repository;
using DEMOAPP.ViewModel;
using DinkToPdf;
using DinkToPdf.Contracts;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DEMOAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ParentController<Employee, EmployeeRepository>
    {
        private IConverter _converter;
        public EmployeeController(EmployeeRepository employeeRepository, IConverter converter) : base(employeeRepository)
        {
            _converter = converter;
        }


        [Route("[action]")]
        [HttpGet]
        public FileContentResult GetImg(int id)
        {

            var img = base.Repository.GetAnprData();
            byte[] byteArray = img.FirstOrDefault().imagedata;
            if (byteArray != null)
            {
                return new FileContentResult(byteArray, "image/jpeg");
            }
            else
            {
                return null;
            }
        }

        [Route("[action]")]
        [HttpGet]
        public List<Employees> GetReportRecog()
        {
            string todaydate = DateTime.Today.ToString("yyyy-MM-dd");
            List<ReportRecog> employee = new List<ReportRecog>();
            List<Employees> employees = new List<Employees>();

            DemoDBContext context = new DemoDBContext();
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "Getreportdata";
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@startdate";
                param.Value = todaydate;
                command.Parameters.Add(param);
                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@enddate";
                param1.Value = todaydate;
                command.Parameters.Add(param1);
                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@filterType";
                param2.Value = "all";
                command.Parameters.Add(param2);
                context.Database.OpenConnection();
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Employees Employeeobj = new Employees();
                    Employeeobj.UserID = dataReader.GetInt32(dataReader.GetOrdinal("UserID"));
                    Employeeobj.RecDateTime = dataReader.GetString(dataReader.GetOrdinal("RecDateTime"));
                    if (dataReader["WebPhoto"] != DBNull.Value)
                    {
                        Employeeobj.Photo = (byte[])(dataReader["WebPhoto"]);
                    }
                    Employeeobj.EmpName = dataReader.GetString(dataReader.GetOrdinal("UserName"));
                    Employeeobj.TimeIn = dataReader.GetString(dataReader.GetOrdinal("TimeIn"));
                    Employeeobj.TimeOut = dataReader.GetString(dataReader.GetOrdinal("TimeOut"));
                    TimeSpan span = Convert.ToDateTime(Employeeobj.TimeOut).Subtract(Convert.ToDateTime(Employeeobj.TimeIn));
                    Employeeobj.TimeIn = Convert.ToDateTime(Employeeobj.TimeIn).ToString("H:mm:ss");
                    if (span.ToString() != "00:00:00")
                    {
                        Employeeobj.TimeOut = Convert.ToDateTime(Employeeobj.TimeOut).ToString("H:mm:ss");
                        Employeeobj.Duration = span.ToString();
                    }
                    else
                    {
                        Employeeobj.TimeOut = "--";
                        Employeeobj.Duration = "--";
                    }                    
                    Employeeobj.CameraName = dataReader.GetString(dataReader.GetOrdinal("CameraName"));//base.Repository.GetReportRecog().Where(x => x.UserID == Employeeobj.UserID).Select(xx => xx.CameraName).FirstOrDefault().ToString();
                    Employeeobj.EmpType = dataReader.GetString(dataReader.GetOrdinal("EmpType"));
                    employees.Add(Employeeobj);
                }
            }          
            return employees;
        }
        [Route("[action]")]
        [HttpGet]
        public List<Employee> GetEmployees()
        {
            string todaydate = DateTime.Today.ToString("yyyy-MM-dd");
            List<Employee> employee = new List<Employee>();
            //List<Employees> employees = new List<Employees>();
            employee = base.Repository.GetEmployees().ToList().Where(x => x.RecDateTime.Substring(0, 10) == todaydate).ToList();
            //foreach (var item in employee)
            //{
            //    Employees Employeeobj = new Employees();
            //    Employeeobj.UserID = item.UserID;
            //    Employeeobj.id = item.id;
            //    Employeeobj.RecDateTime = item.RecDateTime;
            //    Employeeobj.Photo = item.WebPhoto;
            //    Employeeobj.EmpName = item.UserName;
            //    DateTime TimeIN = Convert.ToDateTime(employee.Where(x => x.UserID == item.UserID).Select(xx => xx.RecDateTime).FirstOrDefault().ToString());
            //    DateTime TimeOut = Convert.ToDateTime(employee.Where(x => x.UserID == item.UserID).Select(xx => xx.RecDateTime).LastOrDefault().ToString());
            //    Employeeobj.Outtime = TimeOut.ToString();
            //    TimeSpan span = TimeOut.Subtract(TimeIN);
            //    Employeeobj.Duration = span.ToString();
            //    Employeeobj.CameraName = item.CameraName;
            //    employees.Add(Employeeobj);
            //}
            return employee;
        }


        [Route("[action]")]
        [HttpGet]
        public List<Alarm> GetReportByFilter_Alarm(string filterType, string startdate, string enddate)
        {
            var Alarmdata = base.Repository.Alarm().ToList().Where(x => Convert.ToDateTime(x.RecDateTime.Substring(0, 10)) >= Convert.ToDateTime(startdate.Substring(0, 10)) && Convert.ToDateTime(x.RecDateTime.Substring(0, 10)) <= Convert.ToDateTime(enddate.Substring(0, 10))).ToList();
            return Alarmdata;
        }


        [Route("[action]")]
        [HttpGet]
        public IEnumerable<Anpr> GetAnprData()
        {
            List<Anpr> Anpr = new List<Anpr>();
            string todaydate = System.DateTime.Now.ToString("dd/MM/yyyy");
            DemoDBContext context = new DemoDBContext();
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "GetAnprdata";
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@todaydate";
                param.Value = todaydate;
                command.Parameters.Add(param);
                context.Database.OpenConnection();
                var dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    //id name    camera date    time imagedata
                    Anpr Employeeobj = new Anpr();
                    Employeeobj.id = dataReader.GetInt32(dataReader.GetOrdinal("id"));
                    Employeeobj.date = dataReader.GetString(dataReader.GetOrdinal("date"));
                    if (dataReader["imagedata"] != DBNull.Value)
                    {
                        Employeeobj.imagedata = (byte[])(dataReader["imagedata"]);
                    }
                    Employeeobj.name = dataReader.GetString(dataReader.GetOrdinal("name"));
                    Employeeobj.camera = dataReader.GetString(dataReader.GetOrdinal("camera"));
                    Employeeobj.time = dataReader.GetString(dataReader.GetOrdinal("time"));
                    Anpr.Add(Employeeobj);
                }
            }
            return Anpr;
        }

        [Route("[action]")]
        [HttpGet]
        public List<Employees> GetReportByFilter(string filterType, string startdate, string enddate)
        {
            if (filterType == "NoExit")
                filterType = "full-time";
            List<Employees> employees = new List<Employees>();
            if (filterType != "Recognition")
            {
                DemoDBContext context = new DemoDBContext();
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "Getreportdata";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@startdate";
                    param.Value = startdate;
                    command.Parameters.Add(param);
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@enddate";
                    param1.Value = enddate;
                    command.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@filterType";
                    param2.Value = filterType;
                    command.Parameters.Add(param2);

                    context.Database.OpenConnection();
                    var dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Employees Employeeobj = new Employees();
                        Employeeobj.UserID = dataReader.GetInt32(dataReader.GetOrdinal("UserID"));
                        Employeeobj.RecDateTime = dataReader.GetString(dataReader.GetOrdinal("RecDateTime"));
                        if (dataReader["WebPhoto"] != DBNull.Value)
                        {
                            Employeeobj.Photo = (byte[])(dataReader["WebPhoto"]);
                        }
                        Employeeobj.EmpName = dataReader.GetString(dataReader.GetOrdinal("UserName"));
                        Employeeobj.TimeIn = dataReader.GetString(dataReader.GetOrdinal("TimeIn"));
                        Employeeobj.TimeOut = dataReader.GetString(dataReader.GetOrdinal("TimeOut"));
                        TimeSpan span = Convert.ToDateTime(Employeeobj.TimeOut).Subtract(Convert.ToDateTime(Employeeobj.TimeIn));
                        Employeeobj.TimeIn = Convert.ToDateTime(Employeeobj.TimeIn).ToString("H:mm:ss");
                        if (span.ToString() != "00:00:00")
                        {
                            Employeeobj.TimeOut = Convert.ToDateTime(Employeeobj.TimeOut).ToString("H:mm:ss");
                            Employeeobj.Duration = span.ToString();
                        }
                        else
                        {
                            Employeeobj.TimeOut = "--";
                            Employeeobj.Duration = "--";
                        }
                        Employeeobj.CameraName = dataReader.GetString(dataReader.GetOrdinal("CameraName")); //base.Repository.GetReportRecog().Where(x => x.UserID == Employeeobj.UserID).Select(xx => xx.CameraName).FirstOrDefault().ToString();
                        Employeeobj.EmpType = dataReader.GetString(dataReader.GetOrdinal("EmpType"));
                        employees.Add(Employeeobj);
                    }
                }
            }
            else
            {
                List<UserInfo> UserInfo = new List<UserInfo>();
                UserInfo = base.Repository.GetUserInfoData().ToList();
                var employee = base.Repository.GetReportRecog().ToList().Where(x => Convert.ToDateTime(x.RecDateTime.Substring(0, 10)) >= Convert.ToDateTime(startdate) && Convert.ToDateTime(x.RecDateTime.Substring(0, 10)) <= Convert.ToDateTime(enddate)).ToList();
                foreach (var item in employee)
                {
                    if (UserInfo.Find(X => X.UserID == item.UserID) != null && UserInfo.Count > 0)
                    {
                        Employees Employeeobj = new Employees();
                        Employeeobj.UserID = item.UserID;
                        Employeeobj.id = item.id;
                        Employeeobj.RecDateTime = item.RecDateTime;
                        Employeeobj.Photo = null;
                        Employeeobj.EmpName = item.UserName;
                        Employeeobj.TimeIn = item.RecDateTime.Substring(0, 10);
                        Employeeobj.TimeOut = item.RecDateTime.Substring(0, 10);
                        Employeeobj.Duration = null;
                        Employeeobj.CameraName = item.CameraName;
                        if (UserInfo.Where(x => x.UserID == item.UserID).Select(z => z.mHome).FirstOrDefault() != null)
                            Employeeobj.EmpType = UserInfo.Where(x => x.UserID == item.UserID).Select(z => z.mHome).FirstOrDefault().ToString();
                        else
                            Employeeobj.EmpType = "";

                        employees.Add(Employeeobj);
                    }
                }
            }


            return employees;
        }


        [Route("[action]")]
        [HttpGet]
        public List<EmployeeMonthlyReport> GetReocgReportMonthlyReport(string startdate = "", string enddate = "")
        {            
            if(startdate == "" && enddate =="")
            {
                enddate  = DateTime.Today.ToString("yyyy-MM-dd");
                var today = DateTime.Now;
                var currentDate = new DateTime(today.Year, today.Month, 1);
                startdate = currentDate.ToString("yyyy-MM-dd");
            }
            List<EmployeeMonthlyReport> employees = new List<EmployeeMonthlyReport>();         
                DemoDBContext context = new DemoDBContext();
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "RecogMonthlyReport";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@startdate";
                    param.Value = startdate;
                    command.Parameters.Add(param);
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@enddate";
                    param1.Value = enddate;
                    command.Parameters.Add(param1);
                    context.Database.OpenConnection();
                    var dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        EmployeeMonthlyReport Employeeobj = new EmployeeMonthlyReport();
                        Employeeobj.UserID = dataReader.GetInt32(dataReader.GetOrdinal("UserID"));
                        Employeeobj.UserName = dataReader.GetString(dataReader.GetOrdinal("UserName"));
                        if (dataReader["WebPhoto"] != DBNull.Value)
                        {
                            Employeeobj.Photo = (byte[])(dataReader["WebPhoto"]);
                        }
                        Employeeobj.Presentdays = dataReader.GetInt32(dataReader.GetOrdinal("Presentdays"));
                        Employeeobj.Workingdays = dataReader.GetInt32(dataReader.GetOrdinal("Workingdays"));
                        Employeeobj.TotalHours = dataReader.GetInt32(dataReader.GetOrdinal("TotalHours"));
                        employees.Add(Employeeobj);
                    }
                }
                    
            return employees;
        }

        [Route("[action]")]
        [HttpGet]
        public List<Vehicle> GetReportByNotificationFilter(string filterType, string startdate, string enddate)
        {

            DateTime date = DateTime.ParseExact("31.07.2013".Replace('.', '/'), "dd/MM/yyyy", null);
            var Vehicledata = base.Repository.GetVehicleData().ToList().Where(x => DateTime.ParseExact(x.date.Replace('.', '/'), "dd/MM/yyyy", null) >= DateTime.ParseExact(startdate.Replace('.', '/'), "dd/MM/yyyy", null) && DateTime.ParseExact(x.date.Replace('.', '/'), "dd/MM/yyyy", null) <= DateTime.ParseExact(enddate.Replace('.', '/'), "dd/MM/yyyy", null)).ToList();
            return Vehicledata;
        }

        [Route("[action]")]
        [HttpGet]
        public List<Anpr> GetReportByAnprFilter(string filterType, string startdate, string enddate)
        {    
            //tbllpr date should in this formay 15/03/2021 dd/mm/yyyy
            List<Anpr> Anprdata = new List<Anpr>();
            DateTime date = DateTime.ParseExact(startdate.Replace('.', '/'), "dd/MM/yyyy", null);
            using (var context = new DemoDBContext())
            {
               Anprdata = context.Anpr.AsNoTracking().ToList().Where(x => DateTime.ParseExact(x.date.Replace('.', '/'), "dd/MM/yyyy", null) >= DateTime.ParseExact(startdate.Replace('.', '/'), "dd/MM/yyyy", null) && DateTime.ParseExact(x.date.Replace('.', '/'), "dd/MM/yyyy", null) <= DateTime.ParseExact(enddate.Replace('.', '/'), "dd/MM/yyyy", null)).OrderByDescending(x=>x.time).ToList();
            } 
            return Anprdata;
        }


        [Route("[action]")]
        [HttpGet]
        public IEnumerable<Vehicle> GetVehicleData()
        {
            string todaydate = System.DateTime.Now.ToString("dd/MM/yyyy");
            return base.Repository.GetVehicleData().Where(x => x.date == todaydate);
        }
     

        [Route("[action]")]
        [HttpGet]
        public IEnumerable<UserInfo> GetUserInfoData()
        {
            return base.Repository.GetUserInfoData();
        }

        //[Route("[action]/{id}")]
        //[HttpGet]

        //public IEnumerable<Employee> GetByCountry(int id)
        //{
        //    return (base.Repository.GetEmployeesByCountry(id));
        //}

        private static int InsertDataVehicleData(string VehicleOwner, string VehiclNumber, string Type , byte[] filebytes)
        {           

            int InsertRecord = 0;
            string Date = DateTime.Now.ToString();
            DemoDBContext context = new DemoDBContext();
            using var connection =(SqlConnection) context.Database.GetDbConnection();
            connection.Open();

            using var command = new SqlCommand("AddVechile", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@VehicleOwner", SqlDbType.NVarChar).Value = VehicleOwner;
            command.Parameters.Add("@VehiclNumber", SqlDbType.NVarChar).Value = VehiclNumber;
            command.Parameters.Add("@Type", SqlDbType.NVarChar).Value = Type;
            command.Parameters.Add("@Date", SqlDbType.NVarChar).Value = Date;
            command.Parameters.Add("@Image", SqlDbType.VarBinary).Value = filebytes;

            InsertRecord =  command.ExecuteNonQuery();

            //using (var command = context.Database.GetDbConnection().CreateCommand())
            //{
            //    CommandType = CommandType.StoredProcedure;
            //    context.Database.OpenConnection();
            //    SqlParameter parameter1 = new SqlParameter("@Imagedata", SqlDbType.VarBinary);
            //    parameter1.Value = filebytes;
            //    string Date = DateTime.Now.ToString();
            //    command.CommandText = "INSERT INTO tblVehicleDetailsTemp (VehicleOwner,VehiclNumber,Type,Date,Image) VALUES ('" + VehicleOwner + "','" + VehiclNumber + "','" + Type + "','" + Date + "' , @Imagedata);";
            //    InsertRecord = command.ExecuteNonQuery();

            //    context.Database.CloseConnection();
            //}
            return InsertRecord;
        }

        //[Route("[action]")]
        //[HttpGet]
        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public IActionResult AddVechile( [FromForm] VechileModel vechileModel)
        {
            int val = 0;
            try
            {               
                               
                if (vechileModel.File.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        vechileModel.File.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        //string s = Convert.ToBase64String(fileBytes);
                        byte[] s =fileBytes;
                        val = InsertDataVehicleData(vechileModel.VehicleOwner, vechileModel.VehiclNumber.ToUpper(), vechileModel.Type , fileBytes);

                    }
                }
                else
                {
                   return BadRequest();
                }
                // val = InsertDataVehicleData(VehicleOwner, VehiclNumber.ToUpper(), Type);
            }
            catch (Exception ex)
            {

            }
            return Ok();


            //return RowEffected;
        }

        [Route("[action]")]
        [HttpGet]
        public List<Employees> GetAnprVechileDetaildata()
        {
            List<ReportRecog> employee = new List<ReportRecog>();
            List<Employees> employees = new List<Employees>();
            try
            {
                string todaydate = DateTime.Today.ToString("dd/MM/yyyy");               

                DemoDBContext context = new DemoDBContext();
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetAnprReportdata_test";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@startdate";
                    param.Value = todaydate;
                    command.Parameters.Add(param);

                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@enddate";
                    param1.Value = todaydate;
                    command.Parameters.Add(param1);

                    context.Database.OpenConnection();
                    var dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Employees Employeeobj = new Employees();
                        Employeeobj.UserID = 0;
                        Employeeobj.RecDateTime = dataReader.GetString(dataReader.GetOrdinal("RecDateTime"));
                        if (dataReader["WebPhoto"] != DBNull.Value)
                        {
                            Employeeobj.Photo = (byte[])(dataReader["WebPhoto"]);
                        }
                        Employeeobj.EmpName = dataReader.GetString(dataReader.GetOrdinal("VehicleOwner"));
                        Employeeobj.TimeIn = dataReader.GetString(dataReader.GetOrdinal("TimeIn"));
                        Employeeobj.TimeOut = dataReader.GetString(dataReader.GetOrdinal("TimeOut"));
                        TimeSpan span = Convert.ToDateTime(Employeeobj.TimeOut).Subtract(Convert.ToDateTime(Employeeobj.TimeIn));
                        Employeeobj.TimeIn = Convert.ToDateTime(Employeeobj.TimeIn).ToString("H:mm:ss");
                        if (span.ToString() != "00:00:00")
                        {
                            Employeeobj.TimeOut = Convert.ToDateTime(Employeeobj.TimeOut).ToString("H:mm:ss");
                            Employeeobj.Duration = span.ToString();
                        }
                        else
                        {
                            Employeeobj.TimeOut = "--";
                            Employeeobj.Duration = "--";
                        }
                        Employeeobj.CameraName = dataReader.GetString(dataReader.GetOrdinal("VehiclNumber"));//base.Repository.GetReportRecog().Where(x => x.UserID == Employeeobj.UserID).Select(xx => xx.CameraName).FirstOrDefault().ToString();
                        Employeeobj.EmpType = dataReader.GetString(dataReader.GetOrdinal("Type"));
                        employees.Add(Employeeobj);
                    }
                }
            }
            catch(Exception ex)
            { }
            return employees;
        }

        [Route("[action]")]
        [HttpGet]
        public List<Employees> GetAnprVechileDetaildata_Report(string startdate, string enddate)
        {           
            List<Employees> employees = new List<Employees>();         
             DemoDBContext context = new DemoDBContext();
             using (var command = context.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "GetAnprReportdata";
            command.CommandType = CommandType.StoredProcedure;
            SqlParameter param = new SqlParameter();
            param.ParameterName = "@startdate";
            param.Value = startdate;
            command.Parameters.Add(param);
            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@enddate";
            param1.Value = enddate;
            command.Parameters.Add(param1);
            context.Database.OpenConnection();
            var dataReader = command.ExecuteReader();
        while (dataReader.Read())
        {
                    Employees Employeeobj = new Employees();
                    Employeeobj.UserID = 0;
                    Employeeobj.RecDateTime = dataReader.GetString(dataReader.GetOrdinal("RecDateTime"));
                    if (dataReader["WebPhoto"] != DBNull.Value)
                    {
                        Employeeobj.Photo = (byte[])(dataReader["WebPhoto"]);
                    }
                    Employeeobj.EmpName = dataReader.GetString(dataReader.GetOrdinal("VehicleOwner"));
                    Employeeobj.TimeIn = dataReader.GetString(dataReader.GetOrdinal("TimeIn"));
                    Employeeobj.TimeOut = dataReader.GetString(dataReader.GetOrdinal("TimeOut"));
                    TimeSpan span = Convert.ToDateTime(Employeeobj.TimeOut).Subtract(Convert.ToDateTime(Employeeobj.TimeIn));
                    Employeeobj.TimeIn = Convert.ToDateTime(Employeeobj.TimeIn).ToString("H:mm:ss");
                    if (span.ToString() != "00:00:00")
                    {
                        Employeeobj.TimeOut = Convert.ToDateTime(Employeeobj.TimeOut).ToString("H:mm:ss");
                        Employeeobj.Duration = span.ToString();
                    }
                    else
                    {
                        Employeeobj.TimeOut = "--";
                        Employeeobj.Duration = "--";
                    }
                    Employeeobj.CameraName = dataReader.GetString(dataReader.GetOrdinal("VehiclNumber"));//base.Repository.GetReportRecog().Where(x => x.UserID == Employeeobj.UserID).Select(xx => xx.CameraName).FirstOrDefault().ToString();
                    Employeeobj.EmpType = dataReader.GetString(dataReader.GetOrdinal("Type"));
                    employees.Add(Employeeobj);
                }
    }          
           
            return employees;
        }

    }
}
