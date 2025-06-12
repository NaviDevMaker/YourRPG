using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncountBOSS : MonoBehaviour
{
    //
    //public static EncountBOSS Instance { get; private set;}
    [SerializeField] Battler bossBattler;
    [SerializeField] Canvas bossEncountCanvas;
   
   
    public Battler BossBattler { get => bossBattler;}
    public Canvas BossEncountCanvas { get => bossEncountCanvas;}

    public Battler GetBOSSBattler()
    {

        bossEncountCanvas.gameObject.SetActive(true);
        return bossBattler;
    }



   
}
