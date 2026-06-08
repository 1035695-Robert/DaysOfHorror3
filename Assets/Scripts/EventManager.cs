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

    public delegate void TossRound(int round);
    public static TossRound tossRound;
}
