using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class SettingsPanel : Singleton<SettingsPanel>
{
    //[SerializeField] private PlayerMovement player;
    private int score = 0;
    [SerializeField] private Text txtScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        txtScore.text = "Score: " + score;
    }
}
