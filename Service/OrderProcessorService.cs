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

        public void Addproduct(Product product)
        {
            try
            {
                ProductNotFoundException.InvalidProductData(product);
                orderprocessorrepobj.CreateProduct (product);
                Console.WriteLine("Record inserted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void AddCustomer(Customer customer)
        {
            try
            {
                CustomerNotFoundException.InvalidCustomerData(customer);
                orderprocessorrepobj.CreateCustomer(customer);
                Console.WriteLine("Customer Records inserted successfully");
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
                CustomerNotFoundException.Customernotfound(customer);
                orderprocessorrepobj.DeleteCustomer(customer);
                Console.WriteLine("Customer Records deleted successfully");
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
                ProductNotFoundException.ProductNotFound(product);
                orderprocessorrepobj.DeleteProduct(product);
                Console.WriteLine("Product Records deleted successfully");
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
                ProductNotFoundException.ProductNotFound(product.ProductId);
                ProductNotFoundException.NotEnoughStock(quantity, product.ProductId);
                orderprocessorrepobj.AddToCart(customer, product, quantity);
                Console.WriteLine("Product added to cart ");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;

        }

        public void Deletefromcart(Customer customer, Product product)
        {
                CustomerNotFoundException.Customernotfound(customer.CustomerId);
                ProductNotFoundException.ProductNotFound(product.ProductId);
                orderprocessorrepobj.RemoveFromCart(customer, product);
                Console.WriteLine("Product deleted from cart");
        }

        public void DisplayCartRecord(Customer customerid)
        {
            try
            {
                CustomerNotFoundException.Customernotfound(customerid.CustomerId);
                List<Cart> cartList = orderprocessorrepobj.GetAllFromCart(customerid);
                Console.WriteLine("cartid\tcustomerid\tproductid\tquantity");
                foreach (Cart cart1 in cartList)
                {
                    Console.WriteLine(cart1.CartId + "\t" + cart1.CustomerId + "\t\t" + cart1.ProductId + "\t\t" + cart1.Quantity);
                }
                orderprocessorrepobj.GetAllFromCart(customerid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public bool Placeorderservice(Customer customer, Order order, OrderItem orderitem, Product product)
        {
            try
            {
                CustomerNotFoundException.Customernotfound(customer.CustomerId);
                ProductNotFoundException.ProductNotFound(product.ProductId);
                ProductNotFoundException.NotEnoughStock(orderitem.Quantity, product.ProductId);
                orderprocessorrepobj.PlaceOrder(customer, order, orderitem, product);
                Console.WriteLine("Order placed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
                
        }
        public void DisplayOrderbyCustomer(Order order)
        {
            try
            {
                //OrderNotFoundException.ordernotfound(order.OrderId);
                List<OrderItem> orderList = orderprocessorrepobj.GetOrderByCustomer(order);
                Console.WriteLine("Productid\tQuantity");
                foreach (OrderItem list in orderList)
                {
                    Console.WriteLine(list.ProductId + "\t\t" + list.Quantity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


    }
}
