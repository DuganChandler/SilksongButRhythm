using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class ResultsScreen : MonoBehaviour
{
    [Header("In-Scene Elements")]
    [SerializeField] private TextMeshProUGUI scoreTextBox;
    [Space(5)]
    [SerializeField] private GameObject rankDisplayBox;
    [SerializeField] private TextMeshProUGUI perfectTextBox;
    [SerializeField] private TextMeshProUGUI goodTextBox;
    [SerializeField] private TextMeshProUGUI mehTextBox;
    [SerializeField] private TextMeshProUGUI missTextBox;
    [Space(5)]
    [SerializeField] private GameObject comboDisplayBox;
    [SerializeField] private TextMeshProUGUI comboTextBox;
    [Space(5)]
    [SerializeField] private GameObject pressContinueTextBox;

    [Header("Settings")]
    [SerializeField] private float countScoreTime = 3;
    [SerializeField] private float timeBetweenSections = 0.5f;

    [Header("Scenes")]
    [SerializeField] private int songSelectSceneIndex;

    [Header("Audio")]
    [SerializeField] private AudioClip sectionAudio;

    private Coroutine countTotalCoroutine;
    private AudioSource audioSource;

    private void Awake()
    {
        TryGetComponent(out audioSource);
    }

    private void Start()
    {
        if (countTotalCoroutine != null) StopCoroutine(countTotalCoroutine);

        countTotalCoroutine = StartCoroutine(CountTotalCoroutine());
    }

    private IEnumerator CountTotalCoroutine()
    {
        perfectTextBox.text = ScoreHolder.Instance.PerfectScoreAmount.ToString("000");
        goodTextBox.text = ScoreHolder.Instance.GoodScoreAmount.ToString("000");
        mehTextBox.text = ScoreHolder.Instance.MehScoreAmount.ToString("000");
        missTextBox.text = ScoreHolder.Instance.MissAmount.ToString("000");
        audioSource.PlayOneShot(sectionAudio);

        yield return new WaitForSeconds(timeBetweenSections);

        float timer = 0;
        float maxCombo = ScoreHolder.Instance.MaxCombo;
        while (timer < countScoreTime)
        {
            timer += Time.fixedDeltaTime;
            comboTextBox.text = $"{Mathf.Round(maxCombo * (timer / countScoreTime)):000}";
            yield return null;
        }
        comboTextBox.text = maxCombo.ToString("000");
        audioSource.PlayOneShot(sectionAudio);

        yield return new WaitForSeconds(timeBetweenSections);

        timer = 0;
        float totalScore = ScoreHolder.Instance.TotalScore;

        while (timer < countScoreTime)
        {
            timer += Time.fixedDeltaTime;
            scoreTextBox.text = $"{Mathf.Round(totalScore * (timer / countScoreTime)):000000}";
            yield return null;
        }
        scoreTextBox.text = totalScore.ToString("000000");
        audioSource.PlayOneShot(sectionAudio);

        yield return new WaitForSeconds(timeBetweenSections);

        pressContinueTextBox.SetActive(true);
        audioSource.PlayOneShot(sectionAudio);

        while (!Input.GetMouseButton(0))
        {
            yield return null;
        }

        SceneManager.LoadScene(songSelectSceneIndex);
        yield break;
    }
}
