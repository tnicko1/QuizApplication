using Quiz_Application;

internal class Program
{
    public static void Main(string[] args)
    {
        var userDatabase = new UserDatabase();
        var quizDatabase = new QuizDatabase();
        
        Quiz.QuizDatabase = quizDatabase;
        User.UserDatabase = userDatabase;

        Menu.UserDatabase = userDatabase;
        Menu.QuizDatabase = quizDatabase;

        Menu.DisplayLoginMenu();
    }
}