using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class MoveObjectProPerty : MonoBehaviour
{
   
  
    public int StartIndex;

    [SerializeField]
    private int nowNodeIndex;

    [SerializeField]
    private List<Tileproperty> listTileMap = new List<Tileproperty>();

    [SerializeField]
    private GameObject objMe;

    [SerializeField]
    private bool CanBroken;

    [SerializeField]
    List<MoveObjectProPerty> listMoveObj = new List<MoveObjectProPerty>();

    [SerializeField]
    List<ThronObject> listSpikeObj = new List<ThronObject>();

    [SerializeField]
    private InGameUIManager mgrInGameUIManager;

    Animator animMonsterCharacter;

    [SerializeField]
    public bool lookDirection;

    // Start is called before the first frame update
    void Start()
    {
        mgrInGameUIManager = Mgrmanager.instance.mgrInGameManager;

        if (this.transform.GetChild(0).GetComponent<Animator>() != null)
        {
            animMonsterCharacter = this.transform.GetChild(0).GetComponent<Animator>();
        }

        objMe = this.gameObject;

 

      StartCoroutine(ResetMoveObj());

        if (this.gameObject.name.Substring(0, 5) != "Stone")
        {
            CanBroken = true;
        }

    }

    public int NowNodeIndex
    {
        get { return nowNodeIndex; }
        set { nowNodeIndex = value; }
        
    }

    public void MoveObj(int _changeNodeIndex)
    {
       
    
        if(CanBroken == true)
        {
            if(_changeNodeIndex == 1)//왼쪽방향
            {
                if (this.transform.GetChild(0).transform.rotation.y == 0)
                {
                    this.transform.GetChild(0).transform.Rotate(new Vector2(0, -180), 180);
                }

               
            }

            else if(_changeNodeIndex == -1)//오른쪽방향
            {
                this.transform.GetChild(0).transform.localRotation = Quaternion.EulerRotation(0, 0, 0);
            }

           // for(int i = 0; i<InGameUIManager.instance.)

        }


        if (listTileMap[nowNodeIndex + _changeNodeIndex].ActiveWall == true)
        {
            if(CanBroken == true) //뒤가 벽이고 내가 박살날수 있다면 박살나고 끝
            {
                this.gameObject.SetActive(false);
               
               
                this.nowNodeIndex = 999;

            }

            return;
        }

       
        else if (listTileMap[nowNodeIndex + _changeNodeIndex].ActiveWall == false)
        {
            int Check = 0;

            if (animMonsterCharacter != null)
            {
                animMonsterCharacter.SetTrigger("Hit");
            }
            for (int i = 0; i < listMoveObj.Count; i++)
            {
                //벽이 뚫려있고 뒤에 아무것도 없다면

                if (listMoveObj[i].NowNodeIndex != nowNodeIndex + _changeNodeIndex)
                {
                    Check++;
                }

            }

            if(Check >= listMoveObj.Count )//-1 혹은 0 
            {
                StartCoroutine(CoMoveObject(_changeNodeIndex));
               
            }

            else//벽이 뚫려있고 뒤에 무엇인가 있다면 
            {
                if(CanBroken == true)
                {

                    this.gameObject.SetActive(false);
                    mgrInGameUIManager.Init_MoveObj();
                
                    this.nowNodeIndex = 999;
                    //박살나라
                }
            }
        }
    }

    IEnumerator CoMoveObject(int _changeNodeIndex)
    {
        if (SceneManager.GetActiveScene().name == "Chapter8")
        {
            float MoveSpeed = 100;

            while (objMe.transform.position != listTileMap[nowNodeIndex + _changeNodeIndex].transform.position)
            {
                objMe.transform.position = Vector3.MoveTowards(transform.position, listTileMap[nowNodeIndex + _changeNodeIndex].transform.position, 1 +MoveSpeed);
                yield return new WaitForSeconds(0.02f);
            }

        }

        else
        {
            while (objMe.transform.position != listTileMap[nowNodeIndex + _changeNodeIndex].transform.position)
            {
                objMe.transform.position = Vector3.MoveTowards(transform.position, listTileMap[nowNodeIndex + _changeNodeIndex].transform.position, 1);
                yield return new WaitForSeconds(0.02f);
            }
        }

        nowNodeIndex += _changeNodeIndex;
    }


    public bool GetCanBroke()
    {
        return CanBroken;
    }

    public List<MoveObjectProPerty> SetListMoveObj()
    {
        return listMoveObj;
    }

    public List<ThronObject> SetListSpikeObj()
    {
        return listSpikeObj;
    }
    


   IEnumerator ResetMoveObj()
   {
        yield return new WaitForSeconds(0.1f);
        listTileMap = Mgrmanager.instance.mgrInGameManager.GetListTileMap();
        if (LobbyManager.ActiveLoad == false)
        {
         
            objMe.transform.position = listTileMap[StartIndex].transform.position;

            nowNodeIndex = StartIndex;

     

        }

        else if (LobbyManager.ActiveLoad == true)
        {
          
            if (PlayerPrefs.GetInt(this.gameObject.name) != 999)
            {
                nowNodeIndex = PlayerPrefs.GetInt(this.gameObject.name);
                objMe.transform.position = listTileMap[nowNodeIndex].transform.position;
                
            }
            else
            {
                nowNodeIndex = PlayerPrefs.GetInt(this.gameObject.name);

                this.gameObject.SetActive(false);
            }

            //LobbyManager.ActiveLoad = false;
        }
       
        this.transform.GetChild(0).transform.localRotation = Quaternion.EulerRotation(0, 0, 0);
    }

    public void CheckSpike()
    {
        if (this.CanBroken == true)
        {
            for (int i = 0; i < listSpikeObj.Count; i++)
            {
                if (listSpikeObj[i].NowNodeIndex == nowNodeIndex)
                {
                    if (listSpikeObj[i].onSpike == true)
                    {
                        this.gameObject.SetActive(false);
                        this.nowNodeIndex = 999;
                        mgrInGameUIManager.Init_MoveObj();
                       
                    }

                }
            }
        }
    }



}
