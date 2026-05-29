using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Windows;

namespace CyberSecurityBot1GUI
{
    public partial class MainWindow : Window
    {
        Random random = new Random();
        SpeechSynthesizer synth = new SpeechSynthesizer();

        string currentTopic = "";
        Dictionary<string, string> memory = new Dictionary<string, string>();

        List<string> phishingTips = new List<string>()
        {
            "Be careful of emails asking for personal information.",
            "Never click suspicious links from unknown senders.",
            "Scammers often pretend to be trusted organisations."
        };

        string[] phishingAlerts =
        {
            " This message looks suspicious. Do not share personal details.",
            "Security warning: Possible phishing attempt detected.",
            "Be careful — this may be a scam message."
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = UserInput.Text.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(userMessage))
            {
                ChatDisplay.AppendText("Bot: Please type a message so I can help you.\n\n");
                return;
            }

            ChatDisplay.AppendText("You: " + userMessage + "\n");

            string sentiment = DetectSentiment(userMessage);

            HandleMemory(userMessage);

            HandleKeywords(userMessage);

            HandleConversationFlow(userMessage);

            ChatDisplay.AppendText("Mood detected: " + sentiment + "\n\n");

            Speak("Message received");

            UserInput.Clear();
        }

        private void HandleKeywords(string userMessage)
        {
            if (userMessage.Contains("password"))
            {
                currentTopic = "password";
                ChatDisplay.AppendText("Bot: Use strong, unique passwords with numbers, symbols, and no personal details.\n\n");
            }
            else if (userMessage.Contains("scam"))
            {
                currentTopic = "scam";
                ChatDisplay.AppendText("Bot: Be cautious of scams asking for money or personal information.\n\n");
            }
            else if (userMessage.Contains("privacy"))
            {
                currentTopic = "privacy";
                ChatDisplay.AppendText("Bot: Protect your privacy by securing your accounts and limiting personal information online.\n\n");
            }
            else if (userMessage.Contains("phishing"))
            {
                currentTopic = "phishing";
                ChatDisplay.AppendText("Bot: " + phishingTips[random.Next(phishingTips.Count)] + "\n\n");
            }
            else if (userMessage.Contains("bank") || userMessage.Contains("otp") || userMessage.Contains("login"))
            {
                ChatDisplay.AppendText("Bot: " + phishingAlerts[random.Next(phishingAlerts.Length)] + "\n\n");
                Speak("Warning detected");
            }
            else
            {
                ChatDisplay.AppendText("Bot: I'm not sure I understand. Can you try rephrasing?\n\n");
            }
        }

        private void HandleConversationFlow(string userMessage)
        {
            if (userMessage.Contains("tell me more") ||
                userMessage.Contains("another tip") ||
                userMessage.Contains("explain more"))
            {
                ContinueConversation();
            }

            if (userMessage.Contains("worried"))
            {
                ChatDisplay.AppendText("Bot: It's completely understandable to feel worried. Scammers can be very convincing.\n\n");
                ChatDisplay.AppendText("Bot: " + phishingTips[random.Next(phishingTips.Count)] + "\n\n");
            }

            if (userMessage.Contains("curious"))
            {
                ChatDisplay.AppendText("Bot: Curiosity is good — learning about cybersecurity helps keep you safe.\n\n");
            }

            if (userMessage.Contains("frustrated"))
            {
                ChatDisplay.AppendText("Bot: I understand your frustration. Let me simplify it for you.\n\n");
            }
        }

        private void HandleMemory(string userMessage)
        {
            if (userMessage.StartsWith("my name is"))
            {
                string name = userMessage.Replace("my name is", "").Trim();
                memory["name"] = name;

                ChatDisplay.AppendText("Bot: Nice to meet you " + name + "\n\n");
                Speak("Nice to meet you");
            }

            if (userMessage.Contains("i'm interested in privacy"))
            {
                memory["topic"] = "privacy";

                ChatDisplay.AppendText("Bot: Great! I'll remember that you're interested in privacy. It's a crucial part of staying safe online.\n\n");
                Speak("Noted your interest in privacy");
            }

            if (userMessage.Contains("what am i interested in"))
            {
                string topic = memory.ContainsKey("topic") ? memory["topic"] : "nothing specific";

                ChatDisplay.AppendText("Bot: You are interested in " + topic + ".\n\n");
            }

            if (userMessage.Contains("what is my name"))
            {
                string name = memory.ContainsKey("name") ? memory["name"] : "unknown";

                ChatDisplay.AppendText("Bot: Your name is " + name + "\n\n");
            }
        }

        private void ContinueConversation()
        {
            if (currentTopic == "password")
            {
                ChatDisplay.AppendText("Bot: Avoid using birthdays or names in passwords and change them regularly.\n\n");
            }
            else if (currentTopic == "phishing")
            {
                ChatDisplay.AppendText("Bot: " + phishingTips[random.Next(phishingTips.Count)] + "\n\n");
            }
        }

        private string DetectSentiment(string message)
        {
            if (message.Contains("worried"))
                return "Worried ";

            if (message.Contains("curious"))
                return "Curious ";

            if (message.Contains("frustrated"))
                return "Frustrated ";

            return "Neutral ";
        }

        private void Speak(string text)
        {
            synth.SpeakAsync(text);
        }
    }
}