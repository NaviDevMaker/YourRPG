using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneToNextBase : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] int passSceneChangeIndex;
    [SerializeField] Vector2 playerPos;
    //-0.36 -3.06

    // Start is called before the first frame update
    public virtual void Start()
    {
        player = PlayerController.Instance;
        Debug.Log($"ÉvÉåÉCÉÑÅ[ÇÕ{player}");
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        ReturnToTown.PreScenName = SceneManager.GetActiveScene().name;
       StartCoroutine(CollsionMethod(collision, passSceneChangeIndex));
    }
    IEnumerator CollsionMethod(Collision2D collision, int passSceneChangeIndex)
    {
            if (collision.gameObject == player.gameObject)
            {
                Debug.Log("enter");
                player.Constraint = true;
                yield return player.OpenSceneBases[passSceneChangeIndex].ChangeScene(player.OpenSceneBases[passSceneChangeIndex]);
                player.gameObject.transform.position = playerPos;
                yield return new WaitForSeconds(1.0f);
                player.Constraint = false;
            }
        
    }


}
