using ProjectCSharp1.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ProjectCSharp1.Repository
{
    class ProductRepository
    {
        SqlConnection Connect()
        {
            SqlConnection connection = new SqlConnection(@"Data Source = DESKTOP-QBQVKOF; Initial Catalog = MarketDB; Integrated Security = True");
            return connection;
        }

        public List<ProductModel> viewProduct()
        {
            SqlConnection connect = Connect();
            SqlCommand command = new SqlCommand();
            SqlDataReader reader;

            List<ProductModel> listProduct = new List<ProductModel>();

            string query = "SELECT * FROM Product";

            command.Connection = connect;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;

            connect.Open();

            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ProductModel prod = new ProductModel();
                    prod.ID = int.Parse(reader["ProductID"].ToString());
                    prod.Name = reader["ProductName"].ToString();
                    prod.price = int.Parse(reader["Price"].ToString());
                    prod.quantity = int.Parse(reader["Quantity"].ToString());

                    listProduct.Add(prod);
                }
            }

            reader.Close();
            connect.Close();
            command.Dispose();

            return listProduct;
        }

        public void Insert(ProductModel prod)
        {
            SqlConnection connect = Connect();
            SqlCommand command = new SqlCommand();

            string query = "INSERT INTO Product VALUES(@Name, @Price, @Quantity)";

            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 50).Value = prod.Name;
            command.Parameters.Add("@Price", System.Data.SqlDbType.Int).Value = prod.price;
            command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int).Value = prod.quantity;

            command.Connection = connect;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;

            connect.Open();
            command.ExecuteReader();

            connect.Close();
            command.Dispose();
        }

        public void Update(ProductModel prod)
        {
            SqlConnection connect = Connect();
            SqlCommand command = new SqlCommand();

            string query = "UPDATE Product SET ProductName = @Name, Price = @Price, Quantity = @Quantity WHERE ProductId = @ID";

            command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = prod.ID;
            command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 50).Value = prod.Name;
            command.Parameters.Add("@Price", System.Data.SqlDbType.Int).Value = prod.price;
            command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int).Value = prod.quantity;

            command.Connection = connect;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;

            connect.Open();
            command.ExecuteNonQuery();

            connect.Close();
            command.Dispose();
        }

        public void Delete(int ID)
        {
            SqlConnection connect = Connect();
            SqlCommand command = new SqlCommand();

            string query = "DELETE FROM Product WHERE ProductID = @ID";

            command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = ID;

            command.Connection = connect;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;

            connect.Open();
            command.ExecuteNonQuery();

            connect.Close();
            command.Dispose();
        }
    }
}
