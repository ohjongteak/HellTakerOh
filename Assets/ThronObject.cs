using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ThronObject : MonoBehaviour
{
    [SerializeField]
    int startNodeIndex;

    // Start is called before the first frame update
    [SerializeField]
    private List<Tileproperty> listTileMap = new List<Tileproperty>();

    public bool dontActiveSpike;

    public bool onSpike;

    [SerializeField]
    Animator animSpike;

    [SerializeField]
    bool saveOnSpike;

    public int NowNodeIndex
    {
        get { return startNodeIndex; }
        set { startNodeIndex = value; }

    }

    private void Start()
    {
       

        if (LobbyManager.ActiveLoad == false)
        {
            listTileMap = Mgrmanager.instance.mgrInGameManager.GetListTileMap();

            this.transform.position = listTileMap[startNodeIndex].transform.position;
        }
        else
        {
            this.onSpike = Convert.ToBoolean(PlayerPrefs.GetString(this.gameObject.name));

            if(this.onSpike == true)
            {
                animSpike.SetBool("Down", false);
                animSpike.SetBool("Up", true);
             
            }

        }

    }

    public void UpAndDownSpike()
    {
       

        if(this.dontActiveSpike == false)
        {
          
            if (onSpike == true)
            {
                animSpike.SetBool("Down",true);
                animSpike.SetBool("Up", false);
                onSpike = false;
               
            }
            else if( onSpike == false)
            {   
                animSpike.SetBool("Down", false);
                animSpike.SetBool("Up", true);
                onSpike = true;
   
            }
        }

    }

    public void Reset_Spike()
    {
        if (animSpike != null)
        {
            onSpike = saveOnSpike;
            animSpike.Rebind();
        }
    }
}
