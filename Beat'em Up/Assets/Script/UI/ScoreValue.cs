using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreValue : MonoBehaviour
{
    public int _value;
    [SerializeField] private TextMeshProUGUI _scoreValue;
    private void Awake()
    {
    }
    void Start()
    {
        _value = 000000;
        _scoreValue.text = "" + _value;
    }

    public void ScoreAdd(int score)
    {
            _value += score;
            _scoreValue.text = "" + _value;
        if (_value > 1000)
        {

        }
    }
}
