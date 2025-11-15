// Name of Student: Yohana Montero
// Registration: 2025-0939
// Day of class: Friday

using System;
using System.Collections.Generic;

public class Patient
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int Age { get; set; }
    public string Condition { get; set; }
    public string Doctor { get; set; }

    public Patient(int id, string fullName, int age, string condition, string doctor)
    {
        Id = id;
        FullName = fullName;
        Age = age;
        Condition = condition;
        Doctor = doctor;
    }
}

public class PatientManager
{
    private List<Patient> patients = new List<Patient>();

    // Add Patient
    public void AddPatient()
    {
        Console.WriteLine("\n--- Add New Patient ---");

        int id = patients.Count + 1;

        Console.Write("Enter full name: ");
        string name = Console.ReadLine();

        Console.Write("Enter age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter condition (diagnosis): ");
        string condition = Console.ReadLine();

        Console.Write("Enter doctor in charge: ");
        string doctor = Console.ReadLine();

        patients.Add(new Patient(id, name, age, condition, doctor));

        Console.WriteLine("\nPatient successfully added!\n");
    }

    // View all patients
    public void ViewPatients()
    {
        Console.WriteLine("\nID   Name              Age   Condition             Doctor");
        Console.WriteLine("---------------------------------------------------------------");

        foreach (var p in patients)
        {
            Console.WriteLine($"{p.Id}    {p.FullName}     {p.Age}    {p.Condition}       {p.Doctor}");
        }
        Console.WriteLine();
    }

    // Search patient by ID
    public void SearchPatient()
    {
        ViewPatients();

        Console.Write("Enter ID to search: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Patient found = patients.Find(p => p.Id == id);

        if (found == null)
        {
            Console.WriteLine("\nPatient not found.\n");
            return;
        }

        Console.WriteLine($"\nName: {found.FullName}");
        Console.WriteLine($"Age: {found.Age}");
        Console.WriteLine($"Condition: {found.Condition}");
        Console.WriteLine($"Doctor: {found.Doctor}\n");
    }

    // Edit patient data
    public void EditPatient()
    {
        ViewPatients();
        Console.Write("Enter ID to edit: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Patient found = patients.Find(p => p.Id == id);

        if (found == null)
        {
            Console.WriteLine("\nPatient not found.\n");
            return;
        }

        Console.WriteLine("\nLeave blank to keep the current value.\n");

        Console.Write($"Current Name ({found.FullName}): ");
        string newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName)) found.FullName = newName;

        Console.Write($"Current Age ({found.Age}): ");
        string newAgeStr = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newAgeStr)) found.Age = Convert.ToInt32(newAgeStr);

        Console.Write($"Current Condition ({found.Condition}): ");
        string newCondition = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newCondition)) found.Condition = newCondition;

        Console.Write($"Current Doctor ({found.Doctor}): ");
        string newDoctor = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newDoctor)) found.Doctor = newDoctor;

        Console.WriteLine("\nPatient updated successfully!\n");
    }

    // Delete patient
    public void DeletePatient()
    {
        ViewPatients();
        Console.Write("Enter ID to delete: ");
        int id = Convert.ToInt32(Console.ReadLine());

        Patient found = patients.Find(p => p.Id == id);

        if (found == null)
        {
            Console.WriteLine("\nPatient not found.\n");
            return;
        }

        Console.WriteLine("Are you sure? (1 = Yes, 2 = No)");
        int confirm = Convert.ToInt32(Console.ReadLine());

        if (confirm == 1)
        {
            patients.Remove(found);
            Console.WriteLine("\nPatient removed successfully!\n");
        }
        else
        {
            Console.WriteLine("\nDeletion canceled.\n");
        }
    }
}

class Program
{
    static void Main()
    {
        PatientManager manager = new PatientManager();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n===== PATIENT REGISTRY MENU =====");
            Console.WriteLine("1. Add Patient");
            Console.WriteLine("2. View All Patients");
            Console.WriteLine("3. Search Patient");
            Console.WriteLine("4. Edit Patient");
            Console.WriteLine("5. Delete Patient");
            Console.WriteLine("6. Exit");
            Console.WriteLine("=================================");
            Console.Write("Choose an option: ");

            int option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    manager.AddPatient();
                    break;
                case 2:
                    manager.ViewPatients();
                    break;
                case 3:
                    manager.SearchPatient();
                    break;
                case 4:
                    manager.EditPatient();
                    break;
                case 5:
                    manager.DeletePatient();
                    break;
                case 6:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option.\n");
                    break;
            }
        }
    }
}
