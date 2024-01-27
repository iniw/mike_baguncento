using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float score;
    public float groupedCarMoneyAmount;
    public TrafficLightsState currentTrafficLight;
    public float countdown;
    public List<Sprite> reactionSprites;
    public int reactionIndex;
    public List<Sprite> trafficLightsUISprites;
    
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
        score = float.Parse(data[2]);
        groupedCarMoneyAmount = float.Parse(data[3]);
        currentTrafficLight = (TrafficLightsState) int.Parse(data[4]);
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
            countdown = 0;
            currentTrafficLight = TrafficLightsState.Red;
        }
        SaveState();
    }

    public void SetTrafficLight(TrafficLightsState state)
    {
        currentTrafficLight = state;
    }
}
