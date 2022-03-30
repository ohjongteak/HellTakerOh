using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ttt : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject aa;
    [SerializeField]
    GameObject bb;

    [SerializeField]
    Canvas canv;
    void Start()
    {
        aa = GameObject.Find("aa");

        for (int i = 0; i < aa.transform.childCount; i++)
        {
            if (aa.transform.GetChild(i).name == "bb")
                bb = aa.transform.GetChild(i).gameObject;
        }

        GameObject Fade;
       for(int i = 0; i <canv.transform.childCount;i++)
        {
           if(canv.transform.GetChild(i).name == "Fade")
            {
                Fade = canv.transform.GetChild(i).gameObject;

                Debug.Log(Fade);
            }

        }


    }

    // Update is called once per frame
   
}
