using TMPro;
using UnityEngine;

public class clockScript : MonoBehaviour
{
    float timer = gameManager.timeLeft;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!gameManager.timedMatch)
        {
            gameObject.SetActive(false);
        }
        gameObject.GetComponent<TextMeshPro>().text = formatText(timer);
    }

    //time in seconds
    string formatText(float time)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);

        string result = minutes.ToString().PadLeft(2,'0') + ":" + seconds.ToString().PadLeft(2,'0');
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            gameObject.GetComponent<loadScene>().LoadGameScene();
        }

        gameObject.GetComponent<TextMeshPro>().text = formatText(timer);
    }
}
