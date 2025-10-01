//Student name: Yohana Montero
//Student ID: 2025-0939
//class day : viernes

using System;


class Program
{
    static void Main()
    {
        // Enter a number 
        Console.Write("Digite un numero:");
        int number = Convert.ToInt32(Console.ReadLine());

        if (number % 2 == 0)
        {
            Console.WriteLine("The number:" + number + " es par");
        }
        else
        {
            Console.WriteLine("The number:" + number + " es impar");
        }
    }
}

