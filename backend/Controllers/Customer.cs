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
    public class Customer : ControllerBase
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
        public List<CustomerResultRecord> Get()
        {
            List<CustomerResultRecord> CustomerList = new List<CustomerResultRecord>();
            SqliteDataReader sqlite_datareader;
            SqliteCommand sqlite_cmd;
            SqliteConnection sqlite_conn;
            sqlite_conn = CreateConnectionAP();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM Customer;";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                CustomerResultRecord Obj = new CustomerResultRecord();
                Obj.id = sqlite_datareader["ID"].ToString();
                Obj.name = sqlite_datareader["Name"].ToString();
                Obj.email = sqlite_datareader["Email"].ToString();
                Obj.mobile = sqlite_datareader["Mobile"].ToString();
                Obj.address = sqlite_datareader["Address"].ToString();
                Obj.details = sqlite_datareader["Details"].ToString();
                Obj.userid = sqlite_datareader["UserId"].ToString();
                Obj.password = sqlite_datareader["Password"].ToString();
                CustomerList.Add(Obj);
            }

            sqlite_conn.Close();
            sqlite_conn.Dispose();
            sqlite_cmd.Dispose();
            GC.Collect();
            return CustomerList;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public List<CustomerResultRecord> Post([FromBody] CustomerModel customer)
        {
            List<CustomerResultRecord> CustomerResultList = new List<CustomerResultRecord>();
            CustomerResultRecord obj = new CustomerResultRecord();

         
            obj.name = customer.name;
            obj.email = customer.email;
            obj.mobile = customer.mobile;
            obj.address = customer.address;
            obj.details = customer.details;

            SqliteCommand insertSQL;
            SqliteConnection sqlite_conn;
            sqlite_conn = CreateConnectionAP();

            //var id = new SqliteParameter("@id", SqlDbType.Text) { Value = product.id };
            var name = new SqliteParameter("@name", SqlDbType.Text) { Value = customer.name };
            var email = new SqliteParameter("@email", SqlDbType.Text) { Value = customer.email };
            var mobile = new SqliteParameter("@mobile", SqlDbType.Text) { Value = customer.mobile };
            var address = new SqliteParameter("@address", SqlDbType.Text) { Value = customer.address };
            var details = new SqliteParameter("@details", SqlDbType.Text) { Value = customer.details };
            var userid = new SqliteParameter("@userid", SqlDbType.Text) { Value = customer.userid };
            var password = new SqliteParameter("@password", SqlDbType.Text) { Value = customer.password };

            insertSQL = new SqliteCommand("INSERT INTO Customer (Name, Email, Mobile, Address, Details, UserId, Password) VALUES (@name, @email, @mobile, @address, @details, @userid, @password)", sqlite_conn);
            // insertSQL.Parameters.Add(id);
            insertSQL.Parameters.Add(name);
            insertSQL.Parameters.Add(email);
            insertSQL.Parameters.Add(mobile);
            insertSQL.Parameters.Add(address);
            insertSQL.Parameters.Add(details);
            insertSQL.Parameters.Add(userid);
            insertSQL.Parameters.Add(password);

            insertSQL.ExecuteNonQuery();


            CustomerResultList.Add(obj);
            return CustomerResultList;
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
