using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static bool left = true;
    public static int timesReset=0, timesPutWrongIngredient=0, timeTakenToAssemble=0, piesMade=0, hintsGiven=0, piesFailed=0;

    public static void setLeft()
        { left = true; }
    public static void setRight() 
        { left = false; }

    public static void usedReset()
    {
        timesReset++;
    }

    public static void resetStats()
    {
        timesReset = 0;
        timesPutWrongIngredient = 0;
        timeTakenToAssemble = 0;
        piesMade = 0;
        hintsGiven = 0;
        piesFailed = 0;
    }
    public static string GetStatsString()
    {
        return $"Times Reset: {timesReset}\n" +
               $"Wrong Ingredients Used: {timesPutWrongIngredient}\n" +
               $"Time Taken to Assemble: {timeTakenToAssemble} seconds\n" +
               $"Pies Made: {piesMade}\n" +
               $"Hints Given: {hintsGiven}\n" +
               $"Pies Failed: {piesFailed}";
    }

}
