using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreText : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private void Update()
    {
        scoreText.text = Level.GetInstance().GetPipePassedCount().ToString();
    }
}
