// Student Name: Yohana Montero
// Student ID: 2025-0939
// Class Day: viernes

using System;

class Program
{
    static void Main()
    {
        // 1. Declare variables of different types, assign values, and print them.
        int myInt = 25;
        float myFloat = 12.5f;
        double myDouble = 45.67;
        char myChar = 'A';
        string myString = "Hello Yoa";
        bool myBool = true;

        Console.WriteLine("Integer: " + myInt);
        Console.WriteLine("Float: " + myFloat);
        Console.WriteLine("Double: " + myDouble);
        Console.WriteLine("Char: " + myChar);
        Console.WriteLine("String: " + myString);
        Console.WriteLine("Boolean: " + myBool);

        // 2. Constant declaration in C#
        const double Pi = 3.1416;
        Console.WriteLine("Constant Pi: " + Pi);

        // If you try to change Pi = 4.5; -> ERROR (constants cannot be modified)

        // 3. Declare an integer, increment, decrement, and make operations
        int number = 10;
        number++; // increment
        Console.WriteLine("Increment: " + number);
        number--; // decrement
        Console.WriteLine("Decrement: " + number);
        int result = number * 5;
        Console.WriteLine("Operation result: " + result);

        // 4. Declare a float and a byte
        float bigFloat = 10152466.25f;

        // A float + 5 cannot be directly stored in a byte because of data loss
        // We need to CAST the value
        byte myByte = (byte)(5 + bigFloat);

        Console.WriteLine("Float: " + bigFloat);
        Console.WriteLine("Byte (5 + float with cast): " + myByte);

        // 5. Comments in code
        // This is a single-line comment

        /* 
         * This is a multi-line comment
         * It can describe more details
         */

        // Print system date and time
        Console.WriteLine("System Date and Time: " + DateTime.Now);
    }
}