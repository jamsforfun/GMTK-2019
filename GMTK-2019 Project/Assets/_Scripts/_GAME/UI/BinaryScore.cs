using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class BinaryScore : MonoBehaviour
{
    //variable contenant le score
    private int _score;

    //variable contenant le champ text dans lequel le score sera affiché (auto attribué si le script et déposé sur un gameobject avec un chmap d'UI Text)
    private Text _scoreText;

    private void Awake()
    {
        _scoreText = gameObject.GetComponent<Text>();
    }

    void Start()
    {
        _score = 0;
    }

    void Update()
    {
        _scoreText.text = Convert.ToString(_score, 2);
    }

    /// <summary>
    /// Score
    /// return the score
    /// </summary>
    public int Score
    {
        get { return _score; }
    }

    /// <summary>
    /// AddScore
    /// add value to the current score.
    /// you can add negative value to reduce it.
    /// </summary>
    public void AddScore(int p_scoreAdded)
    {
        _score += p_scoreAdded;
    }
}
