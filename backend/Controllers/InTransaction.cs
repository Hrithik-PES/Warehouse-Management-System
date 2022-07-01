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
    public class InTransaction : ControllerBase
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
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public string Post([FromBody] InTransactionModel tmodel)
        {
            SqliteCommand insertSQL;
            SqliteConnection sqlite_conn;
            sqlite_conn = CreateConnectionAP();

            //var id = new SqliteParameter("@id", SqlDbType.Text) { Value = product.id };
            var cname = new SqliteParameter("@cname", SqlDbType.Text) { Value = tmodel.customerName };
            var pname = new SqliteParameter("@pname", SqlDbType.Text) { Value = tmodel.productName };
            var stock = new SqliteParameter("@stock", SqlDbType.Text) { Value = tmodel.productStock };
            var units = new SqliteParameter("@units", SqlDbType.Text) { Value = tmodel.productUnits };

            insertSQL = new SqliteCommand("INSERT INTO InTransaction (CustomerName, ProductName, ProductStock, ProductUnits) VALUES (@cname, @pname, @stock, @units)", sqlite_conn);
            // insertSQL.Parameters.Add(id);
            insertSQL.Parameters.Add(cname);
            insertSQL.Parameters.Add(pname);
            insertSQL.Parameters.Add(stock);
            insertSQL.Parameters.Add(units);

            insertSQL.ExecuteNonQuery();
            return "success";
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
