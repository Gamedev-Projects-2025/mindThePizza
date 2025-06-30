using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static bool left = true;
    public static int timesReset = 0, timesPutWrongIngredient = 0, timeTakenToAssemble = 0, piesMade = 0, hintsGiven = 0, piesFailed = 0;
    public static float totalTime = 0;
    public static float timeLeft = 1;
    public static bool autoDeliver = true,  timedMatch = false;
    public static void setLeft()
        { left = true; }
    public static void setRight() 
        { left = false; }

    public static void setSide()
    {
        left = !left;
    }

    public static void usedReset()
    {
        timesReset++;
    }

    public static void setTimer(int timeInput)
    {
        timeLeft = timeInput;
    }

    public static void toggleTimedRounds()
    {
        timedMatch = !timedMatch;
    }

    public static void resetStats()
    {
        timesReset = 0;
        timesPutWrongIngredient = 0;
        timeTakenToAssemble = 0;
        piesMade = 0;
        hintsGiven = 0;
        piesFailed = 0;
        totalTime = 0;
        autoDeliver = true;  
        timedMatch = false;
        left = true;
    }
    public static string GetStatsString()
    {
        return $"Times Reset: {timesReset}\n" +
               $"Average Time Taken to Assemble: {timeTakenToAssemble} seconds\n" +
               $"Pies Made: {piesMade}\n" +
               $"Pies Failed: {piesFailed}";
    }

    public static void setAutoDeliver()
    {
        autoDeliver = !autoDeliver;
        
    }


}
