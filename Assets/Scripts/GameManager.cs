using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public enum GameState
{
    Menu,
    Gameplay,
    GameOver
}

public enum TrafficLightsState
{
    Red,
    Yellow,
    Green
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int money;
    public int goal;
    public int day;
    public int score;
    public float groupedCarMoneyAmount;
    public TrafficLightsState currentTrafficLight;
    public List<Sprite> reactionSprites;
    public int reactionIndex;
    public List<Sprite> trafficLightsUISprites;
    public CarMovement cars;
    public bool onMinigame = false;

    public float greenLightDuration = 10f;
    public float yellowLightDuration = 3f;
    public float redLightDuration = 10f;
    public float countdown;
    private TrafficLightsState _lastTrafficLight;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += LoadState;
    }

    public void SaveState()
    {
        var s = "";
        s += $"{money}|{day}|{score}|{groupedCarMoneyAmount}|{currentTrafficLight.GetHashCode()}|{countdown}|{reactionIndex}";
        PlayerPrefs.SetString("SaveState", s);
    }

    private void LoadState(Scene scene, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState")) return;

        var data = PlayerPrefs.GetString("SaveState").Split('|');
        money = int.Parse(data[0]);
        day = int.Parse(data[1]);
        score = int.Parse(data[2]);
        groupedCarMoneyAmount = float.Parse(data[3]);
        currentTrafficLight = (TrafficLightsState)int.Parse(data[4]);
        countdown = float.Parse(data[5]);
        reactionIndex = int.Parse(data[6]);
    }

    public void calcScore()
    {
        // TODO
    }

    private void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown <= 0)
        {
            _lastTrafficLight = currentTrafficLight;
            switch (currentTrafficLight)
            {
                case TrafficLightsState.Green:
                    currentTrafficLight = TrafficLightsState.Yellow;
                    countdown = yellowLightDuration;
                    break;
                case TrafficLightsState.Yellow:
                    currentTrafficLight = TrafficLightsState.Red;
                    countdown = redLightDuration;
                    break;
                case TrafficLightsState.Red:
                    currentTrafficLight = TrafficLightsState.Green;
                    countdown = greenLightDuration;
                    break;
            }
        }

        SaveState();

        if (onMinigame) return;

        cars.SetspeedMultiplier(currentTrafficLight != TrafficLightsState.Green ? 0.4f : 1.0f);

        if (_lastTrafficLight == TrafficLightsState.Red)
        {
            cars.LeaveScene();
            return;
        }
        cars.MoveToFinalPosition();
    }
}
