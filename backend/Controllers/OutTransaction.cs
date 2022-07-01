using Microsoft.AspNetCore.Mvc;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CamfyVAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutTransaction : ControllerBase
    {
        static SqliteConnection CreateConnectionAP()
        {
            //C:\\ScanResultDB\\myDatabase.db
            string DBPath = "";
            //ConfigurationManager.AppSettings["DBPath"] + dbname + ".db";
            SqliteConnection sqlite_conn;
            // Create a new database connection:    
            sqlite_conn = new SqliteConnection("Data Source = C:\\Database\\WarehouseDatabase.db" + "" + ";");
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
        // GET: api/<OutTransaction>
        [HttpGet]
        public List<OutTransactionResultRecord> Get()
        {
            List<OutTransactionResultRecord> result = new List<OutTransactionResultRecord>();
            //OutTransactionResultRecord obj = new OutTransactionResultRecord();

            SqliteDataReader sqlite_datareader;
            SqliteCommand sqlite_cmd;
            SqliteConnection sqlite_conn;
            sqlite_conn = CreateConnectionAP();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM OutTransaction;";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

           // string tid = id.ToString();

            while (sqlite_datareader.Read())
            {
                    OutTransactionResultRecord Obj = new OutTransactionResultRecord();
                    Obj.supplierName = sqlite_datareader["TransportSupplierName"].ToString();
                    Obj.productName = sqlite_datareader["ProductName"].ToString();
                    Obj.productStock = sqlite_datareader["ProductStock"].ToString();
                    Obj.productUnits = sqlite_datareader["ProductStock"].ToString();
                    Obj.amount = sqlite_datareader["Amount"].ToString();
                    Obj.id = sqlite_datareader["TransactionID"].ToString();
                    Obj.date = sqlite_datareader["TransactionDate"].ToString();
                    result.Add(Obj);
                
            }

            sqlite_conn.Close();
            sqlite_conn.Dispose();
            sqlite_cmd.Dispose();
            GC.Collect();
            return result;
           // return new string[] { "value1", "value2" };
        }

        // GET api/<OutTransaction>/5
        [HttpGet("{id}")]
        public List<OutTransactionResultRecord> Get(int id)
        {
            List<OutTransactionResultRecord> result = new List<OutTransactionResultRecord>();
            //OutTransactionResultRecord obj = new OutTransactionResultRecord();

            SqliteDataReader sqlite_datareader;
            SqliteCommand sqlite_cmd;
            SqliteConnection sqlite_conn;
            sqlite_conn = CreateConnectionAP();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM OutTransaction;";
            sqlite_datareader = sqlite_cmd.ExecuteReader();

            string tid = id.ToString();

            while (sqlite_datareader.Read())
            {
                string t = sqlite_datareader["TransactionID"].ToString();
                if (tid == t)
                {
                    OutTransactionResultRecord Obj = new OutTransactionResultRecord();
                    Obj.supplierName = sqlite_datareader["TransportSupplierName"].ToString();
                    Obj.productName = sqlite_datareader["ProductName"].ToString();
                    Obj.productStock = sqlite_datareader["ProductStock"].ToString();
                    Obj.productUnits = sqlite_datareader["ProductStock"].ToString();
                    Obj.amount = sqlite_datareader["Amount"].ToString();
                    Obj.id = sqlite_datareader["TransactionID"].ToString();
                    Obj.date = sqlite_datareader["TransactionDate"].ToString();
                    result.Add(Obj);
                }
            }

            sqlite_conn.Close();
            sqlite_conn.Dispose();
            sqlite_cmd.Dispose();
            GC.Collect();
            return result;
        }

        // POST api/<OutTransaction>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OutTransaction>/5
        [HttpPut("{id}")]
        public string Put(int units, [FromBody] OutTransactionModel tmodel)
        {
            List<OutTransactionResultRecord> result = new List<OutTransactionResultRecord>();
            OutTransactionResultRecord obj = new OutTransactionResultRecord();

            SqliteCommand insertSQL;
            SqliteConnection sqlite_conn;
            sqlite_conn = CreateConnectionAP();

            //var id = new SqliteParameter("@id", SqlDbType.Text) { Value = product.id };
            var sname = new SqliteParameter("@sname", SqlDbType.Text) { Value = tmodel.supplierName };
            var pname = new SqliteParameter("@pname", SqlDbType.Text) { Value = tmodel.productName };
            var stock = new SqliteParameter("@stock", SqlDbType.Text) { Value = tmodel.productStock };
            var amount = new SqliteParameter("@amount", SqlDbType.Int) { Value = units*300 };
            var unit = new SqliteParameter("@unit", SqlDbType.Int) { Value = units };
            string id = "";
            insertSQL = new SqliteCommand("INSERT INTO OutTransaction (TransportSupplierName, ProductName, ProductStock, ProductUnits, Amount, TransactionDate) VALUES (@sname, @pname, @stock, @unit, @amount, CURRENT_DATE)", sqlite_conn);
            // insertSQL.Parameters.Add(id);
            insertSQL.Parameters.Add(sname);
            insertSQL.Parameters.Add(pname);
            insertSQL.Parameters.Add(stock);
            insertSQL.Parameters.Add(unit);
            insertSQL.Parameters.Add(amount);

            insertSQL.ExecuteNonQuery();

            SqliteDataReader sqlite_datareader;
            SqliteCommand sqlite_cmd;
            //SqliteConnection sqlite_conn;
            //sqlite_conn = CreateConnectionAP();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "select * from OutTransaction ORDER BY TransactionID DESC LIMIT 1;";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
               // CustomerResultRecord Obj = new CustomerResultRecord();
                id = sqlite_datareader["TransactionID"].ToString();
               
               // CustomerList.Add(Obj);
            }

            sqlite_conn.Close();
            sqlite_conn.Dispose();
            sqlite_cmd.Dispose();
            GC.Collect();
            return id;
        }

        // DELETE api/<OutTransaction>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
