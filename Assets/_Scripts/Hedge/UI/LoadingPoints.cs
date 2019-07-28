using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LoadingPoints : MonoBehaviour
{
    Text txtPoints;
    Coroutine cicle;
    private void Awake()
    {
        txtPoints = GetComponent<Text>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        txtPoints.text = "..";

        if(cicle!=null)StopCoroutine(cicle);
        cicle = StartCoroutine(Cicle());
    }
    
    private IEnumerator Cicle()
    {
        int i = 0;
        while(true)
        {
            if (i % 3 != 0)
            {
                txtPoints.text = txtPoints.text + ".";
               
            }
            else
            { txtPoints.text = "."; }
            i++;
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }


}
