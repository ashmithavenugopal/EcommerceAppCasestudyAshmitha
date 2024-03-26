using EcommerceApplicationCasestudy.Models;
using EcommerceApplicationCasestudy.Repository;
using EcommerceApplicationCasestudy.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApplicationCasestudy.EcommerceApplication
{
    internal class EcommApp
    {
        

        readonly OrderProcessorRepository orderprocessrepo;

        public EcommApp()
        {
            orderprocessrepo = new OrderProcessorRepository();
        }
        public void Menu()
        {
            bool executeagain = true;
            while (executeagain)
            {

                OrderProcessorService orderProcessorService = new OrderProcessorService();
                Console.WriteLine("Ecommerce");
                Console.WriteLine("1.Add Product in product table");
                Console.WriteLine("2.Add Customer in customer table");
                Console.WriteLine("3.Delete Customer from Customer table");
                Console.WriteLine("4.Delete Product from Product table");
                Console.WriteLine("5.Add Product to cart table");
                Console.WriteLine("6.Delete Product from cart table");
                Console.WriteLine("7.Display cart");
                Console.WriteLine("8.Place Order");
                Console.WriteLine("9.View Customer Order");
                Console.WriteLine("Enter your Input");
                int Menu = int.Parse(Console.ReadLine());
                Product product = new Product();
                Customer customer = new Customer();
                OrderItem orderItem = new OrderItem();
                Order order = new Order();
                switch (Menu)
                {
                    case 1:
        
                        Console.WriteLine("Enter Product Name: ");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter Product price: ");
                        int price = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Product description : ");
                        string desc = Console.ReadLine();
                        Console.WriteLine("Enter stock qunatity: ");
                        int sq = int.Parse(Console.ReadLine());
                        product = new Product() { Name = name, Price = price, Description = desc, StockQuantity = sq };
                        orderProcessorService.Addproduct(product);

                        break;

                    case 2:

                        Console.WriteLine("Enter Name: ");
                        string cname = Console.ReadLine();
                        Console.WriteLine("Enter Email: ");
                        string email = Console.ReadLine();
                        Console.WriteLine("Enter password : ");
                        string password = Console.ReadLine();

                        customer = new Customer() { Name = cname, Email = email, Password = password };
                        orderProcessorService.AddCustomer(customer);

                        break;

                    case 3:
                        Console.WriteLine("Enter Customer_id");
                        int custid = int.Parse(Console.ReadLine());
                        orderProcessorService.DeleteCustomer(custid);
                        break;
                    case 4:
                        Console.WriteLine("Enter Product_id");
                        int prodid = int.Parse(Console.ReadLine());
                        orderProcessorService.DeleteProduct(prodid);
                        break;
                    case 5:
                        
                        Console.WriteLine("enter the customer id:");
                        int customerid = int.Parse(Console.ReadLine());
                        Console.WriteLine("enter the product id:");
                        int productid = int.Parse(Console.ReadLine());
                        Console.WriteLine("enter the quantity:");
                        int quantity = int.Parse(Console.ReadLine());
                        
                        customer = new Customer()
                        {
                            CustomerId = customerid,
                        };
                        product = new Product()
                        {
                            ProductId = productid,
                        };
                        orderProcessorService.Addtocart(customer, product, quantity);

                        Console.WriteLine("Want to add more product to cart then press 1 else 0 ");

                        int addmore = int.Parse(Console.ReadLine());
                        while (addmore == 1)
                        {

                            switch (addmore)
                            {

                                case 1:
                                   
                                    Console.WriteLine("enter the customer id:");
                                    int nextcustid = int.Parse(Console.ReadLine());
                                    Console.WriteLine("enter the product id:");
                                    int nextprodid = int.Parse(Console.ReadLine());
                                    Console.WriteLine("enter the quantity:");
                                    int addquantity = int.Parse(Console.ReadLine());
                                   

                                    customer = new Customer()
                                    {
                                        CustomerId = nextcustid,
                                    };
                                    product = new Product()
                                    {
                                        ProductId = nextprodid,
                                    };
                                    orderProcessorService.Addtocart( customer, product, addquantity);
                                    Console.WriteLine("do you want more then press 1");
                                    int over = int.Parse(Console.ReadLine());
                                    addmore = over;
                                    break;
                            }
                        }
                        break;
                    case 6:
                        Console.WriteLine("enter the customer id:");
                        int deletecustomerid = int.Parse(Console.ReadLine());
                        Console.WriteLine("enter the product id:");
                        int deleteproductid = int.Parse(Console.ReadLine());
                        customer = new Customer()
                        {
                            CustomerId = deletecustomerid
                        };
                        product = new Product()
                        {
                            ProductId = deleteproductid
                        };
                        orderProcessorService.Deletefromcart(customer, product);
                        break;
                    case 7:
                        Console.WriteLine("Enter Customer id:");
                        int cartcustomerid = int.Parse(Console.ReadLine());
                        customer = new Customer()
                        {
                            CustomerId = cartcustomerid
                        };

                        orderProcessorService.DisplayCartRecord(customer);
                        break;
                    case 8:
                        OrderProcessorRepository orderProcessorRepository = new OrderProcessorRepository();

                        Console.WriteLine("Enter Customer id:");
                        int customeridorder = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Product id:");
                        int productidorder = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter quantity:");
                        int quantityorder = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Address:");
                        string address = Console.ReadLine();

                        customer = new Customer()
                        {
                            CustomerId = customeridorder
                        };
                        product = new Product()
                        {
                            ProductId = productidorder
                        };
                        orderItem= new OrderItem()
                        {
                            Quantity = quantityorder
                        };

                        order = new Order()
                        {
                            ShippingAddress = address
                        };

                        orderProcessorService.Placeorderservice(customer, order, orderItem, product);
                        break;
                    case 9:

                        Console.WriteLine("Enter Customer id:");
                        int cust = int.Parse(Console.ReadLine());
                        order = new Order()
                        {
                            CustomerId = cust
                        };
                        orderProcessorService.DisplayOrderbyCustomer(order);

                        break;

                    default:
                        Console.WriteLine("Try again");
                        break;

                }
            }            
        }
    }
}
