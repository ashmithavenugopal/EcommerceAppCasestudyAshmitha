using EcommerceApplicationCasestudy.Exceptions;
using EcommerceApplicationCasestudy.Models;
using EcommerceApplicationCasestudy.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplicationCasestudy.Service
{
    public class OrderProcessorService
    {
        private readonly OrderProcessorRepository orderprocessorrepobj;
        public OrderProcessorService()
        {

            orderprocessorrepobj = new OrderProcessorRepository();
        }

        

        public void AddCustomer(Customer customer)
        {
            try
            {                
                orderprocessorrepobj.CreateCustomer(customer);
            }

            catch (CustomerNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void Addproduct(Product product)
        {
            try
            {

                orderprocessorrepobj.CreateProduct(product);
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine(ex.Message);

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void DeleteCustomer(int customer)
        {
            try
            {
                orderprocessorrepobj.DeleteCustomer(customer);
            }
                         
            catch (CustomerNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteProduct(int product)
        {
            try
            {               
                orderprocessorrepobj.DeleteProduct(product);                
            }

            catch (ProductNotFoundException ex) 
            {
                Console.WriteLine(ex.Message);
                
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public bool Addtocart(Customer customer, Product product, int quantity)
        {
            try
            {               
                orderprocessorrepobj.AddToCart(customer, product, quantity);                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;

        }

        public void Deletefromcart(Customer customer, Product product)
        {
                
            try
            {
                orderprocessorrepobj.RemoveFromCart(customer, product);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
                                
        }

        public void DisplayCartRecord(Customer customerid)
        {
            try
            {
               
                List<Cart> cartList = orderprocessorrepobj.GetAllFromCart(customerid);
                Console.WriteLine("cartid\tcustomerid\tproductid\tquantity");
                foreach (Cart cart1 in cartList)
                {
                    Console.WriteLine(cart1.CartId + "\t" + cart1.CustomerId + "\t\t" + cart1.ProductId + "\t\t" + cart1.Quantity);
                }
                orderprocessorrepobj.GetAllFromCart(customerid);
            }
            catch (CustomerNotFoundException ex)
            {
                Console.WriteLine(ex.Message);

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool PlaceOrderservice(Customer customer, List<Dictionary<Product, int>> productsAndQuantities, string shippingAddress)
        {
            try
            {
                orderprocessorrepobj.PlaceOrder(customer, productsAndQuantities, shippingAddress);
            }
            catch (CustomerNotFoundException ex)
            {
                Console.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return true;
        }


        //public void GetOrdersByCustomerservice(int customerId)
        //{
        //    try
        //    {
        //        orderprocessorrepobj.GetOrdersByCustomer(customerId);

        //    }
        //    catch (OrderNotFoundException ex)
        //    {
        //        Console.WriteLine(ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
    }
}
