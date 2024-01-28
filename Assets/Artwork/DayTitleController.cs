using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayTitleController : MonoBehaviour
{
    public float duration = 4.0f;

    private void Start()
    {
        var txt = GameObject.Find("DayText").GetComponent<Text>();
        txt.text = $"MIKE LITTLE MESSES\nDAY: {GameManager.Instance.day}\nR$ {Math.Abs(GameManager.Instance.goal - GameManager.Instance.money)} REMAINING";
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
