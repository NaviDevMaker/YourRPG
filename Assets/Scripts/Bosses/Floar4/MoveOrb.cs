using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOrb : MonoBehaviour
{
    float upSpeed = 1.5f;
    public ParticleSystem OrbPrt;
    float reachPosY = 25.0f;

    public ParticleSystem GeneratedPrt { get; private set;}
    public bool IsUped { get; private set; } = false;

    private void Start()
    {
       GeneratedPrt = Instantiate(OrbPrt);
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsUped) UpOrbPos();
        else Destroy(this.gameObject);
    }

    void UpOrbPos()
    {
        if (transform.position.y >= reachPosY)
        {
            IsUped = true;
            return;
        }

        Vector2 tmpPos = transform.position;
        Vector2 tmpPos2 = new Vector2(tmpPos.x, tmpPos.y - 1.5f);
        
        tmpPos += Vector2.up * upSpeed * Time.deltaTime;
        transform.position = tmpPos;
        GeneratedPrt.gameObject.transform.position = tmpPos2;
    }
}
