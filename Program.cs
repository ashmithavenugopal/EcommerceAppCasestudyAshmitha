using EcommerceApplicationCasestudy.EcommerceApplication;
using EcommerceApplicationCasestudy.Models;
using EcommerceApplicationCasestudy.Repository;
using EcommerceApplicationCasestudy.Service;

namespace EcommerceApplicationCasestudy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Customer customers = new Customer();
            Order order = new Order();
            OrderItem item = new OrderItem();
            Product product = new Product();
            Cart cart = new Cart();

            EcommApp ecommApp = new EcommApp();

            ecommApp.Menu();
           
        }
    }
}
