using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreScript : MonoBehaviour
{
    public TextField _textField;
    private int _score;
    // Start is called before the first frame update
    void Start()
    {
        _textField = GetComponent<TextField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Addscore(int input)
    {
        _score += input;
        _textField.value = "Score: " + _score;
    }
}
