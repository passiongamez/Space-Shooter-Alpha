using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] Text _scoreText;
    [SerializeField] Text _gameOver;
    [SerializeField] Text _restartText;
    [SerializeField] Text _ammoCountText;
    [SerializeField] Text _boomAmmoText;
    [SerializeField] Text _fuelText;
    [SerializeField] Text _waveText;

    [SerializeField] Image _livesIMG;
    [SerializeField] Image _boomAmmo;
    [SerializeField] Image _fuel;

    [SerializeField] Sprite[] _livesDisplay;

    WaitForSeconds _waveTextLength = new WaitForSeconds(2f);

    void Start()
    {
        _restartText.gameObject.SetActive(false);

        if(_restartText == null)
        {
            Debug.LogError("Restart is null");
        }

        _gameOver.enabled = false;

        if(_gameOver == null)
        {
            Debug.LogError("gameover is null");
        }

        _scoreText.text = "Score: " + 0;

        if(_scoreText == null)
        {
            Debug.LogError("Score text is null");
        }

        _boomAmmoText.text = "";

        _ammoCountText.text = "x15";

    }
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesIMG.sprite = _livesDisplay[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        StartCoroutine(GameOverFlicker());
        _restartText.gameObject.SetActive(true);
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOver.enabled = true;
            yield return new WaitForSeconds(.5f);
            _gameOver.enabled = false;
            yield return new WaitForSeconds(.5f);
        }
    }

    public void UpdateAmmoCount(int ammoCount)
    {
        _ammoCountText.text = "x" + ammoCount;
    }

    public void BoomAmmoUpdate(int ammoCount)
    {
        _boomAmmo.gameObject.SetActive(true);
        _boomAmmoText.text = "x" + ammoCount;
        if(ammoCount == 0)
        {
            _boomAmmo.gameObject.SetActive(false);
            _boomAmmoText.text = "";
        }
    }

    public void FuelUpdate(float fuel)
    {
        _fuelText.text = ":" + fuel.ToString();
    }

    public void UpdateWave(int waveNumber)
    {
        _waveText.text = "Wave " + waveNumber;
        StartCoroutine(WaveTextOff());
    }

    IEnumerator WaveTextOff()
    {
        yield return _waveTextLength;
        _waveText.text = "";
    }
}
