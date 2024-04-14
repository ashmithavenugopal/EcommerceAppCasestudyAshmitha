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
        Product product = new Product();
        Customer customer = new Customer();
        OrderItem orderItem = new OrderItem();
        Order order = new Order();
        OrderProcessorService orderProcessorService = new OrderProcessorService();
        public void Mainmenu()
        {
            bool mainmenu = true;
            while (mainmenu)
            {
                Console.WriteLine("Main Menu");
                Console.WriteLine("1. Admin ");
                Console.WriteLine("2. User / Customer");
                Console.WriteLine("0. Exit");
                Console.WriteLine("Enter your choice");


                int Mmchoice = int.Parse(Console.ReadLine());
                switch(Mmchoice)
                {
                        case 1:
                        Console.WriteLine("Enter PASSWORD: ");
                        string password = Console.ReadLine();
                        if (password == "admin")
                        {
                            Console.WriteLine("Logged in successfully ");
                            Adminmenu();
                        }
                        else
                        {
                            Console.WriteLine("Incorrect password");
                        }
                        break;
                        case 2:

                        Customer();

                        break;
                        case 0:

                        mainmenu = false;

                        break;
                    default: 
                        Console.WriteLine("Try Again");
                        break;
                }
            }
        }
        public void Adminmenu()
        {
            bool adminchoice = true;
            while (adminchoice)
            {
                Console.WriteLine("ADMIN OPERATIONS ");
                Console.WriteLine("1. Create Product ");
                Console.WriteLine("2. Create Customer ");
                Console.WriteLine("3. Delete Product ");
                Console.WriteLine("4. Delete Customer ");
                Console.WriteLine("0 for Exit");
                Console.WriteLine("Enter your choice");

                int adchoice = int.Parse(Console.ReadLine());
                switch (adchoice)
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

                        Console.WriteLine("Enter Product_id");
                        int prodid = int.Parse(Console.ReadLine());
                        orderProcessorService.DeleteProduct(prodid);

                        break;
                    case 4:

                        Console.WriteLine("Enter Customer_id");
                        int custid = int.Parse(Console.ReadLine());
                        orderProcessorService.DeleteCustomer(custid);

                        break;
                    case 0:

                        adminchoice = false;

                        break;
                    default:

                        Console.WriteLine("Try again");

                        break;
                              
                }
            }
        }
        public void Customer()
        {
            bool customerchoice = true;
            while (customerchoice)
            {
                Console.WriteLine("1. REGISTER ");
                Console.WriteLine("2. LOGIN ");
                Console.WriteLine("Enter your choice");
                int custchoice = int.Parse(Console.ReadLine());
                switch (custchoice)
                {
                    case 1:
                        Console.WriteLine("Enter Name: ");
                        string cname = Console.ReadLine();
                        Console.WriteLine("Enter Email: ");
                        string email = Console.ReadLine();
                        Console.WriteLine("Enter password : ");
                        string password = Console.ReadLine();
                        customer = new Customer() { Name = cname, Email = email, Password = password };
                        orderProcessorService.AddCustomer(customer);
                        break;
                    case 2:
                        Console.WriteLine("Enter Email: ");
                        string LoginEmail = Console.ReadLine();
                        Console.WriteLine("Enter password : ");
                        string LoginPassword = Console.ReadLine();
                        customer = new Customer()
                        {
                            Name = LoginEmail,
                            Password = LoginPassword
                        };
                        if (orderprocessrepo.CheckCustomerCredentials(LoginEmail, LoginPassword))
                        {
                            Console.WriteLine("Login successful!");                           
                            Thread.Sleep(2000);
                            Console.Clear();
                            CustomerMenu(LoginEmail , LoginPassword);
                        }
                        else
                        {
                            Console.WriteLine("Invalid login credentials.");
                            Thread.Sleep(2000);
                        }                     
                        break;                    
                }
            }
        }
        public void CustomerMenu(string email , string password)
        {
            
            bool executeagain = true;
            while (executeagain)
            {
               
                Console.WriteLine("1. Add to Cart ");
                Console.WriteLine("2. Remove from Cart ");
                Console.WriteLine("3. Get All from Cart ");
                Console.WriteLine("4. Place Order ");
                Console.WriteLine("5. Get all orders by Customer ");
                Console.WriteLine("Enter your choice");
                int Menu = int.Parse(Console.ReadLine());
                int LogID = orderprocessrepo.CurrentLoggedInCId(email,password);
                
                switch (Menu)
                {
                                                        
                    case 1:

                        List<Product> productname = orderprocessrepo.ShowProductList();
                        Console.WriteLine("  Available   Products      ");
                        foreach (Product product1 in productname)
                        {
                            Console.WriteLine(product1.Name + "\t\t" + product1.ProductId + "\t\t" + product1.Price);
                        }


                        int customerid = LogID;
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

                        Console.WriteLine("Add more products to Cart , press 1 else 0 ");

                        int addmore = int.Parse(Console.ReadLine());
                        while (addmore == 1)
                        {

                            switch (addmore)
                            {

                                case 1:

                                    int nextcustid = LogID;
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
                    case 2:
                    
                        int deletecustomerid = LogID;
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
                    case 3:

                        int cartcustomerid = LogID;
                        customer = new Customer()
                        {
                            CustomerId = cartcustomerid
                        };

                        orderProcessorService.DisplayCartRecord(customer);
                        break;

                    case 4:
                      
                        int customerId = LogID;
                        customer = new Customer() 
                        {
                            CustomerId = customerId 
                        };

                        Console.WriteLine("Enter shipping address:");
                        string shippingAddress = Console.ReadLine();

                        // Fetch cart items for the customer
                        List<Cart> cartItems = orderprocessrepo.GetAllFromCart(customer);

                        // Prepare productsAndQuantities list using cart items
                        List<Dictionary<Product, int>> productsAndQuantities = new List<Dictionary<Product, int>>();
                        foreach (var cartItem in cartItems)
                        {
                            // Prompt user to confirm adding each product to the order
                            Console.WriteLine($"Add product '{cartItem.ProductId}' to order? (yes/no)");
                            string response = Console.ReadLine().ToLower();

                            if (response == "yes")
                            {
                                // Create dictionary entry for the product and its quantity from the cart
                                Dictionary<Product, int> productQuantityPair = new Dictionary<Product, int>();
                                Product product1 = new Product() 
                                {
                                    ProductId = cartItem.ProductId
                                }; // Assuming Product class has appropriate properties
                                productQuantityPair.Add(product1, cartItem.Quantity.Value);
                                productsAndQuantities.Add(productQuantityPair);
                            }
                        }
                        // Call the PlaceOrder method with the gathered input
                        bool orderPlaced = orderProcessorService.PlaceOrderservice(customer, productsAndQuantities, shippingAddress);
                        if (orderPlaced)
                        {
                            Console.WriteLine("Order placed successfully ");

                        }
                        else
                        {
                            Console.WriteLine("Try Again");
                        }

                        break;
                    case 5:

                        int cid = LogID;
                        List<Dictionary<Product, int>> orders = orderprocessrepo.GetOrdersByCustomer(cid);
                        if (orders.Count > 0)
                        {
                            Console.WriteLine($"Orders for customer {cid}:");
                            foreach (var orderitem in orders)
                            {
                                Console.WriteLine("Order:");
                                foreach (var kvp in orderitem)
                                {
                                    Console.WriteLine($"Product: {kvp.Key.Name}, Quantity: {kvp.Value}");
                                }
                                Console.WriteLine();
                            }

                            int Totalprice = orderprocessrepo.GetTotalPriceOfCustomerId(LogID);
                            Console.WriteLine("The total Price of your orders: Rs. " + Totalprice);
                           
                        }
                        else
                        {
                            Console.WriteLine($"No orders found for customer {cid}");
                        }
                        break;


                    default:
                        Console.WriteLine("Try again");
                        break;
                }
            }            
        }
    }
}
