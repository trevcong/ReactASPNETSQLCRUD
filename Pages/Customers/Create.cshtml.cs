using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ReactASPNETSQLCRUD.Pages.Customers
{
    public class Create : PageModel
    {
       [BindProperty, Required(ErrorMessage = "First Name is required")]
       public string FirstName {get; set;} = "";
       [BindProperty, Required(ErrorMessage = "Last Name is required")]
        public string LastName {get; set;} = "";
        [BindProperty, Required, EmailAddress]
        public string Email {get; set;} = "";
        [BindProperty, Phone]
        public string? Phone {get; set;} 
        [BindProperty]
        public string? Address {get; set;} 
        [BindProperty, Required]
        public string Company {get; set;} = "";
        [BindProperty]
        public string? Notes {get; set;}
        public void OnGet()
        {
        }

        public string ErrorMessage {get; set;} = "";
         public void OnPost()
        {
            if (!ModelState.IsValid){
                return;
            }

            if (Phone == null) Phone = "";
            if (Address == null) Address = "";
            if (Notes == null) Notes = "";

            //Create new customer 
            try{
                string connectionString = "Server=.;Database=crmdb;Trusted_Connection=True;TrustServerCertificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString)){
                    connection.Open();

                    string sql = "INSERT INTO customers " + 
                    "(FirstName, LastName, email, phone, address, company, notes) VALUES " +
                    "(@FirstName, @LastName, @Email, @Phone, @Address, @Company, @Notes)";

                    using (SqlCommand command = new SqlCommand(sql, connection)){
                        command.Parameters.AddWithValue("@firstname", FirstName);
                        command.Parameters.AddWithValue("@LastName", LastName);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Phone", Phone);
                        command.Parameters.AddWithValue("@Address", Address);
                        command.Parameters.AddWithValue("@Company", Company);
                        command.Parameters.AddWithValue("@Notes", Notes);

                        command.ExecuteNonQuery();
                    }

                }
            }catch(Exception ex){
                ErrorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Customers/Index");
            
        }
    }
}