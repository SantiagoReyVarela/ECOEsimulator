using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExamManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text questionText;
    public TMP_Text questionCounterText;
    public TMP_Text timerText;

    [Header("Answer Buttons")]
    public Button[] answerButtons;

    [Header("Navigation")]
    public Button nextButton;

    [Header("Panels")]
    public GameObject submitConfirmationPanel;
    public GameObject resultsPanel;

    [Header("Results UI")]
    public TMP_Text resultText;

    [Header("Exam Settings")]
    public int numberOfQuestions = 10;
    public float examDuration = 300f;

    [Header("Scenes")]
    public string gameplaySceneName = "GameScene";

    private QuestionData[] allQuestions;
    private QuestionData[] questions;

    private int currentQuestion = 0;

    private int[] playerAnswers;

    private float remainingTime;

    private bool examFinished = false;

    void Start()
    {
        LoadQuestions();

        GenerateExam();

        playerAnswers = new int[questions.Length];

        for (int i = 0; i < playerAnswers.Length; i++)
        {
            playerAnswers[i] = -1;
        }

        remainingTime = examDuration;

        submitConfirmationPanel.SetActive(false);

        if (resultsPanel != null)
        {
            resultsPanel.SetActive(false);
        }

        ShowQuestion();
    }

    void Update()
    {
        if (examFinished)
            return;

        remainingTime -= Time.deltaTime;

        if (remainingTime < 0)
        {
            remainingTime = 0;
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";

        if (remainingTime <= 60)
        {
            timerText.color = Color.red;
        }

        if (remainingTime <= 0)
        {
            SubmitExam();
        }
    }

    void LoadQuestions()
    {
        TextAsset jsonFile =
            Resources.Load<TextAsset>("questions"); 
            // cambiarle el nombre para que coincida con el JSON que desees >:)
            // ARCHIVOS DISPONIBLES:
            // "infantil" : preguntas sencillisimas que cualquiera sabe responder sobre cultura general
            // "ECOE" : preguntas del ECOE
            // lo suyo sería poner aquí archivos con preguntas de distintas dificultades
        if (jsonFile == null)
        {
            Debug.LogError(
                "No se encontró el archivo .json en Resources"
            );

            return;
        }

        QuestionDatabase database =
            JsonUtility.FromJson<QuestionDatabase>(
                jsonFile.text
            );

        allQuestions = database.questions;

        Debug.Log(
            "Preguntas cargadas: "
            + allQuestions.Length
        );
    }

    void GenerateExam()
    {
        if (allQuestions == null ||
            allQuestions.Length == 0)
        {
            Debug.LogError(
                "No hay preguntas cargadas"
            );

            return;
        }

        if (allQuestions.Length <= numberOfQuestions)
        {
            questions = allQuestions;
            return;
        }

        QuestionData[] shuffled =
            (QuestionData[])allQuestions.Clone();

        for (int i = 0; i < shuffled.Length; i++)
        {
            int randomIndex =
                Random.Range(i, shuffled.Length);

            QuestionData temp = shuffled[i];

            shuffled[i] = shuffled[randomIndex];

            shuffled[randomIndex] = temp;
        }

        questions = new QuestionData[numberOfQuestions];

        for (int i = 0; i < numberOfQuestions; i++)
        {
            questions[i] = shuffled[i];
        }
    }

    void ShowQuestion()
    {
        QuestionData q = questions[currentQuestion];

        questionText.text = q.question;

        questionCounterText.text =
            "Pregunta "
            + (currentQuestion + 1)
            + "/"
            + questions.Length;

        for (int i = 0;
             i < Mathf.Min(
                 answerButtons.Length,
                 q.answers.Length);
             i++)
        {
            TMP_Text buttonText =
                answerButtons[i]
                .GetComponentInChildren<TMP_Text>();

            buttonText.text = q.answers[i];
        }

        if (nextButton != null)
        {
            nextButton.interactable =
                playerAnswers[currentQuestion] != -1;
        }
    }

    public void SelectAnswer(int answerIndex)
    {
        Debug.Log(
            "Botón pulsado: "
            + answerIndex
        );

        playerAnswers[currentQuestion] =
            answerIndex;

        if (nextButton != null)
        {
            nextButton.interactable = true;
        }
    }

    public void NextQuestion()
    {
        if (currentQuestion <
            questions.Length - 1)
        {
            currentQuestion++;

            ShowQuestion();
        }
    }

    public void PreviousQuestion()
    {
        if (currentQuestion > 0)
        {
            currentQuestion--;

            ShowQuestion();
        }
    }

    public void OpenSubmitConfirmation()
    {
        submitConfirmationPanel.SetActive(true);
    }

    public void CloseSubmitConfirmation()
    {
        submitConfirmationPanel.SetActive(false);
    }

    public void SubmitExam()
    {
        if (examFinished)
            return;

        examFinished = true;

        int score = 0;

        for (int i = 0; i < questions.Length; i++)
        {
            if (playerAnswers[i] ==
                questions[i].correctAnswer)
            {
                score++;
            }
        }

        float finalGrade =
            ((float)score / questions.Length) * 10f;

        PlayerPrefs.SetFloat(
            "ExamGrade",
            finalGrade
        );

        PlayerPrefs.SetInt(
            "CorrectAnswers",
            score
        );

        PlayerPrefs.SetFloat(
            "RemainingTime",
            remainingTime
        );

        PlayerPrefs.Save();

        if (submitConfirmationPanel != null)
        {
            submitConfirmationPanel.SetActive(false);
        }

        if (resultsPanel != null)
        {
            resultsPanel.SetActive(true);

            resultText.text =
                "RESULTADO FINAL\n\n"
                + "Aciertos: "
                + score
                + "/"
                + questions.Length
                + "\n\nNota: "
                + finalGrade.ToString("F1");
        }

        Debug.Log(
            "Nota final: "
            + finalGrade.ToString("F1")
        );
    }

    public void ReturnToGame()
    {
        SceneManager.LoadScene(
            gameplaySceneName
        );
    }
}

[System.Serializable]
public class QuestionData
{
    [TextArea]
    public string question;

    public string[] answers;

    public int correctAnswer;
}

[System.Serializable]
public class QuestionDatabase
{
    public QuestionData[] questions;
}