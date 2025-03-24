using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static bool left = true;

    public static void setLeft()
        { left = true; }
    public static void setRight() 
        { left = false; }

}
