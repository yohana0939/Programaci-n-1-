// Name of student: Yohana Montero
// Student ID: 2025-0939
//Class day: Friday 6PM
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to my contact list");

        // Dictionaries and IDs list
        bool running = true;
        List<int> ids = new List<int>();
        Dictionary<int, string> names = new Dictionary<int, string>();
        Dictionary<int, string> lastnames = new Dictionary<int, string>();
        Dictionary<int, string> addresses = new Dictionary<int, string>();
        Dictionary<int, string> telephones = new Dictionary<int, string>();
        Dictionary<int, string> emails = new Dictionary<int, string>();
        Dictionary<int, int> ages = new Dictionary<int, int>();
        Dictionary<int, bool> bestFriends = new Dictionary<int, bool>();

        while (running)
        {
            Console.WriteLine(@"
1. Add contact
2. See contacts
3. Search contact
4. Modify contact
5. Delete contact
6. Exit
");

            Console.Write("Enter the number of the desired option: ");
            string input = Console.ReadLine();
            int typeOption;

            if (!int.TryParse(input, out typeOption))
            {
                Console.WriteLine("Please enter a valid number.\n");
                continue;
            }

            switch (typeOption)
            {
                case 1:
                    AddContact(ids, names, lastnames, addresses, telephones, emails, ages, bestFriends);
                    break;

                case 2:
                    ShowContacts(ids, names, lastnames, addresses, telephones, emails, ages, bestFriends);
                    break;

                case 3:
                    SearchContact(ids, names, lastnames, telephones);
                    break;

                case 4:
                    ModifyContact(ids, names, lastnames, addresses, telephones, emails, ages, bestFriends);
                    break;

                case 5:
                    DeleteContact(ids, names, lastnames, addresses, telephones, emails, ages, bestFriends);
                    break;

                case 6:
                    running = false;
                    Console.WriteLine("Exiting the program. Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid option. Try again.\n");
                    break;
            }
        }
    }

    // ------------------ METHODS ------------------

    static void AddContact(List<int> ids, Dictionary<int, string> names, Dictionary<int, string> lastnames,
        Dictionary<int, string> addresses, Dictionary<int, string> telephones, Dictionary<int, string> emails,
        Dictionary<int, int> ages, Dictionary<int, bool> bestFriends)
    {
        Console.Write("Enter the name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Enter the last name: ");
        string lastname = Console.ReadLine() ?? "";

        Console.Write("Enter the address: ");
        string address = Console.ReadLine() ?? "";

        Console.Write("Enter the phone number: ");
        string phone = Console.ReadLine() ?? "";

        Console.Write("Enter the email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Enter the age: ");
        int age;
        if (!int.TryParse(Console.ReadLine(), out age))
        {
            age = 0;
        }

        Console.Write("Is this person a best friend? (1 = Yes / 0 = No): ");
        string bfInput = Console.ReadLine();
        bool isBestFriend = bfInput == "1";

        int id = ids.Count + 1;
        ids.Add(id);
        names[id] = name;
        lastnames[id] = lastname;
        addresses[id] = address;
        telephones[id] = phone;
        emails[id] = email;
        ages[id] = age;
        bestFriends[id] = isBestFriend;

        Console.WriteLine("Contact added successfully.\n");
    }

    static void ShowContacts(List<int> ids, Dictionary<int, string> names, Dictionary<int, string> lastnames,
        Dictionary<int, string> addresses, Dictionary<int, string> telephones, Dictionary<int, string> emails,
        Dictionary<int, int> ages, Dictionary<int, bool> bestFriends)
    {
        if (ids.Count == 0)
        {
            Console.WriteLine("No contacts available.\n");
            return;
        }

        Console.WriteLine("\nName\tLastname\tAddress\tPhone\tEmail\tAge\tBestFriend");
        Console.WriteLine("--------------------------------------------------------------------------");

        foreach (var id in ids)
        {
            string bestFriendStr = bestFriends.ContainsKey(id) && bestFriends[id] ? "Yes" : "No";
            Console.WriteLine($"{names[id]}\t{lastnames[id]}\t{addresses[id]}\t{telephones[id]}\t{emails[id]}\t{ages[id]}\t{bestFriendStr}");
        }
        Console.WriteLine();
    }

    static void SearchContact(List<int> ids, Dictionary<int, string> names, Dictionary<int, string> lastnames,
        Dictionary<int, string> telephones)
    {
        Console.Write("Enter a name or phone number to search: ");
        string search = (Console.ReadLine() ?? "").ToLower();

        if (string.IsNullOrWhiteSpace(search))
        {
            Console.WriteLine("You must enter a search term.\n");
            return;
        }

        bool found = false;

        foreach (var id in ids)
        {
            string name = names[id].ToLower();
            string phone = telephones[id];

            if (name.Contains(search) || phone.Contains(search))
            {
                Console.WriteLine($"Found: {names[id]} {lastnames[id]} - Tel: {telephones[id]}");
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("No contact was found with that information.\n");
        }
    }

    static void ModifyContact(List<int> ids, Dictionary<int, string> names, Dictionary<int, string> lastnames,
        Dictionary<int, string> addresses, Dictionary<int, string> telephones, Dictionary<int, string> emails,
        Dictionary<int, int> ages, Dictionary<int, bool> bestFriends)
    {
        Console.Write("Enter the name of the contact to modify: ");
        string search = (Console.ReadLine() ?? "").ToLower();

        foreach (var id in ids)
        {
            if (names[id].ToLower() == search)
            {
                Console.WriteLine($"Modifying {names[id]} {lastnames[id]}...");

                Console.Write("New name (press Enter to keep): ");
                string newName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName)) names[id] = newName;

                Console.Write("New phone (press Enter to keep): ");
                string newPhone = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newPhone)) telephones[id] = newPhone;

                Console.Write("New email (press Enter to keep): ");
                string newEmail = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newEmail)) emails[id] = newEmail;

                Console.WriteLine("Contact modified successfully.\n");
                return;
            }
        }

        Console.WriteLine("Contact not found.\n");
    }

    static void DeleteContact(List<int> ids, Dictionary<int, string> names, Dictionary<int, string> lastnames,
        Dictionary<int, string> addresses, Dictionary<int, string> telephones, Dictionary<int, string> emails,
        Dictionary<int, int> ages, Dictionary<int, bool> bestFriends)
    {
        Console.Write("Enter the name of the contact to delete: ");
        string search = (Console.ReadLine() ?? "").ToLower();

        foreach (var id in new List<int>(ids))
        {
            if (names[id].ToLower() == search)
            {
                ids.Remove(id);
                names.Remove(id);
                lastnames.Remove(id);
                addresses.Remove(id);
                telephones.Remove(id);
                emails.Remove(id);
                ages.Remove(id);
                bestFriends.Remove(id);

                Console.WriteLine("Contact deleted successfully.\n");
                return;
            }
        }

        Console.WriteLine("The contact to delete was not found.\n");
    }
}


