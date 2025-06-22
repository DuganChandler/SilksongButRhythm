using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreHolder : MonoBehaviour
{
    public static ScoreHolder Instance { get; private set; }

    [SerializeField] private int resultsSceneIndex;

    [Header("In-Scene Elements")]
    [SerializeField] private TextMeshProUGUI scoreTextBox;

    [Header("Score Values")]
    [SerializeField] private int perfectValue = 1000;
    [SerializeField] private int goodValue = 500;
    [SerializeField] private int mehValue = 100;

    public int PerfectScoreAmount { get; private set; }
    public int GoodScoreAmount { get; private set; }
    public int MehScoreAmount { get; private set; }
    public int MissAmount { get; private set; }
    public int TotalScore => (PerfectScoreAmount * perfectValue) + (GoodScoreAmount * goodValue) + (MehScoreAmount + mehValue);
    public int MaxCombo { get; private set; }

    private int currentCombo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        LaneManager.NoteCompletedAction += RegisterNote;
        HealthManager.OnAllHealthLoss += GoToResultsScreen;
        Composer.OnSongEnd += GoToResultsScreen;
    }

    private void OnDisable()
    {
        LaneManager.NoteCompletedAction -= RegisterNote;
        HealthManager.OnAllHealthLoss -= GoToResultsScreen;
        Composer.OnSongEnd -= GoToResultsScreen;
    }

    private void RegisterNote(Rank rank)
    {
        switch (rank)
        {
            case Rank.Perfect:
                PerfectScoreAmount++;
                break;
            case Rank.Good:
                GoodScoreAmount++;
                break;
            case Rank.Meh:
                MehScoreAmount++;
                break;
            case Rank.Miss:
                MissAmount++;
                break;
        }

        if (rank != Rank.Miss) currentCombo++;
        if (currentCombo > MaxCombo) MaxCombo = currentCombo;
        scoreTextBox.text = TotalScore.ToString();
    }

    private void GoToResultsScreen()
    {
        SceneManager.LoadScene(resultsSceneIndex);
        scoreTextBox.text = "";
    }

    [ContextMenu("Print Scores")]
    private void PrintScores()
    {
        print("Perfect: " + PerfectScoreAmount +
            "\nGood: " + GoodScoreAmount +
            "\nMeh: " + MehScoreAmount +
            "\nMiss: " + MissAmount);
    }
}

public enum Rank
{
    Perfect,
    Good,
    Meh,
    Miss
}