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
        public bool PlaceOrder(Customer customer, List<Dictionary<Product, int>> productsAndquantity, string shippingAddress);
        List<Dictionary<Product, int>> GetOrdersByCustomer(int customerId);
    }
}
