using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Windows;
using MySql.Data.MySqlClient;

namespace CyberSecurityBot1GUI
{
    public partial class MainWindow : Window
    {
        Random random = new Random();
        SpeechSynthesizer synth = new SpeechSynthesizer();
        DatabaseHelper db = new DatabaseHelper();

        List<(string Question, string[] Options, int CorrectIndex, string Explanation)> quiz;
        int currentQuestion = 0;
        int score = 0;
        bool quizActive = false;

        string currentTopic = "";
        Dictionary<string, string> memory = new Dictionary<string, string>();

        List<string> activityLog = new List<string>();

        List<string> phishingTips = new List<string>()
        {
            "Be careful of emails asking for personal information.",
            "Never click suspicious links from unknown senders.",
            "Scammers often pretend to be trusted organisations."
        };

        string[] phishingAlerts =
        {
            "This message looks suspicious. Do not share personal details.",
            "Security warning: Possible phishing attempt detected.",
            "Be careful — this may be a scam message."
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddLog(string action)
        {
            activityLog.Add(action);
        }

        private void LoadQuiz()
        {
            quiz = new List<(string, string[], int, string)>
            {
                ("What should you do if you receive an email asking for your password?",
                    new string[] { "Reply with it", "Delete email", "Report phishing", "Ignore it" },
                    2,
                    "Reporting phishing emails helps prevent scams."),

                ("What is a strong password?",
                    new string[] { "123456", "Your name", "Mix of letters, numbers & symbols", "password" },
                    2,
                    "Strong passwords are complex and hard to guess."),

                ("Phishing is:",
                    new string[] { "A game", "A cyber scam", "A firewall", "A browser" },
                    1,
                    "Phishing tricks users into giving sensitive information."),

                ("True or False: Public Wi-Fi is safe",
                    new string[] { "True", "False" },
                    1,
                    "Public Wi-Fi is not always safe."),

                ("What is 2FA?",
                    new string[] { "Extra security step", "Virus", "App", "Email" },
                    0,
                    "2FA adds extra protection.")
            };
        }

        private void ShowQuestion()
        {
            if (currentQuestion < quiz.Count)
            {
                var q = quiz[currentQuestion];

                ChatDisplay.AppendText("\nQuestion " + (currentQuestion + 1) + "\n");
                ChatDisplay.AppendText(q.Question + "\n\n");

                ChatDisplay.AppendText("A: " + q.Options[0] + "\n");
                ChatDisplay.AppendText("B: " + q.Options[1] + "\n");

                if (q.Options.Length > 2)
                    ChatDisplay.AppendText("C: " + q.Options[2] + "\n");

                if (q.Options.Length > 3)
                    ChatDisplay.AppendText("D: " + q.Options[3] + "\n");

                ChatDisplay.AppendText("\nType A, B, C or D\n\n");
            }
            else
            {
                quizActive = false;

                ChatDisplay.AppendText("\nQuiz finished!\n");
                ChatDisplay.AppendText("Score: " + score + "/" + quiz.Count + "\n");

                if (score >= 4)
                    ChatDisplay.AppendText("Great job! You're a cybersecurity pro!\n");
                else
                    ChatDisplay.AppendText("Keep learning to stay safe online!\n");

                AddLog("Quiz completed - Score: " + score + "/" + quiz.Count);
            }
        }

        private void CheckAnswer(string input)
        {
            var q = quiz[currentQuestion];

            input = input.ToLower().Trim();

            int selected = -1;

            if (input == "a") selected = 0;
            else if (input == "b") selected = 1;
            else if (input == "c") selected = 2;
            else if (input == "d") selected = 3;

            if (selected == q.CorrectIndex)
            {
                score++;
                ChatDisplay.AppendText("Correct!\n\n");
                AddLog("Correct answer - Q" + (currentQuestion + 1));
            }
            else
            {
                ChatDisplay.AppendText("Wrong!\n");
                ChatDisplay.AppendText(q.Explanation + "\n\n");
                AddLog("Wrong answer - Q" + (currentQuestion + 1));
            }

            currentQuestion++;
            ShowQuestion();
        }

        private string DetectIntent(string message)
        {
            message = message.ToLower();

            if (message.Contains("quiz") || message.Contains("test me") || message.Contains("start quiz"))
                return "quiz";

            if (message.Contains("add task") || message.Contains("remind me") || message.Contains("set reminder"))
                return "task";

            if (message.Contains("password"))
                return "password";

            if (message.Contains("phishing") || message.Contains("scam"))
                return "phishing";

            if (message.Contains("show log") || message.Contains("activity log"))
                return "log";

            return "unknown";
        }

        private string ExtractTask(string message)
        {
            string cleaned = message.ToLower();

            cleaned = cleaned.Replace("add task", "")
                             .Replace("remind me", "")
                             .Replace("set reminder", "")
                             .Replace("create task", "")
                             .Trim();

            return cleaned;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = UserInput.Text.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(userMessage))
            {
                ChatDisplay.AppendText("Bot: Please type a message.\n\n");
                return;
            }

            ChatDisplay.AppendText("You: " + userMessage + "\n");

            if (quizActive)
            {
                CheckAnswer(userMessage);
                UserInput.Clear();
                return;
            }

            string intent = DetectIntent(userMessage);

            switch (intent)
            {
                case "quiz":
                    LoadQuiz();
                    currentQuestion = 0;
                    score = 0;
                    quizActive = true;

                    ChatDisplay.AppendText("Bot: Starting Quiz...\n\n");
                    AddLog("Quiz started");
                    ShowQuestion();
                    break;

                case "task":
                    string task = ExtractTask(userMessage);

                    if (string.IsNullOrWhiteSpace(task))
                    {
                        ChatDisplay.AppendText("Bot: What should I remind you about?\n\n");
                    }
                    else
                    {
                        db.AddTask(task, "Cybersecurity task", DateTime.Now.AddDays(3).ToString("yyyy-MM-dd"));
                        ChatDisplay.AppendText("Bot: Task added: " + task + "\n\n");
                        AddLog("Task added: " + task);
                    }
                    break;

                case "password":
                    ChatDisplay.AppendText("Bot: Use strong passwords with numbers and symbols.\n\n");
                    AddLog("Password query detected");
                    break;

                case "phishing":
                    ChatDisplay.AppendText("Bot: " + phishingTips[random.Next(phishingTips.Count)] + "\n\n");
                    AddLog("Phishing query detected");
                    break;

                case "log":
                    ChatDisplay.AppendText("\nBot: Activity Log (Last 10 actions)\n\n");

                    int start = Math.Max(0, activityLog.Count - 10);

                    for (int i = start; i < activityLog.Count; i++)
                    {
                        ChatDisplay.AppendText((i + 1) + ". " + activityLog[i] + "\n");
                    }
                    break;

                default:
                    ChatDisplay.AppendText("Bot: I didn't understand that.\n\n");
                    break;
            }

            UserInput.Clear();
        }
    }
}