using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _livesDisplay;
    [SerializeField] private Sprite[] _liveSprites;

    [SerializeField] private Text _gameOverText;

    [SerializeField] private Text _restartGame;

    [SerializeField] private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _livesDisplay.sprite = _liveSprites[3];
        _gameOverText.gameObject.SetActive(false);
        _restartGame.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager not found!");
        }
    }

    public void UpdateScore(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
    }

    public void UpdateLives(int currentLive)
    {
        _livesDisplay.sprite = _liveSprites[currentLive];
        if (currentLive == 0)
        {
            ShowGameOver();
        }
    }

    void ShowGameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartGame.gameObject.SetActive(true);
        _restartGame.text = "Press 'R' to restart the game";
        StartCoroutine(GameOverFlicker());
        _gameManager.GameOver();
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.4f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.4f);
        }
    }
}
