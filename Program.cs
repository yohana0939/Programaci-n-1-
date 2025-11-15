//Name of student: Yohana Montero
//Registration: 2025-0939
// Day of the class: Friday 


using System;
using System.Collections.Generic;

public class Contact
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }

    public Contact(int id, string fullName, string phone, string email, string address)
    {
        Id = id;
        FullName = fullName;
        Phone = phone;
        Email = email;
        Address = address;
    }
}

public class ContactManager
{
    private List<Contact> contacts = new List<Contact>();

    public void AddContact()
    {
        Console.WriteLine("\nLet's add a new contact.");

        int id = contacts.Count + 1;

        Console.Write("Enter Name: ");
        string name = Console.ReadLine();

        Console.Write("Enter Phone: ");
        string phone = Console.ReadLine();

        Console.Write("Enter Email: ");
        string email = Console.ReadLine();

        Console.Write("Enter Address: ");
        string address = Console.ReadLine();

        contacts.Add(new Contact(id, name, phone, email, address));

        Console.WriteLine("\nContact successfully added!\n");
    }

    public void ViewContacts()
    {
        Console.WriteLine("\nID     Name          Phone          Email          Address");
        Console.WriteLine("--------------------------------------------------------------");

        foreach (var c in contacts)
        {
            Console.WriteLine($"{c.Id}    {c.FullName}    {c.Phone}    {c.Email}    {c.Address}");
        }
        Console.WriteLine();
    }

    public void SearchContact()
    {
        ViewContacts();
        Console.Write("Enter ID to search: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Contact found = contacts.Find(c => c.Id == id);

        if (found == null)
        {
            Console.WriteLine("\nContact not found.\n");
            return;
        }

        Console.WriteLine($"\nName: {found.FullName}");
        Console.WriteLine($"Phone: {found.Phone}");
        Console.WriteLine($"Email: {found.Email}");
        Console.WriteLine($"Address: {found.Address}\n");
    }

    public void EditContact()
    {
        ViewContacts();
        Console.Write("Enter ID to edit: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Contact found = contacts.Find(c => c.Id == id);

        if (found == null)
        {
            Console.WriteLine("\nContact not found.\n");
            return;
        }

        Console.WriteLine("\nLeave empty to keep current value.\n");

        Console.Write($"Current Name ({found.FullName}): ");
        string newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName)) found.FullName = newName;

        Console.Write($"Current Phone ({found.Phone}): ");
        string newPhone = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newPhone)) found.Phone = newPhone;

        Console.Write($"Current Email ({found.Email}): ");
        string newEmail = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newEmail)) found.Email = newEmail;

        Console.Write($"Current Address ({found.Address}): ");
        string newAddress = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newAddress)) found.Address = newAddress;

        Console.WriteLine("\nContact successfully updated!\n");
    }

    public void DeleteContact()
    {
        ViewContacts();
        Console.Write("Enter ID to delete: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Contact found = contacts.Find(c => c.Id == id);

        if (found == null)
        {
            Console.WriteLine("\nContact not found.\n");
            return;
        }

        Console.WriteLine("Are you sure? 1 = Yes, 2 = No");
        int option = Convert.ToInt32(Console.ReadLine());

        if (option == 1)
        {
            contacts.Remove(found);
            Console.WriteLine("\nContact removed!\n");
        }
    }
}


class Program
{
    static void Main(string[] args)
    {
        ContactManager manager = new ContactManager();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n===== CONTACT MENU =====");
            Console.WriteLine("1. Add Contact");
            Console.WriteLine("2. View Contacts");
            Console.WriteLine("3. Search Contact");
            Console.WriteLine("4. Edit Contact");
            Console.WriteLine("5. Delete Contact");
            Console.WriteLine("6. Exit");
            Console.WriteLine("=================================");
            Console.Write("Choose an option: ");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    manager.AddContact();
                    break;
                case 2:
                    manager.ViewContacts();
                    break;
                case 3:
                    manager.SearchContact();
                    break;
                case 4:
                    manager.EditContact();
                    break;
                case 5:
                    manager.DeleteContact();
                    break;
                case 6:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}