using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyUnit : BattleUnit
{
    public static EnemyUnit Instance { get; private set; }

    [SerializeField] Image image;
    [SerializeField] Text nameText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }
    public override void Setup(Battler battler)
    {
        base.Setup(battler);
        //enemy:‰æ‘œ‚Æ–¼‘O‚Ìİ’è
        image.sprite = battler.Base.Sprite;
        nameText.text = battler.Base.Name;
    }

   

}
