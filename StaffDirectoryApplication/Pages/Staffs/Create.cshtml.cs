using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace StaffDirectoryApplication.Pages.Staffs
{
    public class CreateModel : PageModel
    {
        public StaffInfo staffInfo = new StaffInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
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
                IConfiguration configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .Build();
                String connectionString = configuration["ConnectionStrings:MyDatabase"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO staffs " + "(fname, lname, gender, dateofbirth, position, department) VALUES " + "(@fname, @lname, @gender, @dateofbirth, @position, @department)";
                    using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("fname", staffInfo.fname);
                        sqlCommand.Parameters.AddWithValue("lname", staffInfo.lname);
                        sqlCommand.Parameters.AddWithValue("gender", staffInfo.gender);
                        /*sqlCommand.Parameters.AddWithValue("dateofbirth", staffInfo.dateofbirth);*/
                        if (DateTime.TryParseExact(staffInfo.dateofbirth, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                        {
                            sqlCommand.Parameters.AddWithValue("@dateofbirth", SqlDbType.DateTime).Value = parsedDate;
                        }
                        sqlCommand.Parameters.AddWithValue("position", staffInfo.position);
                        sqlCommand.Parameters.AddWithValue("department", staffInfo.department);

                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            staffInfo.fname = "";staffInfo.lname = "";staffInfo.gender = "";staffInfo.dateofbirth = "";staffInfo.position = "";staffInfo.department = "";
            successMessage = "New Staff Has Been Added";

            Response.Redirect("/Staffs/Index");
        }
    }
}
