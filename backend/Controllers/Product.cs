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
    public class Product : ControllerBase
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
        public List<ProductResultRecord> Get()
        {
            List<ProductResultRecord> ProductRecordList = new List<ProductResultRecord>();
            SqliteDataReader sqlite_datareader;
            SqliteCommand sqlite_cmd;
            SqliteConnection sqlite_conn;
            sqlite_conn = CreateConnectionAP();
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM Product;";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                ProductResultRecord Obj = new ProductResultRecord();
                Obj.id = sqlite_datareader["ID"].ToString();
                Obj.name = sqlite_datareader["Name"].ToString();
                Obj.cost = sqlite_datareader["Cost"].ToString();
                Obj.description = sqlite_datareader["Description"].ToString();
                Obj.stock = sqlite_datareader["Stock"].ToString();
                Obj.purchaseDate = sqlite_datareader["PurchaseDate"].ToString();
                Obj.location = sqlite_datareader["Location"].ToString();
                Obj.manufctdate = sqlite_datareader["ManufacturedDate"].ToString();
                Obj.expiredate = sqlite_datareader["ExpireDate"].ToString();
                ProductRecordList.Add(Obj);
            }

            sqlite_conn.Close();
            sqlite_conn.Dispose();
            sqlite_cmd.Dispose();
            GC.Collect();
            return ProductRecordList;

            //return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public List<ProductResultRecord> Post([FromBody] ProductModel product)
        {
            List<ProductResultRecord> ProductResultList = new List<ProductResultRecord>();
            ProductResultRecord obj = new ProductResultRecord();
           
            obj.name = product.name; 
            obj.cost = product.cost; 
            obj.description = product.description;
            obj.stock = product.stock;
            obj.purchaseDate = product.purchaseDate;
            obj.location = product.location;
            obj.manufctdate = product.manufctdate;
            obj.expiredate = product.expiredate;

            SqliteCommand insertSQL;
            SqliteConnection sqlite_conn;
            sqlite_conn = CreateConnectionAP();

            //var id = new SqliteParameter("@id", SqlDbType.Text) { Value = product.id };
            var name = new SqliteParameter("@name", SqlDbType.Text) { Value = product.name };
            var cost = new SqliteParameter("@cost", SqlDbType.Text) { Value = product.cost };
            var description = new SqliteParameter("@description", SqlDbType.Text) { Value = product.description };
            var stock = new SqliteParameter("@stock", SqlDbType.Text) { Value = product.stock };
            var purchaseDate = new SqliteParameter("@purchaseDate", SqlDbType.Text) { Value = product.purchaseDate };
            var location = new SqliteParameter("@location", SqlDbType.Text) { Value = product.location };
            var manufctdate = new SqliteParameter("@manufctdate", SqlDbType.Text) { Value = product.manufctdate };
            var expiredate = new SqliteParameter("@expiredate", SqlDbType.Text) { Value = product.expiredate };

            insertSQL = new SqliteCommand("INSERT INTO Product (Name, Cost, Description, Stock, PurchaseDate, Location, ManufacturedDate, ExpireDate) VALUES (@name, @cost, @description, @stock, @purchaseDate, @location, @manufctdate, @expiredate)", sqlite_conn);
           // insertSQL.Parameters.Add(id);
            insertSQL.Parameters.Add(name);
            insertSQL.Parameters.Add(cost);
            insertSQL.Parameters.Add(description);
            insertSQL.Parameters.Add(stock);
            insertSQL.Parameters.Add(purchaseDate);
            insertSQL.Parameters.Add(location);
            insertSQL.Parameters.Add(manufctdate);
            insertSQL.Parameters.Add(expiredate);
            
            insertSQL.ExecuteNonQuery();
            

            ProductResultList.Add(obj);
            return ProductResultList;
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
