using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class CharacterMove : MonoBehaviour
{
    [SerializeField]
    private GameObject objTileMap;
   
    public List<Tileproperty> listTileMap = new List<Tileproperty>();
    [SerializeField]
    private GameObject objPlayerCharacter;
    [SerializeField]
    private int startIndex;

    [SerializeField]
    private int nowNodeIndex;

    [SerializeField]
    List<MoveObjectProPerty> listMoveObjProPerty =new List<MoveObjectProPerty>();
    [SerializeField]
    List<ThronObject> listThronObject = new List<ThronObject>();

    Animator animCharacter;

    [SerializeField]
    AudioSource adioPlayer;

    [SerializeField]
    int characterMoveIndex;

    [SerializeField]
    public bool hasKey;

    [SerializeField]
    public int keyIndex;

    [SerializeField]
    public int goldKeyBoxIndex;

    [SerializeField]
    public GameObject cameraMain;

    void Start()
    {
        animCharacter = this.transform.GetChild(0).GetComponent<Animator>();

        listTileMap = Mgrmanager.instance.mgrInGameManager.GetListTileMap();

        objPlayerCharacter = this.gameObject;

        nowNodeIndex = startIndex;

        objPlayerCharacter.transform.position = listTileMap[startIndex].transform.position;

        DontDestroyOnLoad(this.gameObject);

        ResetMoveObj();

        adioPlayer = this.GetComponent<AudioSource>();

        Debug.Log(listTileMap.Count);

        transform.SetSiblingIndex(9);

        if (listTileMap.Count > 89)
        {
           
            characterMoveIndex = 10;
        }

        else
        {
           
            characterMoveIndex = 9;
        }

        if(listTileMap.Count > 130)
        {
            characterMoveIndex = 11;
        }
        if (SceneManager.GetActiveScene().name == "Chapter8")
        {
            this.transform.localScale = new Vector2(this.transform.localScale.x * 0.7f, this.transform.localScale.y * 0.7f);
            cameraMain = GameObject.Find("Main Camera");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.W))
        {
           
            MoveCharacter(-characterMoveIndex);
        }

        else if(Input.GetKeyDown(KeyCode.A))
        {
            MoveCharacter(-1);
            if(this.transform.GetChild(0).transform.rotation.y == 0)
            this.transform.GetChild(0).transform.Rotate(new Vector2(0, -180), 180);
        }

        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveCharacter(characterMoveIndex);
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            this.transform.GetChild(0).transform.localRotation = Quaternion.EulerRotation(0, 0, 0);
            MoveCharacter(1);
        }

        else if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log(this.transform.position);
        }

    }

    public void MoveCharacter(int moveIndex)
    {
        if(moveIndex == 1)
        this.transform.GetChild(0).transform.localRotation = Quaternion.EulerRotation(0, 0, 0);

        else if( moveIndex == -1)
        {
            if (this.transform.GetChild(0).transform.rotation.y == 0)
                this.transform.GetChild(0).transform.Rotate(new Vector2(0, -180), 180);
        }



        for (int i = 0; i < listMoveObjProPerty.Count; i++)
        {
          
           
            if (nowNodeIndex + moveIndex == listMoveObjProPerty[i].NowNodeIndex)
            {

               
                for (int z = 0; z < listThronObject.Count; z++)
                {
                    listThronObject[z].UpAndDownSpike();
                }

                listMoveObjProPerty[i].MoveObj(moveIndex);
                animCharacter.SetTrigger("Kick");
                Mgrmanager.instance.mgrInGameManager.CharacterMove -= 1;
                Mgrmanager.instance.mgrInGameManager.ChangeMoveCount();
                Instantiate(Mgrmanager.instance.mgrInGameManager.objAttackEffect, listTileMap[nowNodeIndex + moveIndex].transform.position, Quaternion.identity, Mgrmanager.instance.mgrInGameManager.canvMain.transform);

                PlayOnShotSound(Soundmanager.instance.adioPlayerKick);

                for (int z = 0; z < listThronObject.Count; z++)
                {

                    if (nowNodeIndex == listThronObject[z].NowNodeIndex)
                    {
                        if (listThronObject[z].onSpike == true || listThronObject[z].dontActiveSpike ==true)
                        {
                            Mgrmanager.instance.mgrInGameManager.CharacterMove -= 1;
                            Mgrmanager.instance.mgrInGameManager.ChangeMoveCount();

                            Vector3 playerPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

                            Instantiate(Mgrmanager.instance.mgrInGameManager.objPlayerBloodEffect, playerPosition, Quaternion.identity, Mgrmanager.instance.mgrInGameManager.canvMain.transform);

                            StartCoroutine(CoActiveCharacterBlood());

                            PlayOnShotSound(Soundmanager.instance.adioPlayerSpike);
                        }

                    }

                }
                return;
            }

        }

        if (listTileMap[nowNodeIndex + moveIndex].ActiveWall == false)
        {
           
            for (int i = 0; i < listThronObject.Count; i++)
            {
                listThronObject[i].UpAndDownSpike();
            }

            if (goldKeyBoxIndex ==  nowNodeIndex + moveIndex)
            {
                if(hasKey != true)
                {
                   
                    animCharacter.SetTrigger("Kick");
                    Mgrmanager.instance.mgrInGameManager.CharacterMove -= 1;
                    Mgrmanager.instance.mgrInGameManager.ChangeMoveCount();
                    Instantiate(Mgrmanager.instance.mgrInGameManager.objAttackEffect, listTileMap[nowNodeIndex + moveIndex].transform.position, Quaternion.identity, Mgrmanager.instance.mgrInGameManager.canvMain.transform);

                    PlayOnShotSound(Soundmanager.instance.adioPlayerKick);

                    return;
                }
                else
                {
                    GameObject.Find("GoldBox").gameObject.SetActive(false);
                    Instantiate(Mgrmanager.instance.mgrInGameManager.objHasKeyEffect, listTileMap[nowNodeIndex + moveIndex].transform.position, Quaternion.identity, Mgrmanager.instance.mgrInGameManager.canvMain.transform);

                    PlayOnShotSound(Soundmanager.instance.adioGoldBox);

                    adioPlayer.PlayOneShot(adioPlayer.clip);

                }

            }


            if(moveIndex == -1)
            {
                Instantiate(Mgrmanager.instance.mgrInGameManager.objSandAttackEffect, this.transform.position, Quaternion.Euler(0,-180,0), Mgrmanager.instance.mgrInGameManager.canvMain.transform);
            }

            else if(moveIndex == 1)
            {
                Instantiate(Mgrmanager.instance.mgrInGameManager.objSandAttackEffect, this.transform.position, Quaternion.identity, Mgrmanager.instance.mgrInGameManager.canvMain.transform);
            }

            else
            {
                if(this.transform.GetChild(0).transform.rotation.y == -1)
                {
                    Instantiate(Mgrmanager.instance.mgrInGameManager.objSandAttackEffect, this.transform.position, Quaternion.Euler(0, -180, 0), Mgrmanager.instance.mgrInGameManager.canvMain.transform);
                }
                else
                {
                    Instantiate(Mgrmanager.instance.mgrInGameManager.objSandAttackEffect, this.transform.position, Quaternion.identity, Mgrmanager.instance.mgrInGameManager.canvMain.transform);
                }

            }

            

            for (int i = 0; i < listThronObject.Count; i++)
            {
              
                if (nowNodeIndex + moveIndex == listThronObject[i].NowNodeIndex)
                {
                    if (listThronObject[i].dontActiveSpike == true)
                    {

                        Mgrmanager.instance.mgrInGameManager.CharacterMove -= 1;
                        Mgrmanager.instance.mgrInGameManager.ChangeMoveCount();

                        Vector3 playerPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

                        Instantiate(Mgrmanager.instance.mgrInGameManager.objPlayerBloodEffect, playerPosition, Quaternion.identity, Mgrmanager.instance.mgrInGameManager.canvMain.transform);

                        StartCoroutine(CoActiveCharacterBlood());

                        PlayOnShotSound(Soundmanager.instance.adioPlayerSpike);

                        adioPlayer.PlayOneShot(adioPlayer.clip);
                        break;
                    }

                    else if (listThronObject[i].dontActiveSpike == false)
                    {

                       if( listThronObject[i].onSpike == true)
                       {
                            Mgrmanager.instance.mgrInGameManager.CharacterMove -= 1;
                            Mgrmanager.instance.mgrInGameManager.ChangeMoveCount();

                            Vector3 playerPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

                            Instantiate(Mgrmanager.instance.mgrInGameManager.objPlayerBloodEffect, playerPosition, Quaternion.identity, Mgrmanager.instance.mgrInGameManager.canvMain.transform);

                            StartCoroutine(CoActiveCharacterBlood());

                            PlayOnShotSound(Soundmanager.instance.adioPlayerSpike);

                            adioPlayer.PlayOneShot(adioPlayer.clip);
                            break;

                       }


                    }

                    //break;
                }

            }

           

            StartCoroutine( CoMoveCharacter(moveIndex));

           
        }

    }

    IEnumerator CoMoveCharacter(int moveIndex)
    {
       
        adioPlayer.PlayOneShot(Soundmanager.instance.adioPlayerMove);

        if (SceneManager.GetActiveScene().name == "Chapter8")
        {
            float MoveSpeed = 100;
          
            if(moveIndex == 11)
            {
                cameraMain.transform.position = new Vector3(cameraMain.transform.position.x, cameraMain.transform.position.y - 100 ,-10);

            }
            else if(moveIndex == -11)
            {
                cameraMain.transform.position = new Vector3(cameraMain.transform.position.x, cameraMain.transform.position.y + 100, -10);
            }


            while (objPlayerCharacter.transform.position!= listTileMap[nowNodeIndex + moveIndex].transform.position)
            {
                objPlayerCharacter.transform.position = Vector2.MoveTowards(transform.position, listTileMap[nowNodeIndex + moveIndex ].transform.position, 1 + MoveSpeed);
                yield return new WaitForSeconds(0.02f);
            }

            Debug.Log(objPlayerCharacter.transform.localPosition+"플레");
            Debug.Log(listTileMap[nowNodeIndex + moveIndex].transform.localPosition + "타일맵");

            nowNodeIndex = nowNodeIndex + moveIndex;

        }

        else
        {
           


            while (objPlayerCharacter.transform.position != listTileMap[nowNodeIndex + moveIndex].transform.position)
            {
                objPlayerCharacter.transform.position = Vector3.MoveTowards(transform.position, listTileMap[nowNodeIndex + moveIndex].transform.position, 1);
                yield return new WaitForSeconds(0.02f);
            }

            nowNodeIndex = nowNodeIndex + moveIndex;

            if (nowNodeIndex == keyIndex && hasKey == false)
            {

                Instantiate(Mgrmanager.instance.mgrInGameManager.objHasKeyEffect, this.transform.position, Quaternion.identity, Mgrmanager.instance.mgrInGameManager.canvMain.transform);
                PlayOnShotSound(Soundmanager.instance.adioKeyShy);

                hasKey = true;
                if (GameObject.Find("Key") != null)
                    GameObject.Find("Key").gameObject.SetActive(false);

            }
        }

        Mgrmanager.instance.mgrInGameManager.CharacterMove -= 1;
        Mgrmanager.instance.mgrInGameManager.ChangeMoveCount();

    }

    IEnumerator CoActiveCharacterBlood()
    {
        this.transform.GetChild(0).GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        this.transform.GetChild(0).GetComponent<Image>().color = Color.white;

    }

    public List<MoveObjectProPerty> SetListMoveObj()
    {
         
        return listMoveObjProPerty;

    }

    public List<ThronObject> SetListThronObj()
    {

        return listThronObject;

    }

    public void Onbtn_Arrow(int MoveIndex)
    {
        MoveCharacter(MoveIndex);

        if(MoveIndex == -1)
        {
            if (this.transform.GetChild(0).transform.rotation.y == 0)
            {
                this.transform.GetChild(0).transform.Rotate(new Vector2(0, -180), 180);
            }
        }

        else if(MoveIndex == 1)
        {
            this.transform.GetChild(0).transform.localRotation = Quaternion.EulerRotation(0, 0, 0);
        }

    }

    public int CharacterNowNodeIndex
    {
        get { return nowNodeIndex; }
        set { nowNodeIndex = value; }
    }

    public int CharacterStartNodeIndex
    {
        get { return startIndex; }
        set { startIndex = value; }
    }

    public int CharacterMoveIndex
    {
        get { return characterMoveIndex; }
        set { characterMoveIndex = value; }

    }

    public void ResetMoveObj()
    {
        if (LobbyManager.ActiveLoad == false)
        {
            this.transform.position = listTileMap[startIndex].transform.position;
            nowNodeIndex = startIndex;

        }

        else if (LobbyManager.ActiveLoad == true)
        {
                nowNodeIndex = PlayerPrefs.GetInt("PlayerNowNodeIndex");
                this.transform.position = listTileMap[nowNodeIndex].transform.position;
        }

       

        this.transform.GetChild(0).transform.localRotation = Quaternion.EulerRotation(0, 0, 0);
    }

    public void PlayOnShotSound( AudioClip _adioClip)
    {
        if (adioPlayer.clip != _adioClip)
        {
           
            adioPlayer.clip = _adioClip;
        }

        adioPlayer.PlayOneShot(adioPlayer.clip);


    }

}
