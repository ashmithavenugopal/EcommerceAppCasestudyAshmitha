using EcommerceApplicationCasestudy.Models;
using EcommerceApplicationCasestudy.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplicationCasestudy.Exceptions
{
    internal class OrderNotFoundException : Exception
    {
        public OrderNotFoundException(string message) : base(message)
        {


        }
        public static void ordernotfound(int order_id)
        {
            OrderProcessorRepository orderProcessorRepository = new OrderProcessorRepository();
            if (!orderProcessorRepository.OrderNotExist(order_id))
                throw new OrderNotFoundException("Order not found!!!");


        }
    }
}
