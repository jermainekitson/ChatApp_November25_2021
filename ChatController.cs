using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace ChatApp
{
    public class ChatController
    {
        private string connectionString = "";
        SqlConnection conn;

        public ChatController(string connString)
        {
            this.connectionString = connString;
        }

        public DataTable GetMessages()
        {
            DataTable ret = new DataTable();



            string getAllMessages = "SELECT USERNAME +': ' + CHATMESSAGE [ChatMessage] FROM Chat ORDER BY POSTED";
            conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(getAllMessages, conn);
                //Create the adapter, send it the command
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);



                //Fill the datatable with the results of our query
                adapter.Fill(ret);




                //cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return ret;
        }
        public void SendMessage(string msg, string username)
        {
            // string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string messageInsert = $@"
            INSERT INTO CHAT
            VALUES (
            @user,
            @message,
            @chatmessageDate)
            ";

            conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(messageInsert, conn);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@message", msg);
                cmd.Parameters.AddWithValue("@chatmessageDate", 
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));


                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        //public void SendMessage_Old(string msg, string username)
        //{
        //    // string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        //    string messageInsert = $@"
        //    INSERT INTO CHAT
        //    VALUES (
        //    '{username}',
        //    '{msg}',
        //    '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')
        //    ";

        //    conn = new SqlConnection(connectionString);
        //    try
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(messageInsert, conn);
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        public void CreateSchema()
        {
            string createTableQuery = @"
            CREATE TABLE USERS (
            [USERNAME] varchar(25) PRIMARY KEY,
            [Password] varchar(64),
            [REGISTER_DATE][DATETIME],
            [USER_AVATAR] VARBINARY(MAX)

                );

            CREATE TABLE CHAT (
            [USERNAME] varchar(25) FOREIGN KEY REFERENCES USERS(USERNAME),
            [CHATMESSAGE] varchar(100),
            [POSTED][DATETIME]
                );
            
            ";

            conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(createTableQuery, conn);

            try
            {
                conn.Open();
                //Use nonquery, when you dont expect results back
                cmd.ExecuteNonQuery();
            }
            //Jumps to catch only during exception
            catch(Exception ex)
            {
                throw;
            }
            //Always jumps to finally
            finally
            {
                //No matter what happens, close the connection
                conn.Close();
            }
        }
        public bool RegisterUser(string username, string password, Bitmap bmp)
        {
            bool ret = false;
            string insertUserQuery = $@"
            INSERT INTO USERS
            VALUES (@username,
            CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @password), 2),
            @registerDate,
            @theImage
            )
            ";

            //create a byte array
            byte[] imageBytes;
            //use a memory stream to convert the bitmap into an array of bytes
            using (MemoryStream stream = new MemoryStream()) 
            {
                bmp.Save(stream, ImageFormat.Bmp);
                imageBytes = stream.ToArray();
            }

                try
                {
                    conn = new SqlConnection(connectionString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(insertUserQuery, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@registerDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@theImage", imageBytes);
                    cmd.ExecuteNonQuery();
                    ret = true;
                }
                catch (Exception ex)
                {
                    ret = false;
                }
                finally
                {
                    conn.Close();
                }
            return ret;
        }
        public Bitmap GetAvatarByUsername (string username)
        {
            Bitmap ret;
            string searchUseravatarQuery = @"SELECT [USER_AVATAR]
            FROM [USERS]
            WHERE username = @username";
            conn = new SqlConnection(connectionString);
            try
            {
                
                conn.Open();
                SqlCommand cmd = new SqlCommand(searchUseravatarQuery, conn);
                cmd.Parameters.AddWithValue("@username", username);
                byte[] ImageBytes;
                ImageBytes = (byte[])cmd.ExecuteScalar();
                using (MemoryStream stream = new MemoryStream(ImageBytes))
                {
                    ret = new Bitmap(stream);
                }
                //cmd.ExecuteNonQuery();
                //ret = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return ret;
        }
        public bool LoginUser(string username, string password)
        {
            bool ret = false;
            string loginUserQuery = $@"SELECT COUNT(*) from USERS
            WHERE username = @username
                and password = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256',@password), 2)
            ";
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                //bring back a single value with executescalar
                SqlCommand cmd = new SqlCommand(loginUserQuery, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                
                int countRecord = (int)cmd.ExecuteScalar();
                if (countRecord > 0)
                {
                    //login worked
                    ret = true;
                }
                else
                {
                    //login failed
                    return false;
                }

            }
            catch
            {
                ret = false;
            }
            finally
            {
                conn.Close();
            }
            return ret;
        }

        public DataTable GetUsers()
        {
            DataTable ret = new DataTable();



            string getAllMessages = "SELECT USERNAME FROM USERS";
            conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(getAllMessages, conn);
                //Create the adapter, send it the command
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);



                //Fill the datatable with the results of our query
                adapter.Fill(ret);




                //cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return ret;
        }


    }
}
