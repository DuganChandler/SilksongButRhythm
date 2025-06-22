using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreHolder : MonoBehaviour
{
    public static ScoreHolder Instance { get; private set; }

    [Header("Scenes")]
    [SerializeField] private int resultsSceneIndex;
    [SerializeField] private int lossSceneIndex;

    [Header("In-Scene Elements")]
    [SerializeField] private TextMeshProUGUI scoreTextBox;
    [SerializeField] private Animator rankBoxAnim;

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
        HealthManager.OnAllHealthLoss += GoToLoseScreen;
        Composer.OnSongEnd += GoToWinScreen;
    }

    private void OnDisable()
    {
        LaneManager.NoteCompletedAction -= RegisterNote;
        HealthManager.OnAllHealthLoss -= GoToLoseScreen;
        Composer.OnSongEnd -= GoToWinScreen;
    }

    private void RegisterNote(Rank rank)
    {
        switch (rank)
        {
            case Rank.Perfect:
                PerfectScoreAmount++;
                currentCombo++;
                break;
            case Rank.Good:
                GoodScoreAmount++;
                currentCombo++;
                break;
            case Rank.Meh:
                MehScoreAmount++;
                currentCombo++;
                break;
            case Rank.Miss:
                MissAmount++;
                currentCombo = 0;
                break;
        }

        rankBoxAnim.SetTrigger(rank.ToString());

        if (currentCombo > MaxCombo) MaxCombo = currentCombo;
        scoreTextBox.text = TotalScore.ToString();
    }

    private void GoToWinScreen()
    {
        SceneManager.LoadScene(resultsSceneIndex);
        scoreTextBox.text = "";
    }

    private void GoToLoseScreen()
    {
        SceneManager.LoadScene(lossSceneIndex);
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