using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace StaffDirectoryApplication.Pages.Staffs
{
    public class EditModel : PageModel
    {
        public StaffInfo staffInfo = new StaffInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try 
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=staffdirectory;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM staffs WHERE id=@id";
                    using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                staffInfo.id = "" + reader.GetInt32(0);
                                staffInfo.fname = reader.GetString(1);
                                staffInfo.lname = reader.GetString(2);
                                staffInfo.gender = reader.GetString(3);
                                staffInfo.dateofbirth = reader.GetDateTime(4).ToString("dd/MM/yyyy");
                                staffInfo.position = reader.GetString(5);
                                staffInfo.department = reader.GetString(6);
                                staffInfo.created_at = reader.GetDateTime(7).ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost() 
        {
            staffInfo.id = Request.Form["id"];
            staffInfo.fname = Request.Form["fname"];
            staffInfo.lname = Request.Form["lname"];
            staffInfo.gender = Request.Form["gender"];
            staffInfo.dateofbirth = Request.Form["dateofbirth"];
            staffInfo.position = Request.Form["position"];
            staffInfo.department = Request.Form["department"];

            if (staffInfo.fname.Length == 0 || staffInfo.lname.Length == 0 || staffInfo.gender.Length == 0 || staffInfo.dateofbirth.Length == 0 || staffInfo.position.Length == 0 || staffInfo.department.Length == 0)
            {
                errorMessage = "Please Fill In All The Required Fields";
                return;
            }

            try 
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=staffdirectory;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE staffs " + "SET fname=@fname, lname=@lname, gender=@gender, dateofbirth=@dateofbirth, position=@position, department=@department " + "WHERE id=@id";
                    using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@fname", staffInfo.fname);
                        sqlCommand.Parameters.AddWithValue("@lname", staffInfo.lname);
                        sqlCommand.Parameters.AddWithValue("@gender", staffInfo.gender);
                        /*sqlCommand.Parameters.AddWithValue("@dateofbirth", staffInfo.dateofbirth);*/
                        if (DateTime.TryParseExact(staffInfo.dateofbirth, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                        {
                            sqlCommand.Parameters.Add("@dateofbirth", SqlDbType.DateTime).Value = parsedDate;
                        }
                        sqlCommand.Parameters.AddWithValue("@position", staffInfo.position);
                        sqlCommand.Parameters.AddWithValue("@department", staffInfo.department);
                        sqlCommand.Parameters.AddWithValue("@id", staffInfo.id);

                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) 
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Staffs/Index");
        }
    }
}
