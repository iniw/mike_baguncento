using System;
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
    public Image reactionSprite;

    private void Update()
    {
        countdownText.text = GameManager.Instance.countdown.ToString("F0");
        countdownText.gameObject.SetActive(GameManager.Instance.currentTrafficLight == TrafficLightsState.Red);
        scoreText.text = GameManager.Instance.score.ToString();
        scoreBar.transform.localScale = new Vector3(1, GameManager.Instance.score / 200f, 1);
        trafficLightSprite.sprite = GameManager.Instance.trafficLightsUISprites[(int)GameManager.Instance.currentTrafficLight];
        reactionSprite.sprite = GameManager.Instance.reactionSprites[GameManager.Instance.reactionIndex];

        if (moneyText != null)
            moneyText.text = $"$ {GameManager.Instance.money.ToString()} / {GameManager.Instance.goal.ToString()}";
        if (dayText != null)
            dayText.text = $"DAY {GameManager.Instance.day.ToString()} / 3";
    }
}
