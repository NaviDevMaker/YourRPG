using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public static Camera Instance { get; private set; }
    [SerializeField] GameObject Player;
    Transform playerTransform;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else if(Instance != null)
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = Player.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
      transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }
}
