using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ReturnToTown : MonoBehaviour
{
    bool isSceneChange = false;
    public static string  PreScenName { get; set; }

    [SerializeField] Vector2 playerPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if(player != null && !isSceneChange)
        {
            isSceneChange = true;
            CheckPreScene(player);
        }
    }

    

    async void CheckPreScene(PlayerController player)
    {
        player.Constraint = true;
        switch (PreScenName)
        {
            case "Town":
                await UniTask.Yield();
                await player.OpenSceneBases[0].ChangeScene(player.OpenSceneBases[0]).ToUniTask();
                break;
            case "Town1":
               
                await UniTask.Yield();
                await player.OpenSceneBases[6].ChangeScene(player.OpenSceneBases[6]).ToUniTask();
              
                break;

        }


        player.gameObject.transform.position = playerPos;
        await UniTask.Delay(1000);
        isSceneChange = false;
        player.Constraint = false;
    }
}
