using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using SpriteRenderer = UnityEngine.SpriteRenderer;

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
    public static GameManager Instance = null;

    public int money;
    public float goal;
    public int day;
    public int score;
    public TrafficLightsState currentTrafficLight;
    public List<RuntimeAnimatorController> reactionSprites;
    public int reactionIndex;
    public List<Sprite> trafficLightsUISprites;
    private CarMovement _cars;

    public float greenLightDuration = 10f;
    public float yellowLightDuration = 3f;
    public float redLightDuration = 10f;
    public float countdown;
    private TrafficLightsState _lastTrafficLight;

    public int playableBalls = 0;
    public int spawnedBalls = 0;
    public Rigidbody2D[] balls;
    public List<Sprite> handSprites;

    public bool canProceed = false;
    public bool moveTillNext = false;

    public float ballsLinearDrag = 1f;
    public float ballsThrowMultiplier = 1.5f;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        LoadState(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        SceneManager.sceneLoaded += LoadState;
    }

    private void LoadState(Scene scene, LoadSceneMode mode)
    {
        if (Instance == null || this == null)
            return;

        if (scene.name == "Minigame")
            InitBalls();

        if (scene.name == "TopDown")
        {
            if (score <= 0)
                GameObject.Find("GoldBag").SetActive(false);

            if (_cars)
            {
                foreach (var car in GameObject.FindGameObjectsWithTag("Cars"))
                    if (car != _cars.gameObject)
                        Destroy(car);
            }
            else
            {
                _cars = GameObject.FindGameObjectWithTag("Cars").GetComponent<CarMovement>();
                DontDestroyOnLoad(_cars.gameObject);
            }
        }

        if (_cars)
            foreach (var sprite in _cars.GetComponentsInChildren<SpriteRenderer>())
                sprite.enabled = scene.name == "TopDown";
    }


    public void Destroy()
    {
        Destroy(Player.Instance.gameObject);
        Player.Instance = null;

        if (_cars)
            Destroy(_cars.gameObject);

        if (balls != null && balls.Length > 0)
            foreach (var ball in balls)
                Destroy(ball.gameObject);

        Destroy(gameObject);
        Instance = null;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "TopDown" || SceneManager.GetActiveScene().name == "Minigame")
            countdown -= Time.deltaTime;

        if (countdown <= 0)
        {
            _lastTrafficLight = currentTrafficLight;
            switch (currentTrafficLight)
            {
                case TrafficLightsState.Green:
                    currentTrafficLight = TrafficLightsState.Yellow;
                    countdown = yellowLightDuration;
                    if (_cars)
                        if (_cars.transform.position.x >= 0)
                            moveTillNext = true;
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

        if (_cars == null)
            return;

        _cars.SetspeedMultiplier(currentTrafficLight != TrafficLightsState.Green ? 0.4f : 1.0f);

        if (_lastTrafficLight == TrafficLightsState.Red || moveTillNext)
        {
            moveTillNext = _cars.LeaveScene();
            return;
        }

        _cars.MoveToFinalPosition();
    }

    private void InitBalls()
    {
        spawnedBalls = 0;
        playableBalls = 0;

        balls = GameObject.FindGameObjectsWithTag("Ball").Select(ball => ball.GetComponent<Rigidbody2D>()).ToArray();
        foreach (Rigidbody2D ball in balls)
            ball.gameObject.SetActive(false);
        SpawnBall();
    }

    public void SpawnBall()
    {
        balls[spawnedBalls].gameObject.SetActive(true);
        balls[spawnedBalls].GetComponent<Rigidbody2D>().drag = ballsLinearDrag;

        spawnedBalls++;
        playableBalls++;
    }

    public void StartGame()
    {
        currentTrafficLight = TrafficLightsState.Green;
        countdown = greenLightDuration;
        day = 1;
        score = 0;
        money = 0;
        SceneManager.LoadScene("TopDown");
    }
}
