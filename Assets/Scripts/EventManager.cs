using System.Collections;
using UnityEngine;

public class EventManager 
{

    //Gaga Ball
    public delegate void FleeBall();
    public static FleeBall flee;

    public delegate void Ball();
    public static Ball ballCheck;

    public delegate void FinalShowdown();
    public static FinalShowdown finalShowdown;



    //Character Movement
    public delegate void TossRound(int round);
    public static TossRound tossRound;

    public delegate void SetPlayerActions(GameObject target);
    public static SetPlayerActions holding;
    public static SetPlayerActions throwing;
    public static SetPlayerActions drop;
    public static SetPlayerActions Die;
}
