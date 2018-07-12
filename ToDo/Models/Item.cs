using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ToDo;
using System;

namespace ToDo.Models
{
    public class Item
    {
        private int _id;
        private string _description;
        private string _date;
        private int _categoryId;

        public Item(string Description, string Date, int categoryId, int Id = 0)
        {
            
            _description = Description;
            _date = Date;
            _categoryId = categoryId;
            _id = Id;
        }

        public void SetDate(string date)
        {
            _date = date;
        }

        public string GetDate()
        {

            return _date;
        }

        public int GetCategoryId()
        {
            return _categoryId;
        }

        public int GetId()
        {
            return _id;
        }

        public void SetDescription(string description)
        {
            _description = description;
        }

        public string GetDescription()
        {
            return _description;
        }

    
        public override bool Equals(System.Object otherItem)
        {
            if (!(otherItem is Item))
            {
                return false;
            }
            else
            {
                Item newItem = (Item)otherItem;
                bool idEquality = (this.GetId() == newItem.GetId());
                bool descriptionEquality = (this.GetDescription() == newItem.GetDescription());
                bool dateEquality = (this.GetDate() == newItem.GetDate());
                bool categoryIdEquality = (this.GetCategoryId() == newItem.GetCategoryId());
                return (idEquality && descriptionEquality);
            }
        }

   

        public static List<Item> GetAll()
        {
            List<Item> allItems = new List<Item> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM items;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int itemId = rdr.GetInt32(0);
                string itemDescription = rdr.GetString(1);
                string itemDate = rdr.GetString(2);
                int itemCategoryId = rdr.GetInt32(3);
                Item newItem = new Item(itemDescription, itemDate, itemId, itemCategoryId);
                allItems.Add(newItem);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allItems;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM items;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
       
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO items (description, date, categoryId) VALUES (@ItemDescription, @ItemDate, @ItemCategoryID);";

            MySqlParameter description = new MySqlParameter();
            cmd.Parameters.AddWithValue("@ItemDescription", this._description);
            MySqlParameter date = new MySqlParameter();
            cmd.Parameters.AddWithValue("@ItemDAte", this._date);
            MySqlParameter categoryId = new MySqlParameter();
            cmd.Parameters.AddWithValue("@ItemCategoryId", this._date);
    

            cmd.ExecuteNonQuery(); 
            _id = (int) cmd.LastInsertedId; 

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }


        public static Item Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM `items` WHERE id = @thisId;";


            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = id;
            cmd.Parameters.Add(thisId);   

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int itemId = 0;
            string itemDescription = "";
            string itemDate = "";
            int itemCategoryId = 0;

            while (rdr.Read())
            {
                itemId = rdr.GetInt32(0);
                itemDescription = rdr.GetString(1);
                itemDate = rdr.GetString(2);
                itemCategoryId = rdr.GetInt32(3);
            }

            Item foundItem = new Item(itemDescription, itemDate, itemId); 


            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return foundItem; 
        }

        public void Edit(string newDescription)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE items SET description = @newDescription WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@newDescription";
            description.Value = newDescription;
            cmd.Parameters.Add(description);

            cmd.ExecuteNonQuery();
            _description = newDescription;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
  }
}