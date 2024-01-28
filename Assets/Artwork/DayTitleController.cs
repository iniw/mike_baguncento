using System;
using UnityEngine;
using UnityEngine.UI;

public class DayTitleController : MonoBehaviour
{
    public float duration = 4.0f;

    private void Start()
    {
        var txt = GameObject.Find("DayText").GetComponent<Text>();
        var moneyRemaining = GameManager.Instance.goal - GameManager.Instance.money;
        txt.text = $"MIKE LITTLE MESSES\nDAY: {GameManager.Instance.day}\nR$ {Math.Abs(moneyRemaining < 0 ? 0 : moneyRemaining)} REMAINING";
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
        }
    }
}
