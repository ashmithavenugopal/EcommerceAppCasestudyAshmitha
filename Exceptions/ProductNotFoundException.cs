using EcommerceApplicationCasestudy.Models;
using EcommerceApplicationCasestudy.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplicationCasestudy.Exceptions
{
    internal class ProductNotFoundException:Exception
    {
        public ProductNotFoundException(string message) : base(message)
        {


        }
        public static void ProductNotFound(int product_id)
        {
            OrderProcessorRepository orderProcessorRepository = new OrderProcessorRepository();
            if (!orderProcessorRepository.ProductNotFound(product_id))
                throw new ProductNotFoundException("Product not found!!!");


        }

        public static void InvalidProductData(Product product)
        {
            if (product.Price < 0)
            {
                throw new ProductNotFoundException("Invalid Price amount");
            }
        }

        public static void NotEnoughStock(int stockvalue, int productid)
        {
            Product product = new Product();
            OrderProcessorRepository orderProcessorRepository = new OrderProcessorRepository();
            if (stockvalue > orderProcessorRepository.availablestockquantity(productid))
            {
                throw new ProductNotFoundException("Not Enough stock");
            }
        }

        public static void productnotincart(int productid, int customerid)
        {

            OrderProcessorRepository orderProcessorRepository = new OrderProcessorRepository();
            if (!orderProcessorRepository.ProductNotExistinCart(productid, customerid))
                throw new ProductNotFoundException("Product not found in cart for the customer!!!");
        }
    }
}
