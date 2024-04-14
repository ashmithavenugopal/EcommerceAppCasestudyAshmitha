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
   
    }
}

