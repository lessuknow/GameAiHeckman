using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour {

    int score = 0;
    public Text curScoreTxt, maxScoreTxt;
    public Text curScore, maxScore;
    public Button butn;

    public void Start()
    {
        string tmp = ApplicationData.HighScore.ToString();
        maxScoreTxt.text = "";
        for (int i = 0; i < (4 - tmp.Length); i++)
        {
            maxScoreTxt.text += "0";
        }

        maxScoreTxt.text += tmp;

        PositionText();

    }

    private void PositionText()
    {
        curScore.transform.position = new Vector3(curScore.preferredWidth / 2 + 10, Screen.height - curScore.preferredHeight);
        maxScore.transform.position = new Vector3(Screen.width - maxScore.preferredWidth / 2 - 10, curScore.transform.position.y);
        butn.transform.position = new Vector3(butn.GetComponent<RectTransform>().rect.width / 2,
            butn.GetComponent<RectTransform>().rect.height / 2);
    }

    private void Update()
    {
        string tmp = score.ToString();
        curScoreTxt.text = "";
        for (int i = 0;i < (4 - tmp.Length); i++)
        {
            curScoreTxt.text += "0";
        }

        curScoreTxt.text += tmp;
    }

    public void EndGame()
    {
        if(ApplicationData.HighScore < score)
        {
            ApplicationData.HighScore = score;
            maxScoreTxt.text = curScoreTxt.text;
        }
        
        score = 0;
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddScore()
    {
        score += 1;
    }

}
