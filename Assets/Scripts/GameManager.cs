using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Ball _ball;
    private Paddle _paddle;
    private Brick[] _bricks;

    [Header("UI References")]
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _timerText;

    [Header("Audio")]
    [SerializeField] private AudioClip _gameOverClip;
    private int _score = 0;
    private int _lives = 1;
    private float _timer = 0.0f;

    private bool _isPaused;

    private void Awake()
    {
        _ball = FindObjectOfType<Ball>();
        _paddle = FindObjectOfType<Paddle>();
        _bricks = FindObjectsOfType<Brick>();
    }

    private void GameOver() => SceneManager.LoadScene("Game");

    public void Miss()
    {
        _lives--;

        if (_lives > 0)
            ResetLevel();
        else
        {
            if(_score > 0) Database.instance.AddNewScoreDatabase(_score);
            AudioManager.instance.PlayAudio(_gameOverClip);
            if (AudioManager.instance.isVibration) Vibration.Vibrate();
            GameOver();
        }

    }

    private void ResetLevel()
    {
        _paddle.ResetPaddle();
        _ball.ResetBall();
    }

    public void Hit(Brick brick)
    {
        _score += brick.points;
        _scoreText.text = _score.ToString();

        if (Cleared()) {
            GameOver();
        }
    }

    private bool Cleared()
    {
        for (int i = 0; i < _bricks.Length; i++)
        {
            if (_bricks[i].gameObject.activeInHierarchy && !_bricks[i].isBreakable)
                return false;
        }

        return true;
    }

    public void Pause(bool isPause)
    {
        _isPaused = isPause;
        Time.timeScale = !_isPaused ? 1.0f : 0.0f;
    }

    public void Exit()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }

    private void Update()
    {
        if (_isPaused) 
            return;

        if (Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu))
            SceneManager.LoadScene("Menu");

        _timer += Time.deltaTime * 1.0f;
        int sec = (int)(_timer % 60);
        int min = (int)(_timer / 60) % 60;

        _timerText.text = string.Format("00:{0:00}:{1:00}", min, sec);
    }

}
