using TMPro;
using UnityEngine;

public class setTimer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setRoundTime()
    {
        int time = 60;
        if (gameObject.GetComponent<TMP_InputField>().text.Length != 0)
        {
            time = int.Parse(gameObject.GetComponent<TMP_InputField>().text) * 60;
        }
        gameManager.timeLeft = time;
    }
}
