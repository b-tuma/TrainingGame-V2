using UnityEngine;
using System.Collections;

public class Account : MonoBehaviour
{
    public static Account ac;
    public int currentMoney;

    public string[] contentName;
    public int[] contentPercent;


    void Awake()
    {
        if (ac != null)
        {
            Destroy(ac);
        }
        else
        {
            ac = this;
        }
    }
}
