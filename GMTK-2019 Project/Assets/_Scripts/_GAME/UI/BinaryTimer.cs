using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BinaryTimer : MonoBehaviour
{
    //variable contenant le Time.time auquel le timer à commencé
    private float _startingTimer;
    //variable contenant le timer se terminera
    private float _endingTimer;
    //variable servant à contenir le temps restant
    private float _timeRemaning;

    //indique si le timer et arrivé à terme
    private bool _timerEnded = false;

    //variable contenant le champ text dans lequel le timer sera affiché (auto attribué si le script et déposé sur un gameobject avec un chmap d'UI Text)
    private Text _timerText;

    //variable contenant la durée du timer
    [SerializeField]
    private float timerTime;
    //multiplicateur pour goflet le chiffre affiché au conteur
    [SerializeField]
    private int multiplicateur;


    private void Awake()
    {
        _timerText = gameObject.GetComponent<Text>();
    }

    private void Start()
    {
        _startingTimer = Time.time;
        _endingTimer = _startingTimer + timerTime;
    }

    private void Update()
    {
        _timeRemaning = _endingTimer - Time.time;

        if (_timeRemaning >= 0)
            _timerText.text = Convert.ToString((int)_timeRemaning * multiplicateur, 2);
        else
            _timerEnded = true;
    }


    /// <summary>
    /// TimeRemaning
    /// return the remaning time of the timer
    /// </summary>
    public float TimeRemaning
    {
        get { return _timeRemaning; }
    }

    /// <summary>
    /// TimerEnded
    /// return a boolean at true if the timer as reach 0
    /// </summary>
    public bool TimerEnded
    {
        get { return _timerEnded; }
    }
}
