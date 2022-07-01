using DemoApp.DAL.Model;
using FireApp.Model;
using FireApp.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FireApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamfyVFaceResult : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<List<ResultRecord>>> post([FromForm] FileModel faceApiModel)
        {
            var Success = 0;
            List<ResultRecord> ResultRecord = new List<ResultRecord>();
            string confidencee = string.Empty;
            if (faceApiModel.AccessorName != "wipro")
            {
                BadRequest();
            }
            try
            {
                var file = Request.Form.Files[0];
                // name 
                var folderName = Path.Combine("Resources", "image");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (faceApiModel != null)
                {
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        // dbpath
                        var dbPath = Path.Combine("", fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            Success = 1;
                            // image data after processing
                            await Task.Delay(2000);
                            ResultRecord = await GetImageData();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ResultRecord;            
        }


        static SqliteConnection CreateConnectionAP()
        {
            //C:\\ScanResultDB\\myDatabase.db
            string DBPath = "";
            //ConfigurationManager.AppSettings["DBPath"] + dbname + ".db";
            SqliteConnection sqlite_conn;
            // Create a new database connection:    
            sqlite_conn = new SqliteConnection("Data Source = C:\\Database\\ReportDatabase.db" + "" + ";");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }

      

        static List<ResultRecord> ReadData(SqliteConnection conn)
        {
            List<ResultRecord> FireRecordList = new List<ResultRecord>();
            SqliteDataReader sqlite_datareader;
            SqliteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();         
            sqlite_cmd.CommandText = "";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    ResultRecord FRObj = new ResultRecord();
                    FRObj.id = Convert.ToInt64(sqlite_datareader["id"].ToString());
                    FRObj.name = sqlite_datareader["name"].ToString();
                    FRObj.datetime = sqlite_datareader["date"].ToString();
                    FRObj.Photo = (byte[])(sqlite_datareader["imagedata"]);
                    FireRecordList.Add(FRObj);
                }
            
            conn.Close();
            conn.Dispose();
            sqlite_cmd.Dispose();
            GC.Collect();
            return FireRecordList;
        }


        private async Task<List<ResultRecord>> GetImageData()
        {
            SqliteConnection sqlite_conn;
            sqlite_conn = CreateConnectionAP();
            return ReadData(sqlite_conn);
        }


    }




}
