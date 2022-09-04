using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        // Disable the best score text as a starting position. CommunicateScore will enable
        // it if need be, based on current score landscape.
        BestScoreText.enabled = false;
        // Set the current score to 0.
        SaveDataHandler.Instance.CurrentPlayerScore = 0;
        // Communicate the current score.
        CommunicateScore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void AddPoint(int point)
    {
        // Delegate adding the score to the save data score handler.
        // For efficiency purposes, we also use the save data handler as the scorekeeper.
        SaveDataHandler.Instance.AddToScore(point);
        CommunicateScore();
    }

    void CommunicateScore()
    {
        ScoreText.text = $"Score for {SaveDataHandler.Instance.CurrentPlayerName}: {SaveDataHandler.Instance.CurrentPlayerScore}";
        if (SaveDataHandler.Instance.BestPlayerScore > 0)
        {
            BestScoreText.enabled = true;
            BestScoreText.text = $"Top score for {SaveDataHandler.Instance.BestPlayerName}: {SaveDataHandler.Instance.BestPlayerScore}";
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        // Save the game data.
        SaveDataHandler.Instance.SavePlayerData();
    }
}
