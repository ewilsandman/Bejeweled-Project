using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScoreScript : MonoBehaviour
{
    public Text text;
    private int _score;
    // Start is called before the first frame update
    void Start()
    {
        _score = 0;
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Addscore(int input)
    {
        _score += input;
        text.text = "Score: " + _score;
    }
}
