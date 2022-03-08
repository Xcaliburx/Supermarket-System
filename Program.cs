using ProjectCSharp1.Model;
using ProjectCSharp1.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCSharp1
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            startMenu();
        }

        ProductRepository prodRepo = new ProductRepository();
        TransactionRepository transRepo = new TransactionRepository();

        void startMenu()
        {
            int chooseMenu = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("Supermarket System");
                Console.WriteLine("==================");
                Console.WriteLine("1. Login as User");
                Console.WriteLine("2. Login as Admin");
                Console.WriteLine("3. Exit");
                Console.Write("Choice: ");
                chooseMenu = int.Parse(Console.ReadLine());
            } while (chooseMenu < 1 || chooseMenu> 3);

            switch (chooseMenu)
            {
                case 1:
                    User();
                    break;
                case 2:
                    Admin();
                    break;
                case 3:
                    Exit();
                    break;
            }
        }

        void User()
        {
            int chooseUser;
            do
            {
                Console.Clear();
                Console.WriteLine("Supermarket System");
                Console.WriteLine("==================");
                Console.WriteLine("1. View Product");
                Console.WriteLine("2. Buy Product");
                Console.WriteLine("3. Exit");
                Console.Write("Choice: ");
                chooseUser = int.Parse(Console.ReadLine());
            } while (chooseUser < 1 || chooseUser > 3);

            switch (chooseUser)
            {
                case 1:
                    viewProduct();
                    User();
                    break;
                case 2:
                    buyProduct();
                    User();
                    break;
                case 3:
                    startMenu();
                    break;
            }
        }

        void viewProduct()
        {
            Console.Clear();
            List<ProductModel> listProduct = prodRepo.viewProduct();

            if(listProduct.Count == 0)
            {
                Console.WriteLine("No product available!");
            }
            else
            {
                Console.WriteLine("View Product");
                Console.WriteLine("============");
                int i = 1;
                foreach(ProductModel prod in listProduct)
                {
                    Console.WriteLine("Product ID       : " + (i++));
                    Console.WriteLine("Product Name     : " + prod.Name);
                    Console.WriteLine("Product Quantity : " + prod.quantity);
                    Console.WriteLine("Price            : Rp "+ prod.price);
                    Console.WriteLine();
                }
            }
            Console.WriteLine("\nPress Enter to Continue...");
            Console.ReadLine();
        }

        void buyProduct()
        {
            List<ProductModel> listProduct = prodRepo.viewProduct();

            List<ProductModel> buyProduct = new List<ProductModel>();

            Console.Clear();
            if (listProduct.Count == 0)
            {
                Console.WriteLine("No product available!");
            }
            else
            {
                int num, id, quantity, price, totalSum = 0;
                string valid, payment;

                transRepo.insertTemp();

                Console.WriteLine("Buy Product");
                Console.WriteLine("===========");

                do
                {
                    listProduct = prodRepo.viewProduct();
                    do
                    {
                        Console.Write("Input Product ID [1-{0}]: ", listProduct.Count);
                        num = int.Parse(Console.ReadLine());
                    } while (num < 1 || num > listProduct.Count);

                    id = listProduct.ElementAt(num - 1).ID;
                    price = listProduct.ElementAt(num - 1).price;

                    do
                    {
                        Console.Write("Input Product Quantity [1-{0}]: ", listProduct.ElementAt(num - 1).quantity);
                        quantity = int.Parse(Console.ReadLine());
                    } while (quantity < 1 || quantity > listProduct.ElementAt(num - 1).quantity);

                    ProductModel prod = new ProductModel();
                    prod.ID = id;
                    prod.quantity = quantity;

                    transRepo.insertDetail(prod);
                    totalSum += (price * quantity);

                    do
                    {
                        Console.Write("Do you want to add another product ? [Yes | No]: ");
                        valid = Console.ReadLine();
                    } while (!(valid.Equals("Yes") || valid.Equals("No")));

                } while (valid.Equals("Yes"));

                do
                {
                    Console.Write("Choose payment method [Cash | Credit]: ");
                    payment = Console.ReadLine();
                } while (!(payment.Equals("Cash") || payment.Equals("Credit")));

                transRepo.insertHeader(payment);

                Console.WriteLine("Rp " + totalSum + " Sucessfully paid by " + payment + "!");
            }

            Console.WriteLine("\nPress Enter to Continue...");
            Console.ReadLine();
        }

        void Admin()
        {
            int chooseAdmin;
            do
            {
                Console.Clear();
                Console.WriteLine("Supermarket System");
                Console.WriteLine("==================");
                Console.WriteLine("1. Insert Product");
                Console.WriteLine("2. Update Product");
                Console.WriteLine("3. Delete Product");
                Console.WriteLine("4. View Product");
                Console.WriteLine("5. View Transaction");
                Console.WriteLine("6. Exit");
                Console.Write("Choice: ");
                chooseAdmin = int.Parse(Console.ReadLine());
            } while (chooseAdmin < 1 || chooseAdmin > 6);

            switch (chooseAdmin)
            {
                case 1:
                    insertProduct();
                    Admin();
                    break;
                case 2:
                    updateProduct();
                    Admin();
                    break;
                case 3:
                    deleteProduct();
                    Admin();
                    break;
                case 4:
                    viewProduct();
                    Admin();
                    break;
                case 5:
                    viewTransaction();
                    Admin();
                    break;
                case 6:
                    startMenu();
                    break;
            }
        }

        void insertProduct()
        {
            string name;
            int price, quantity;

            Console.Clear();
            Console.WriteLine("Insert Product");
            Console.WriteLine("==============");

            do
            {
                Console.Write("Input Product Name [Length between 5-20]: ");
                name = Console.ReadLine();
            } while (name.Length < 5 || name.Length > 20);

            do
            {
                Console.Write("Input Product Price [1000-1000000]: ");
                price = int.Parse(Console.ReadLine());
            } while (price < 1000 || price > 1000000);

            do
            {
                Console.Write("Input Product Quantity [1-1000]: ");
                quantity = int.Parse(Console.ReadLine());
            } while (quantity < 1 || quantity > 1000);

            ProductModel prod = new ProductModel();
            prod.Name = name;
            prod.price = price;
            prod.quantity = quantity;

            prodRepo.Insert(prod);

            Console.WriteLine("The product has been successfully inserted!");
            Console.WriteLine("\nPress Enter to Continue...");
            Console.ReadLine();
        }

        void updateProduct()
        {
            List<ProductModel> listProduct = prodRepo.viewProduct();

            Console.Clear();
            if (listProduct.Count == 0)
            {
                Console.WriteLine("No product available!");
            }
            else
            {
                string name;
                int price, quantity, num, id;
                Console.WriteLine("Update Product");
                Console.WriteLine("==============");

                do
                {
                    Console.Write("Input Product ID [1-{0}]: ", listProduct.Count);
                    num = int.Parse(Console.ReadLine());
                } while (num < 1 || num > listProduct.Count);

                id = listProduct.ElementAt(num - 1).ID;

                do
                {
                    Console.Write("Input Product Name [Length between 5-20]: ");
                    name = Console.ReadLine();
                } while (name.Length < 5 || name.Length > 20);

                do
                {
                    Console.Write("Input Product Price [1000-1000000]: ");
                    price = int.Parse(Console.ReadLine());
                } while (price < 1000 || price > 1000000);

                do
                {
                    Console.Write("Input Product Quantity [1-1000]: ");
                    quantity = int.Parse(Console.ReadLine());
                } while (quantity < 1 || quantity > 1000);

                ProductModel prod = new ProductModel();
                prod.ID = id;
                prod.Name = name;
                prod.price = price;
                prod.quantity = quantity;

                prodRepo.Update(prod);
                Console.WriteLine("The product has been successfully updated!");
            }

            Console.WriteLine("\nPress Enter to Continue...");
            Console.ReadLine();
        }

        void deleteProduct()
        {
            List<ProductModel> listProduct = prodRepo.viewProduct();

            Console.Clear();
            if (listProduct.Count == 0)
            {
                Console.WriteLine("No product available!");
            }
            else
            {
                int num, id;
                string valid;
                Console.WriteLine("Delete Product");
                Console.WriteLine("==============");

                do
                {
                    Console.Write("Input Product ID [1-{0}]: ", listProduct.Count);
                    num = int.Parse(Console.ReadLine());
                } while (num < 1 || num > listProduct.Count);

                id = listProduct.ElementAt(num - 1).ID;

                Console.WriteLine();

                Console.WriteLine("Product ID       : " + num);
                Console.WriteLine("Product Name     : " + listProduct.ElementAt(num - 1).Name);
                Console.WriteLine("Product Quantity : " + listProduct.ElementAt(num - 1).quantity);
                Console.WriteLine("Price            : Rp " + listProduct.ElementAt(num - 1).price);
                Console.WriteLine();

                do
                {
                    Console.Write("Are you sure you want to delete this product? [Yes | No]: ");
                    valid = Console.ReadLine();
                } while (!(valid.Equals("Yes") || valid.Equals("No")));

                if (valid.Equals("Yes"))
                {
                    prodRepo.Delete(id);
                    Console.WriteLine("The product has been successfully deleted!");
                }
                else
                {
                    Console.WriteLine("The product is not deleted");
                }

            }
            Console.WriteLine("\nPress Enter to Continue...");
            Console.ReadLine();
        }

        void viewTransaction()
        {
            List<TransactionModel> listTransaction = transRepo.viewTransaction();
            int checkID = -1, sumTotal = 0, num = 1;
            string method = "";

            Console.Clear();
            if (listTransaction.Count == 0)
            {
                Console.WriteLine("There is no transaction!");
            }
            else
            {
                Console.WriteLine("View Transaction");
                Console.WriteLine("================");
                foreach(TransactionModel trans in listTransaction)
                {
                    if(checkID == trans.transID)
                    {
                        Console.WriteLine("|{0,-3}| {1,-20} | {2,9}| {3,-8} |", num, trans.Name, trans.soldQty, (trans.price * trans.soldQty));
                        sumTotal += (trans.price * trans.soldQty);
                        num++;
                    }
                    else
                    {
                        if(checkID != -1)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Grand Total          : " + sumTotal + " by " + method);
                            sumTotal = 0;
                            num = 1;
                            Console.WriteLine();
                        }
                        Console.WriteLine("Transaction ID       : " + trans.transID);
                        Console.WriteLine("|No | Product Name         | Quantity | Price    |");
                        Console.WriteLine("|{0,-3}| {1,-20} | {2,9}| {3,-8} |", num, trans.Name, trans.soldQty, (trans.price * trans.soldQty));
                        num++;
                        checkID = trans.transID;
                        method = trans.payment;
                        sumTotal = (trans.price * trans.soldQty);
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Grand Total          : " + sumTotal + " by " + method);
                Console.WriteLine();
            }
            Console.WriteLine("\nPress Enter to Continue...");
            Console.ReadLine();
        }

        void Exit()
        {
            Console.Clear();
            return;
        }
    }
}
