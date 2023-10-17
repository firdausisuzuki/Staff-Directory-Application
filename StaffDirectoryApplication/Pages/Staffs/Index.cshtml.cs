using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace StaffDirectoryApplication.Pages.Staffs
{
    public class IndexModel : PageModel
    {
        public List<StaffInfo> listStaffs = new List<StaffInfo>();

        public void OnGet(string searchName)
        {
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
                    String sql = "SELECT * FROM staffs";

                    if (!string.IsNullOrEmpty(searchName))
                    {
                        sql += " WHERE fname LIKE @SearchName OR lname LIKE @SearchName OR department LIKE @SearchName ORDER BY fname";
                    }

                    using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
                    {
                        if (!string.IsNullOrEmpty(searchName))
                        {
                            // Add a parameter for the search name
                            sqlCommand.Parameters.Add("@SearchName", SqlDbType.NVarChar).Value = "%" + searchName + "%";
                        }

                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StaffInfo staffInfo = new StaffInfo();
                                staffInfo.id = "" + reader.GetInt32(0);
                                staffInfo.fname = reader.GetString(1);
                                staffInfo.lname = reader.GetString(2);
                                staffInfo.gender = reader.GetString(3);
                                staffInfo.dateofbirth = reader.GetDateTime(4).ToString("dd/MM/yyyy");
                                staffInfo.position = reader.GetString(5);
                                staffInfo.department = reader.GetString(6);
                                staffInfo.created_at = reader.GetDateTime(7).ToString();

                                listStaffs.Add(staffInfo);
                            }
                        }
                    }
                    
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class StaffInfo
    {
        public String id;
        public String fname;
        public String lname;
        public String gender;
        public String dateofbirth;
        public String position;
        public String department;
        public String created_at;
    }
}
