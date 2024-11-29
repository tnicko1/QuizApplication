using System.Text.Json;

namespace Quiz_Application;

public class QuizDatabase
{
    private List<Quiz> Quizzes { get; set; }
    private readonly string _filePath;
    
    public QuizDatabase()
    {
        _filePath = Path.Combine("..","..","..", "quizzes.json");
        Quizzes = [];
        LoadQuizzesFromJson();
    }
    
    private void LoadQuizzesFromJson()
    {
        try 
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                var quizzesDict = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);
                
                if (quizzesDict != null)
                {
                    foreach (var (name, quizInfo) in quizzesDict)
                    {
                        if (quizInfo.TryGetValue("Questions", out var questions) &&
                            quizInfo.TryGetValue("Answers", out var answers) &&
                            quizInfo.TryGetValue("IncorrectAnswers", out var incorrectAnswers))
                        {
                            var newQuiz = new Quiz
                            {
                                Name = name,
                                Questions = questions.Split(',').ToList(),
                                Answers = answers.Split(',').ToList(),
                                IncorrectAnswers = incorrectAnswers.Split(',').ToList()
                            };
                            Quizzes.Add(newQuiz);
                        }
                    }
                }
                    
                
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading users: {ex.Message}");
        }
    }

    public void SaveQuizzesToJson()
    {
        try 
        {
            var quizzesDict = Quizzes.ToDictionary(
                quiz => quiz.Name,
                quiz => new Dictionary<string, string?>
                {
                    { "Questions", string.Join(",", quiz.Questions) },
                    { "Answers", string.Join(",", quiz.Answers) },
                    { "IncorrectAnswers", string.Join(",", quiz.IncorrectAnswers) }
                }
            );
        
            var jsonOptions = new JsonSerializerOptions 
            { 
                WriteIndented = true 
            };

            var json = JsonSerializer.Serialize(quizzesDict, jsonOptions);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving quizzes: {ex.Message}");
        }
    }

    public void AddQuiz(Quiz quiz)
    {
        Quizzes.Add(quiz);
        SaveQuizzesToJson();
    }
    
    public List<Quiz> GetQuizzes()
    {
        return Quizzes;
    }
    
    public void RemoveQuiz(Quiz quiz)
    {
        Quizzes.Remove(quiz);
        SaveQuizzesToJson();
    }

    public Quiz? FindQuizByName(string quizName)
    {
        return Quizzes.Find(quiz => quiz.Name == quizName);
    }
}