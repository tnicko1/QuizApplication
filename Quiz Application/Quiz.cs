namespace Quiz_Application;

public class Quiz
{
    public string? Name { get; set; }
    public List<string> Questions { get; set; }
    
    public List<string> Answers { get; set; }
    public List<string> IncorrectAnswers { get; set; }
    public static QuizDatabase? QuizDatabase { get; set; }
    
    private int Score { get; set; }
    private int HighScore { get; set; }

    public Quiz()
    {
        Questions = [];
        Answers = [];
        IncorrectAnswers = [];
        Score = 0;
    }

    private static void CreateQuestions(ref Quiz quizObject)
    {
        for (var i = 0; i < 5; i++)
        {
            Console.WriteLine($"Enter question {i + 1}");
            var question = Console.ReadLine();
            var questionList = quizObject.GetQuestions();
            if (question != null) questionList.Add(question);
        }
    }

    private static void CreateAnswers(ref Quiz quizObject)
    {
        for (var i = 0; i < 5; i++)
        {
            Console.WriteLine($"Enter correct answer for Question: {quizObject.GetQuestions()[i]}");
            var answer = Console.ReadLine();
            var answerList = quizObject.GetAnswers();
            if (answer != null) answerList.Add(answer);
            
            for (var j = 0; j < 3; j++)
            {
                Console.WriteLine($"Enter incorrect answer for Question: {quizObject.GetQuestions()[i]}");
                var incorrectAnswer = Console.ReadLine();
                var incorrectAnswerList = quizObject.GetIncorrectAnswers();
                if (incorrectAnswer != null) incorrectAnswerList.Add(incorrectAnswer);
            }
        }
    }
    
    public static Quiz CreateQuiz()
    {
        var quizObject = new Quiz();
        Console.WriteLine("Enter quiz name:");
        quizObject.Name = Console.ReadLine();
        CreateQuestions(ref quizObject);
        CreateAnswers(ref quizObject);
        QuizDatabase?.AddQuiz(quizObject);
        return quizObject;
    }
    
    public List<string> GetQuestions()
    {
        return Questions;
    }
    
    public List<string> GetAnswers()
    {
        return Answers;
    }
    
    public List<string> GetIncorrectAnswers()
    {
        return IncorrectAnswers;
    }
    
    public int GetScore()
    {
        return Score;
    }
    
    public int GetHighScore()
    {
        return HighScore;
    }

    public void Play()
    {
        var random = new Random();
        for (var i = 0; i < 5; i++)
        {
            Console.WriteLine(Questions[i]);
            var options = new List<string> { Answers[i], IncorrectAnswers[i * 3], IncorrectAnswers[i * 3 + 1], IncorrectAnswers[i * 3 + 2] };
            options = options.OrderBy(x => random.Next()).ToList();

            Console.WriteLine("Options:");
            for (var j = 0; j < options.Count; j++)
            {
                Console.WriteLine($"{j + 1}. {options[j]}");
            }

            var timeRequired = DateTime.Now.AddMinutes(2);
            var answeredCorrectly = false;
            while (DateTime.Now < timeRequired && !answeredCorrectly)
            {
                Console.WriteLine("Enter your answer:");
                var userAnswer = Console.ReadLine();
                if (userAnswer != null && int.TryParse(userAnswer, out int answerIndex) && answerIndex > 0 && answerIndex <= options.Count)
                {
                    if (options[answerIndex - 1] == Answers[i])
                    {
                        Score += 20;
                        answeredCorrectly = true;
                    }
                    else
                    {
                        Score -= 20;
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number corresponding to one of the options.");
                }
            }
        }
        Console.WriteLine($"Your score is: {Score}");
        if (Score <= HighScore) return;
        HighScore = Score;
        Console.WriteLine("New high score!");
    }
}