using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAndWepons : MonoBehaviour
{
    public static ItemAndWepons Instance { get; private set; }
    public List<ItemMoveBase> AllItems  => allItems;
    public List<WeponBase> AllWepons => allWepons;

    [SerializeField] List<ItemMoveBase> allItems;
    [SerializeField] List<WeponBase> allWepons;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
