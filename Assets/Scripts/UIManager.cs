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
    [SerializeField] Text _homingMissileInstructions;
    [SerializeField] Text _theBoomInstructions;
    [SerializeField] Text _shootInstructions;
    [SerializeField] Text _movementInstructions;
    [SerializeField] Text _powerUpInstructions;
    [SerializeField] Text _thrusterInstructions;
    [SerializeField] Text _finalInstructions;

    [SerializeField] Image _livesIMG;
    [SerializeField] Image _boomAmmo;
    [SerializeField] Image _fuel;

    [SerializeField] Sprite[] _livesDisplay;

    WaitForSeconds _waveTextLength = new WaitForSeconds(2f);
    WaitForSeconds _tutorialTextLength = new WaitForSeconds(3f);

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

        StartCoroutine(StartInstructions());

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

    public IEnumerator HomingMissileTutorial()
    {
        _homingMissileInstructions.gameObject.SetActive(true);
        yield return _waveTextLength;
        _homingMissileInstructions.gameObject.SetActive(false);
    }

    public IEnumerator TheBoomTutorial()
    {
        _theBoomInstructions.gameObject.SetActive(true);
        yield return _waveTextLength;
        _theBoomInstructions.gameObject.SetActive(false);
    }

    public IEnumerator StartInstructions()
    {
        yield return _tutorialTextLength;
        _movementInstructions.gameObject.SetActive(false);
        yield return _tutorialTextLength;
        _thrusterInstructions.gameObject.SetActive(true);
        yield return _tutorialTextLength;
        _thrusterInstructions.gameObject.SetActive(false);
        yield return _tutorialTextLength;
        _powerUpInstructions.gameObject.SetActive(true);
        yield return _tutorialTextLength;
        _powerUpInstructions.gameObject.SetActive(false);
        yield return _tutorialTextLength;
        _shootInstructions.gameObject.SetActive(true);
        yield return _tutorialTextLength;
        _shootInstructions.gameObject.SetActive(false);
        yield return _tutorialTextLength;
        _finalInstructions.gameObject.SetActive(true);
        yield return _tutorialTextLength;
        _finalInstructions.gameObject.SetActive(false);
    }
}
