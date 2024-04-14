using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplicationCasestudy.Models
{
    public class Customer
    {
        private int customerId;
        private string name;
        private string email;
        private string password;

        // Customer_id (Primary Key)
        public int CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    
        public Customer() 
        { 

        }

        public Customer(int customerId, string name, string email, string password)
        {
            CustomerId = customerId;
            Name = name;
            Email = email;
            Password = password;
        }

        public override string ToString()
        {
            return $"{CustomerId} {Name} {Email} {Password}";
        }


    }
  
}
