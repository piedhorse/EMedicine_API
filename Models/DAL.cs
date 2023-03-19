using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EMedicineBE.Models
{
    public class DAL
    {
        public Response register(Users users, SqlConnection sqlConnection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_register", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", users.FirstName);
            cmd.Parameters.AddWithValue("@LastName", users.LastName);
            cmd.Parameters.AddWithValue("@Password", users.Password);
            cmd.Parameters.AddWithValue("@Email", users.Email);
            cmd.Parameters.AddWithValue("@Fund", 0);
            cmd.Parameters.AddWithValue("@Type", "Users");
            cmd.Parameters.AddWithValue("@Type", "Pending");
            sqlConnection.Open();
            int i = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "User Registered succesfully";

            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "User Registration failed";
            }


            return response;
        }

        public Response login(Users users, SqlConnection sqlConnection)
        {

            SqlDataAdapter adapter = new SqlDataAdapter("sp_login", sqlConnection);
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand.Parameters.AddWithValue("@Email", users.Email);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            Response response = new Response();
            Users user = new Users();
            if (dt.Rows.Count > 0)
            {
                user.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                user.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                user.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Type = Convert.ToString(dt.Rows[0]["Type"]);
                response.StatusCode = 200;
                response.StatusMessage = "User is valid";
                response.user = user;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "User is invalid";
                response.user = null;
            }

            return response;


        }

        public Response viewUser(Users users, SqlConnection sqlConnection)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("p_viewUser", sqlConnection);
            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand.Parameters.AddWithValue("@ID", users.ID);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            Response response = new Response();
            Users user = new Users();

            if (dt.Rows.Count > 0)
            {
                user.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                user.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                user.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Type = Convert.ToString(dt.Rows[0]["Type"]);
                user.Fund = Convert.ToDecimal(dt.Rows[0]["Fund"]);
                user.CreatedOn = Convert.ToDateTime(dt.Rows[0]["CreatedOn"]);
                user.Password = Convert.ToString(dt.Rows[0]["Password"]);
                response.StatusCode = 200;
                response.StatusMessage = "User exists.";

            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "User does not exists.";
                response.user = user;

            }

            return response;

        }

        public Response updateProfile(Users users, SqlConnection sqlConnection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_updateProfile", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", users.FirstName);
            cmd.Parameters.AddWithValue("@LastName", users.LastName);
            cmd.Parameters.AddWithValue("@Password", users.Password);
            cmd.Parameters.AddWithValue("@Email", users.Email);
            sqlConnection.Open();
            int i = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Record updated succesfuly";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Some error occured. Try after sometimes .";

            }
            return response;
        }

        public Response addToCart(Cart cart, SqlConnection sqlConnection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_addToCart", sqlConnection);
            cmd.Parameters.AddWithValue("@UserId", cart.UserId);
            cmd.Parameters.AddWithValue("@UnitPrice", cart.UnitPrice);
            cmd.Parameters.AddWithValue("@Discount", cart.Discount);
            cmd.Parameters.AddWithValue("@Quantity", cart.Quantity);
            cmd.Parameters.AddWithValue("@TotalPrice", cart.TotalPrice);
            cmd.Parameters.AddWithValue("@MedicineID", cart.MedicineID);
            sqlConnection.Open();
            int i = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Item added  succesfuly";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Item Could not be added.";

            }
            return response;

        }

        public Response placeOrder(Users user, SqlConnection sqlConnection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_placeOrder", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", user.ID);
            sqlConnection.Open();
            int i = cmd.ExecuteNonQuery();
            sqlConnection.Close();

            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Order has been  succesfuly";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Order could  not be placed.";

            }

            return response;

        }

        public Response OrderList(Users user, SqlConnection sqlConnection)
        {
            Response response = new Response();
            List<Orders> list = new List<Orders>();
            SqlDataAdapter cmd = new SqlDataAdapter("sp_userOrderList", sqlConnection);
            cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
            cmd.SelectCommand.Parameters.AddWithValue("@Type", user.Type);
            cmd.SelectCommand.Parameters.AddWithValue("@ID", user.ID);
            DataTable dataTable = new DataTable();
            cmd.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Orders orders = new Orders();
                    orders.ID = Convert.ToInt32(dataTable.Rows[i]["ID"]);
                    orders.OrderNo = Convert.ToString(dataTable.Rows[i]["OrderNo"]);
                    orders.OrderTotal = Convert.ToDecimal(dataTable.Rows[i]["OrderTotal"]);

                    orders.OrderStatus = Convert.ToString(dataTable.Rows[i]["OrderStatus"]);
                    list.Add(orders);

                }
                if (list.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Order details fetched";
                    response.listOrders = null;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "Order details are not available.";
                    response.listOrders = null;
                }

            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Order details are not available.";
                response.listOrders = null;
            }

            return response;
        }

        public Response addUpdateMedicine(Medicines medicines, SqlConnection sqlConnection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_placeOrder", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", medicines.Name);
            cmd.Parameters.AddWithValue("@Manufacturer", medicines.Manufacturer);
            cmd.Parameters.AddWithValue("@UnitPrice", medicines.UnitPrice);
            cmd.Parameters.AddWithValue("@Discount", medicines.Discount);
            cmd.Parameters.AddWithValue("@Quantity", medicines.Quantity);
            cmd.Parameters.AddWithValue("@ExpDate", medicines.ExpDate);
            cmd.Parameters.AddWithValue("@ImageUrl", medicines.ImageUrl);
            cmd.Parameters.AddWithValue("@Status", medicines.Status);
            cmd.Parameters.AddWithValue("@Type", medicines.Type);

            sqlConnection.Open();
            int i = cmd.ExecuteNonQuery();
            sqlConnection.Close();

            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Medicines inserted succesfuly";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Medicines did not save . try again!";

            }

            return response;

        }

        public Response UserList( SqlConnection sqlConnection)
        {
            Response response = new Response();
            List<Users> listUsers = new List<Users>();
            SqlDataAdapter cmd = new SqlDataAdapter("sp_UserList", sqlConnection);
            cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
         
            DataTable dataTable = new DataTable();
            cmd.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Users users = new Users();
                    users.ID = Convert.ToInt32(dataTable.Rows[i]["ID"]);
                    users.FirstName = Convert.ToString(dataTable.Rows[i]["FirstName"]);
                    users.LastName = Convert.ToString(dataTable.Rows[i]["LastName"]);
                    users.Password = Convert.ToString(dataTable.Rows[i]["Password"]);
                    users.Email = Convert.ToString(dataTable.Rows[i]["Email"]);
                    users.Fund = Convert.ToDecimal(dataTable.Rows[i]["Fund"]);
                    users.Status = Convert.ToInt32(dataTable.Rows[i]["Status"]);
                    users.CreatedOn = Convert.ToDateTime(dataTable.Rows[i]["CreatedOn"]);
                    listUsers.Add(users);




                }
                if (listUsers.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "User details fetched";
                    response.listOrders = null;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "User details are not available.";
                    response.listOrders = null;
                }

            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "User details are not available.";
                response.listOrders = null;
            }

            return response;
        }

    }
}
