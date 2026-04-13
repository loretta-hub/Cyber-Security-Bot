using System;
using System.Speech.Synthesis;

class Program
{
    static void Main()
    {
        PlayVoice();
        ShowHeader();

        Console.Write("Enter your name: ");
        string name = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(name))
        {
            Console.Write("Name cannot be empty. Enter your name: ");
            name = Console.ReadLine();
        }

        Console.WriteLine($"\nWelcome {name}! 👋");
        Console.WriteLine("Type 'help', 'scenario', or 'exit'.\n");

        ChatLoop(name);
    }

    // 🎧 Voice greeting
    static void PlayVoice()
    {
        SpeechSynthesizer speech = new SpeechSynthesizer();
        speech.Speak("Hello, welcome to CyberAwareBot. I will help you stay safe online.");
    }

    // 🎨 Header
    static void ShowHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=====================================");
        Console.WriteLine("🤖 CYBER AWARE BOT");
        Console.WriteLine("🔐 Cybersecurity Awareness Assistant");
        Console.WriteLine("=====================================");
        Console.ResetColor();
    }

    // 💬 Chat system
    static void ChatLoop(string name)
    {
        while (true)
        {
            Console.Write("\nYou: ");
            string input = Console.ReadLine().ToLower();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("🤖 Please type something.");
                continue;
            }

            if (input == "exit")
            {
                Console.WriteLine($"🤖 Goodbye {name}! Stay safe online 🔐");
                break;
            }

            else if (input == "help")
            {
                Console.WriteLine("\n🤖 I can help you with:");
                Console.WriteLine("- Password safety");
                Console.WriteLine("- Phishing awareness");
                Console.WriteLine("- Safe browsing");
                Console.WriteLine("- Scenario (type 'scenario')");
            }

            else if (input.Contains("password"))
            {
                Console.WriteLine("🔐 Use strong passwords with letters, numbers & symbols.");
            }

            else if (input.Contains("phishing"))
            {
                Console.WriteLine("⚠️ Phishing is when scammers trick you into giving personal info.");
            }

            else if (input.Contains("safe browsing"))
            {
                Console.WriteLine("🌐 Always check HTTPS and avoid suspicious links.");
            }

            else if (input.Contains("scenario"))
            {
                ShowScenarios();
            }

            else
            {
                Console.WriteLine("🤖 I didn't understand that. Type 'help' for options.");
            }
        }
    }

    // 🎯 Scenario menu
    static void ShowScenarios()
    {
        Console.WriteLine("\n⚠️ CYBERSECURITY SCENARIOS");
        Console.WriteLine("1. Phishing Email");
        Console.WriteLine("2. Fake Password Reset");
        Console.WriteLine("3. Suspicious Link");

        Console.Write("\nChoose (1-3): ");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            PhishingScenario();
        }
        else if (choice == "2")
        {
            PasswordResetScenario();
        }
        else if (choice == "3")
        {
            SuspiciousLinkScenario();
        }
        else
        {
            Console.WriteLine("🤖 Invalid choice. Please select 1-3.");
        }
    }

    // 📧 Scenario 1
    static void PhishingScenario()
    {
        Console.WriteLine("\n📧 SCENARIO: Phishing Email");
        Console.WriteLine("You receive an email: 'Your bank account is locked. Click here!'");

        Console.WriteLine("\nWhat do you do?");
        Console.WriteLine("1. Click the link");
        Console.WriteLine("2. Ignore it");
        Console.WriteLine("3. Report it");

        Console.Write("Answer: ");
        string answer = Console.ReadLine();

        if (answer == "3")
            Console.WriteLine("✅ Correct! Reporting is best.");
        else if (answer == "2")
            Console.WriteLine("⚠️ Safer, but reporting is better.");
        else
            Console.WriteLine("❌ Wrong! This is phishing.");
    }

    // 🔐 Scenario 2
    static void PasswordResetScenario()
    {
        Console.WriteLine("\n🔐 SCENARIO: Fake Password Reset");
        Console.WriteLine("You get a message asking to reset your password via a link.");

        Console.WriteLine("\nWhat do you do?");
        Console.WriteLine("1. Click link");
        Console.WriteLine("2. Go to official website manually");
        Console.WriteLine("3. Ignore");

        Console.Write("Answer: ");
        string answer = Console.ReadLine();

        if (answer == "2")
            Console.WriteLine("✅ Correct! Always use official sites.");
        else
            Console.WriteLine("❌ Wrong! This could be a scam.");
    }

    // 🌐 Scenario 3
    static void SuspiciousLinkScenario()
    {
        Console.WriteLine("\n🌐 SCENARIO: Suspicious Link");
        Console.WriteLine("A friend sends you a shortened link you don't recognize.");

        Console.WriteLine("\nWhat do you do?");
        Console.WriteLine("1. Click it");
        Console.WriteLine("2. Check it first");
        Console.WriteLine("3. Ignore it");

        Console.Write("Answer: ");
        string answer = Console.ReadLine();

        if (answer == "2")
            Console.WriteLine("✅ Correct! Always verify links.");
        else
            Console.WriteLine("❌ Dangerous choice!");
    }
}