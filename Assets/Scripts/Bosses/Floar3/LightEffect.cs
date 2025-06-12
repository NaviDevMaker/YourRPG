using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[System.Serializable]
public class LightEffect
{
    [SerializeField] Image lightImage;
    [SerializeField] Transform barrier;

   



    public IEnumerator BarrierDestoryEvent()
    {
        Debug.Log("ê¨å˜");
        //lightImage.gameObject.SetActive(true);
        Color color = lightImage.color;
        color.a = 1.0f;

        
        for (int i = 0; i < 2; i++)
        {
            Debug.Log("äJén");
            color.a = 1.0f;
            lightImage.color = color;
            //lightImage.gameObject.SetActive(true);
            yield return null;
            yield return new WaitForSeconds(0.5f);
            //lightImage.gameObject.SetActive(false);
            color.a = 0.0f;
            lightImage.color = color;
            yield return null;
            yield return new WaitForSeconds(0.5f);

        }

        bool isCompleted = false;
        float alphaValue = 0f;
        while (!isCompleted)
        {
           
            alphaValue += 0.4f * Time.deltaTime;
            color.a = alphaValue;
            lightImage.color = color;
            yield return null;
            if (alphaValue >= 1.0f)
            {
                alphaValue = 1.0f;
                color.a = alphaValue;
                lightImage.color = color;
                yield return new WaitForSeconds(1.0f);
                isCompleted = true;
                
            }

        }

        while(isCompleted)
        {
            
            alphaValue -= 0.4f * Time.deltaTime;
            color.a = alphaValue;
            lightImage.color = color;
            yield return null;
            if (alphaValue <= 0.0f)
            {
                alphaValue = 0f;
                color.a = alphaValue;
                lightImage.color = color;
                yield return new WaitForSeconds(1.0f);
                isCompleted = false;
               
              


            }

            
        }

        while (barrier.localScale.y <= 40)
        {
            Vector2 barrierScale = new Vector2(barrier.localScale.x + 2.0f, barrier.localScale.y + 2.0f);
            barrier.localScale = barrierScale;
            yield return null;
         
        }

        Renderer barrierRenderer = barrier.gameObject.GetComponent<Renderer>();
        Color barrierColor = barrierRenderer.material.color;
        barrierColor.a = 0f;
        barrierRenderer.material.color = barrierColor;

        yield return null;

     

      
   
       
    }
}
