using ProjectCSharp1.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ProjectCSharp1.Repository
{
    class TransactionRepository
    {
        SqlConnection Connect()
        {
            SqlConnection connection = new SqlConnection(@"Data Source = DESKTOP-QBQVKOF; Initial Catalog = MarketDB; Integrated Security = True");
            return connection;
        }
        public List<TransactionModel> viewTransaction()
        {
            SqlConnection connect = Connect();
            SqlCommand command = new SqlCommand();
            SqlDataReader reader;

            List<TransactionModel> listTransaction = new List<TransactionModel>();

            string query = "SELECT ht.TransactionID,pt.ProductName,dt.ProductQuantity,pt.Price,ht.PaymentMethod FROM HeaderTransaction ht JOIN DetailTransaction dt ON dt.TransactionID = ht.TransactionID JOIN Product pt ON pt.ProductID = dt.ProductID ORDER BY ht.TransactionID";

            command.Connection = connect;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;

            connect.Open();

            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TransactionModel trans = new TransactionModel();
                    trans.transID = int.Parse(reader["TransactionID"].ToString());
                    trans.Name = reader["ProductName"].ToString();
                    trans.soldQty = int.Parse(reader["ProductQuantity"].ToString());
                    trans.price = int.Parse(reader["Price"].ToString());
                    trans.payment = reader["PaymentMethod"].ToString();

                    listTransaction.Add(trans);
                }
            }

            reader.Close();
            connect.Close();
            command.Dispose();

            return listTransaction;
        }

        public void insertTemp()
        {
            SqlConnection connect = Connect();
            SqlCommand command = new SqlCommand();

            string query = "INSERT INTO HeaderTransaction VALUES(@Payment)";

            command.Parameters.Add("@Payment", System.Data.SqlDbType.VarChar, 50).Value = "dummy";

            command.Connection = connect;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;

            connect.Open();

            command.ExecuteReader();

            connect.Close();
            command.Dispose();
        }

        public int getHeaderID()
        {
            SqlConnection connect = Connect();
            SqlCommand command = new SqlCommand();
            SqlDataReader reader;

            int transID = -1;


            string query = "SELECT TOP 1 * FROM HeaderTransaction ORDER BY TransactionID DESC";

            command.Connection = connect;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;

            connect.Open();

            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    transID = int.Parse(reader["TransactionID"].ToString());
                }
            }

            reader.Close();
            connect.Close();
            command.Dispose();

            return transID;
        }

        public void insertHeader(string payment)
        {
            SqlConnection connect = Connect();
            SqlCommand command = new SqlCommand();

            string query = "UPDATE HeaderTransaction SET PaymentMethod = @Payment WHERE TransactionID = @ID";

            command.Parameters.Add("@Payment", System.Data.SqlDbType.VarChar, 50).Value = payment;
            command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = getHeaderID();

            command.Connection = connect;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;

            connect.Open();

            command.ExecuteNonQuery();

            connect.Close();
            command.Dispose();
        }

        public void insertDetail(ProductModel prod)
        {
            SqlConnection connect = Connect();
            SqlCommand command = new SqlCommand();

            string query = "INSERT INTO DetailTransaction VALUES(@TransID, @ProdID, @Quantity)";

            command.Parameters.Add("@TransID", System.Data.SqlDbType.Int).Value = getHeaderID();
            command.Parameters.Add("@ProdID", System.Data.SqlDbType.Int).Value = prod.ID;
            command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int).Value = prod.quantity;

            command.Connection = connect;
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;

            connect.Open();

            command.ExecuteReader();

            connect.Close();
            command.Dispose();
        }
    }
}
