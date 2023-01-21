using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObj : Singleton<MainObj>
{

    private string Used_Fold = "";
#if UNITY_Editor
   
#endif
    public int[] GameInfo_Arr = new int[18];

    public const int Credit = 0;
    public const int Bank = 1;
    public const int WinScore = 2;
    public const int Bet = 3;

    public const int Day_Credit = 4;
    public const int Day_BankDel = 5;
    public const int Day_Win = 6;
    public const int Day_WinDel = 7;

    public const int Total_Credit = 8;
    public const int Total_BankDel = 9;
    public const int Total_Win = 10;
    public const int Total_WinDel = 11;

    public const int Emergency1 = 12;
    public const int Emergency2 = 13;
    public const int Emergency3 = 14;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
