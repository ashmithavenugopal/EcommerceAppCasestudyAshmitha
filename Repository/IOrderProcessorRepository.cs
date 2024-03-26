using EcommerceApplicationCasestudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplicationCasestudy.Repository
{
    public interface IOrderProcessorRepository
    {
        bool CreateProduct(Product product);
        bool CreateCustomer(Customer customer);
        bool DeleteProduct(int productId);
        bool DeleteCustomer(int customerId);
        bool AddToCart(Customer customer, Product products, int quantity);
        bool RemoveFromCart(Customer customer, Product products);
        List<Cart> GetAllFromCart(Customer customer);
        bool PlaceOrder(Customer customer, Order order, OrderItem orderitem, Product product);
        List<OrderItem> GetOrderByCustomer(Order order);
    }
}
