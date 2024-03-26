using EcommerceApplicationCasestudy.Models;
using EcommerceApplicationCasestudy.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplicationCasestudy.Exceptions
{
    internal class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(string message) : base(message)
        {


        }
        public static void Customernotfound(int Customer_id)
        {
            OrderProcessorRepository orderProcessorRepository = new OrderProcessorRepository();
            if (!orderProcessorRepository.CustomerNotExists(Customer_id))
                throw new CustomerNotFoundException("Customer not found!!!");


        }

        public static void InvalidCustomerData(Customer customer)
        {
            if (!customer.Email.Contains('@'))
            {
                throw new CustomerNotFoundException("Invalid Email");
            }
        }
    }
}

