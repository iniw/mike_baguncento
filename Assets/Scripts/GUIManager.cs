using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI scoreText;
    public GameObject scoreBar;
    public Image trafficLightSprite;
    public Animator reactionSprite;

    private float Normalize(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    private void Update()
    {
        if (!GameManager.Instance)
            return;

        countdownText.text = GameManager.Instance.countdown.ToString("F0");
        countdownText.gameObject.SetActive(GameManager.Instance.currentTrafficLight == TrafficLightsState.Red);
        scoreText.text = GameManager.Instance.score.ToString();

        var norm = Normalize(GameManager.Instance.score, 0, GameManager.Instance.goal);
        var progress = Mathf.Lerp(0f, 2.5f, norm);

        scoreBar.transform.localScale = new Vector3(1, progress, 1);
        trafficLightSprite.sprite = GameManager.Instance.trafficLightsUISprites[(int)GameManager.Instance.currentTrafficLight];
        reactionSprite.runtimeAnimatorController = GameManager.Instance.reactionSprites[GameManager.Instance.day - 1];

        if (moneyText != null)
            moneyText.text = $"$ {GameManager.Instance.money.ToString()} / {GameManager.Instance.goal.ToString()}";
        if (dayText != null)
            dayText.text = $"DAY {GameManager.Instance.day.ToString()} / 3";
    }
}
