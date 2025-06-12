using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class EndRollPlayerAnim : MonoBehaviour
{
    [SerializeField] Animator animator;

  
     public async void StartAnim(CancellationToken clt)
    {
        
        animator.SetBool("IsRolling", true);

        try
        {
            while (!clt.IsCancellationRequested)
            {
                await Task.Yield();
            }
        }
        finally
        {
            animator.SetBool("IsRolling", false);

        }

    }


}
