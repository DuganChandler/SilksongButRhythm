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
    [Space(5)]
    [SerializeField] private Animator rankBoxAnim;
    [Space(5)]
    [SerializeField] private Animator comboBoxAnim;
    [SerializeField] private TextMeshProUGUI comboTextBox;

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
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
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
                if (currentCombo == 10) comboBoxAnim.SetTrigger("Open");
                break;
            case Rank.Good:
                GoodScoreAmount++;
                currentCombo++;
                if (currentCombo == 10) comboBoxAnim.SetTrigger("Open");
                break;
            case Rank.Meh:
                MehScoreAmount++;
                currentCombo++;
                if (currentCombo == 10) comboBoxAnim.SetTrigger("Open");
                break;
            case Rank.Miss:
                MissAmount++;
                comboBoxAnim.SetTrigger("Close");
                currentCombo = 0;
                break;
        }

        rankBoxAnim.SetTrigger(rank.ToString());

        comboTextBox.text = currentCombo.ToString();

        if (currentCombo > MaxCombo) MaxCombo = currentCombo;
        scoreTextBox.text = TotalScore.ToString();
    }

    private void GoToWinScreen()
    {
        SceneManager.LoadScene(resultsSceneIndex);
        scoreTextBox.text = "";
        rankBoxAnim.gameObject.SetActive(false);
        comboBoxAnim.gameObject.SetActive(false);
        scoreTextBox.transform.parent.gameObject.SetActive(false);
    }

    private void GoToLoseScreen()
    {
        SceneManager.LoadScene(lossSceneIndex);
        scoreTextBox.text = "";
        rankBoxAnim.gameObject.SetActive(false);
        comboBoxAnim.gameObject.SetActive(false);
        scoreTextBox.transform.parent.gameObject.SetActive(false);
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