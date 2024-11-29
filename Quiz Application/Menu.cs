namespace Quiz_Application;

public class Menu
{
    public static UserDatabase? UserDatabase { get; set; }
    public static QuizDatabase? QuizDatabase { get; set; }
    public static Quiz? CurrentQuiz { get; set; }
    private static User? CurrentUser { get; set; }
    
    public static void DisplayLoginMenu()
    {
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Exit");
        
        var choice = int.Parse(Console.ReadLine() ?? string.Empty);
        
        switch (choice)
        {
            case 1:
                Login();
                break;
            case 2:
                Register();
                break;
            case 3:
                Environment.Exit(0);
                break;
        }
        
    }

    private static void DisplayUserMenu()
    {
        while (true)
        {
            Console.WriteLine("1. Take Quiz");
            Console.WriteLine("2. Logout");

            var choice = int.Parse(Console.ReadLine() ?? string.Empty);

            switch (choice)
            {
                case 1:
                    Console.WriteLine("\n Available Quizzes:\n");
                    foreach (var quiz in QuizDatabase?.GetQuizzes()!)
                    {
                        Console.WriteLine(quiz.Name);
                    }

                    Console.WriteLine("\n");
                    DisplayQuizMenu();
                    continue;
                case 2:
                    DisplayLoginMenu();
                    break;
            }

            break;
        }
    }

    private static void DisplayAdminMenu()
    {
        while (true)
        {
            Console.WriteLine("1. Create Quiz");
            Console.WriteLine("2. View Users");
            Console.WriteLine("3. Make User Admin");
            Console.WriteLine("4. Logout");

            var choice = int.Parse(Console.ReadLine() ?? string.Empty);

            switch (choice)
            {
                case 1:
                    Quiz.CreateQuiz();
                    continue;
                case 2:
                    foreach (var user in UserDatabase?.GetUsers()!)
                    {
                        user.Display();
                    }
                    DisplayAdminMenu();
                    break;
                case 3:
                    UserDatabase?.ChangeRole();
                    DisplayAdminMenu();
                    break;
                case 4:
                    DisplayLoginMenu();
                    break;
            }

            break;
        }
    }

    private static void DisplayQuizMenu()
    {
        Console.WriteLine("1. play Quiz");
        Console.WriteLine("2. View Scores");
        Console.WriteLine("3. Exit");
        
        var choice = int.Parse(Console.ReadLine() ?? string.Empty);
        
        switch (choice)
        {
            case 1:
                Console.WriteLine("Enter quiz name:");
                var quizName = Console.ReadLine();
                if (quizName != null)
                {
                    var quiz = QuizDatabase?.FindQuizByName(quizName);
                    if (quiz != null)
                    {
                        CurrentQuiz = quiz;
                        Console.WriteLine("Quiz found\n");
                        Console.WriteLine("Press any key to start quiz");
                        Console.ReadKey();
                        CurrentQuiz.Play();
                    }
                    else
                    {
                        Console.WriteLine("Quiz not found");
                    }
                }
                break;
            case 2:
                DisplayHighScores();
                break;
            case 3:
                DisplayUserMenu();
                break;
        }
    }

    private static void DisplayHighScores()
    {
        foreach (var quiz in QuizDatabase?.GetQuizzes()!)
        {
            Console.WriteLine($"Quiz: {quiz.Name}");
            Console.WriteLine($"High Score: {quiz.GetHighScore()}");
            Console.WriteLine("\n");
        }
    }

    private static void Login()
    {
        Console.WriteLine("Enter email:");
        var email = Console.ReadLine();
        Console.WriteLine("Enter password:");
        var password = Console.ReadLine();
        
        // Check if user exists
        if (email == null) return;
        var user = UserDatabase?.FindUserByEmail(email);
        if (user != null && user.GetPassword() == password)
        {
            Console.WriteLine("Login successful\n");
            if (user.GetRole() == "admin")
            {
                DisplayAdminMenu();
            }
            else
            {
                DisplayUserMenu();
            }
            CurrentUser = user;
        }
        else
        {
            Console.WriteLine("Invalid email or password");
        }
    }

    private static void Register()
    {
        Console.WriteLine("Enter name:");
        var name = Console.ReadLine();
        Console.WriteLine("Enter email:");
        var email = Console.ReadLine();
        Console.WriteLine("Enter password:");
        var password = Console.ReadLine();
        
        if (name == null || email == null || password == null) return;
        User.CreateUser(name, email, password, "user", UserDatabase);
        
        Console.WriteLine("User created successfully");
        
        DisplayLoginMenu();
    }
}