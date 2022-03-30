using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
public class InGameUIManager : MonoBehaviour
{
    [SerializeField]
    private Image imgBackGround;

    [SerializeField]
    private GameObject objTileMap;

    [SerializeField]
    private List<Tileproperty> listTileMap = new List<Tileproperty>();

    [SerializeField]
    private GameObject objMove;

    [SerializeField]
    private GameObject objThron;

    [SerializeField]
    private List<MoveObjectProPerty> listMoveObj = new List<MoveObjectProPerty>();

    [SerializeField]
    private List<ThronObject> listThronObj = new List<ThronObject>();

    [SerializeField]
    private CharacterMove characterMove;

    [SerializeField]
    int characterMoveCount;

    [SerializeField]
    TextMeshProUGUI textCharacterMoveCount;

    [SerializeField]
    TextMeshProUGUI textCharacterStageCount;

    [SerializeField]
    int gameClearNodeIndex;

    [SerializeField]
    Animator animReload;

    [SerializeField]
    int nowStage;

    public static InGameUIManager instance;


    [SerializeField]
    private List<int> listMoveStageCount = new List<int>();

    [SerializeField]
    private List<int> listStartStagePlayerIndex = new List<int>();

    [SerializeField]
    private List<int> listStageClearIndex = new List<int>();

  
    public GameObject objAttackEffect;

   
    public Canvas canvMain;


    public GameObject objSandAttackEffect;

    public GameObject objClearFail;

    [SerializeField]
    GameObject objClearChain;

    [SerializeField]
    GameObject ReloadBackGround;

    
    public GameObject objPlayerBloodEffect;

 
    public GameObject objHasKeyEffect;

    public GameObject objKeyPad;


    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

  
       

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (SceneManager.GetActiveScene().name != "LobbyScnece")
        {
            objTileMap = GameObject.Find("BackGround").transform.GetChild(0).gameObject;
            objMove = GameObject.Find("MoveObjects");

            if (GameObject.Find("ThronObjects") != null)
            {
                objThron = GameObject.Find("ThronObjects");
            }

            characterMove = Mgrmanager.instance.mgrCharacterManager;

            objKeyPad = GameObject.Find("KeyPad");

            objKeyPad.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => characterMove.MoveCharacter(-characterMove.CharacterMoveIndex));
            objKeyPad.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => characterMove.MoveCharacter(characterMove.CharacterMoveIndex));
            objKeyPad.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => characterMove.MoveCharacter(-1));
            objKeyPad.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => characterMove.MoveCharacter(1));

            textCharacterMoveCount = GameObject.Find("MoveCount").GetComponent<TextMeshProUGUI>();
            textCharacterStageCount = GameObject.Find("StageText").GetComponent<TextMeshProUGUI>();
            Button Btn_Restart = GameObject.Find("RestartButton").GetComponent<Button>();
            Btn_Restart.onClick.AddListener(() => Onbtn_ReStart());
         

            listMoveObj.RemoveRange(0, listMoveObj.Count);
            listTileMap.RemoveRange(0, listTileMap.Count);
            listThronObj.RemoveRange(0, listThronObj.Count);

            if (FindObjectOfType<Canvas>().name == "MainCanvas")
            {
                canvMain = FindObjectOfType<Canvas>();

                Debug.Log("들어옴?1");
            }

            if (canvMain == null)
            {

                GameObject canv = GameObject.Find("MainCanvas");
               

                canvMain = canv.GetComponent<Canvas>();

                Debug.Log("들어옴?2");

            }

            if(SceneManager.GetActiveScene().name != "Chapter8")
            animReload = Instantiate(ReloadBackGround, canvMain.transform).GetComponent<Animator>();

            else if(SceneManager.GetActiveScene().name == "Chapter8")
            {
                animReload = Instantiate(ReloadBackGround, GameObject.Find("Canvas").transform).GetComponent<Animator>();
            }

            for (int i = 0; i < objTileMap.transform.childCount; i++)
            {
                listTileMap.Add(objTileMap.transform.GetChild(i).GetComponent<Tileproperty>());
            }

            for (int i = 0; i < objMove.transform.childCount; i++)
            {
                listMoveObj.Add(objMove.transform.GetChild(i).GetComponent<MoveObjectProPerty>());
            }



            if (objThron != null)
            {
                for (int i = 0; i < objThron.transform.childCount; i++)
                {
                    listThronObj.Add(objThron.transform.GetChild(i).GetComponent<ThronObject>());

                    
                }
                for (int i = 0; i < objThron.transform.childCount; i++)
                {
                    for (int z = 0; z < listMoveObj.Count; z++)
                    {
                        listMoveObj[z].SetListSpikeObj().Add(listThronObj[i]);

                    }
                }

            }

            nowStage = int.Parse(SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length - 1));


            for (int i = 0; i < listStageClearIndex.Count; i++)
            {
                if (i == nowStage -1)
                {
                   
                    gameClearNodeIndex = listStageClearIndex[i];
                }; 
            }

            if (LobbyManager.ActiveLoad == false)
            {
                Debug.Log(LobbyManager.ActiveLoad);
                for (int i = 0; i < listMoveStageCount.Count; i++)
                {
                    if (nowStage -1 == i)
                    {
                        
                        characterMoveCount = listMoveStageCount[i];
                        characterMove.CharacterStartNodeIndex = listStartStagePlayerIndex[i];
                       
                        break;
                    }

                }
            }
            else
            {
              
                
                characterMoveCount = PlayerPrefs.GetInt("PlayerMoveCount");
                characterMove.hasKey = Convert.ToBoolean(PlayerPrefs.GetString("HasKey"));
                characterMove.CharacterStartNodeIndex = PlayerPrefs.GetInt("PlayerNowNodeIndex");
            

                if (characterMove.hasKey == true)
                {
                    GameObject.Find("Key").gameObject.SetActive(false);
                }

                if (GameObject.Find("GoldBox") != null)
                {
                    GameObject.Find("GoldBox").gameObject.SetActive(Convert.ToBoolean(PlayerPrefs.GetString("GoldBox")));

                }
               
            }

            Button Btn_Save = null;
            Button Btn_Lobby = null;
            
            for (int i = 0; i < canvMain.transform.childCount; i++)
            {
                if (canvMain.transform.GetChild(i).name == "OptionPanel")
                {

                    Btn_Save = canvMain.transform.GetChild(i).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).GetComponent<Button>();
                    Btn_Save.onClick.AddListener(() => Onbtn_Save());
                    Btn_Lobby = canvMain.transform.GetChild(i).transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).GetComponent<Button>();
                    Btn_Lobby.onClick.AddListener(() => OnBtn_Lobby());
                    //OnBtn_Lobby()
                    break;
                }
                
            }

            if (Btn_Save == null)
            {
                GameObject objUICanvas = GameObject.Find("Canvas");

                Btn_Save = objUICanvas.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).GetComponent<Button>();
                Btn_Save.onClick.AddListener(() => Onbtn_Save());
                Btn_Lobby = objUICanvas.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).transform.GetChild(1).GetComponent<Button>();
                Btn_Lobby.onClick.AddListener(() => OnBtn_Lobby());


            }

            if (GameObject.Find("Key") != null)
            {
                
                characterMove.keyIndex = GameObject.Find("Key").GetComponent<GoldKey>().NowNodeIndex;

                characterMove.goldKeyBoxIndex = GameObject.Find("GoldBox").GetComponent<GoldKeyBox>().NowNodeIndex;
            }

            Init_MoveObj();
         
        }

    }


    public List<Tileproperty> GetListTileMap()
    {
        return listTileMap;
    }

    public List<ThronObject> SetListThornObj()
    {
        return listThronObj;
    }

    public MoveObjectProPerty[] GetArrtMoveObj()
    {
        MoveObjectProPerty[] arrMoveObj = new MoveObjectProPerty[listMoveObj.Count];

        for(int i = 0; i< arrMoveObj.Length; i++)
        {
            arrMoveObj[i] = listMoveObj[i];
        }


        return arrMoveObj;
    }

     public void Init_MoveObj()
     {
       

        for (int i = 0; i < listMoveObj.Count; i++)
            listMoveObj[i].SetListMoveObj().RemoveRange(0, listMoveObj[i].SetListMoveObj().Count);

        characterMove.SetListMoveObj().RemoveRange(0, characterMove.SetListMoveObj().Count);

        for (int i = 0; i < listMoveObj.Count; i++)
        {
            if (listMoveObj[i].gameObject.activeInHierarchy != false)
            {
                characterMove.SetListMoveObj().Add(listMoveObj[i]);
            }

            for (int z = 0; z < listMoveObj.Count; z++)
            {
                listMoveObj[i].SetListMoveObj().Add(listMoveObj[z]);
            }

        }

        characterMove.SetListThronObj().RemoveRange(0, characterMove.SetListThronObj().Count);

        for (int i = 0; i < listThronObj.Count; i++)
        {
         
            characterMove.SetListThronObj().Add(listThronObj[i]);
        }

        if (characterMoveCount >= 0)
        {
            textCharacterMoveCount.text = "" + characterMoveCount;
        }
        else
        {
            textCharacterMoveCount.text = "X";
        }
        string strNowScence = SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length - 1);

        int indexNextScence = int.Parse(strNowScence);

    
        textCharacterStageCount.text = indexNextScence.ToString();
    }


    public int CharacterMove
    {
        get { return characterMoveCount;}
        set { characterMoveCount = value; }
       
    }

    public void ChangeMoveCount()
    {
        Debug.Log("야");

        if (characterMoveCount >= 0)
        {
            textCharacterMoveCount.text = "" + characterMoveCount;
        }
        else
        {
            textCharacterMoveCount.text = "X";
        }

        for (int i = 0; i < listMoveObj.Count;i++)
        {
            listMoveObj[i].CheckSpike();
        }

        if(characterMoveCount < -1)
        {
           if(Mgrmanager.instance.mgrCharacterManager.CharacterNowNodeIndex != gameClearNodeIndex)
           {
                if (SceneManager.GetActiveScene().name != "Chapter8")
                {
                    GameObject objclearFail = Instantiate(objClearFail, Mgrmanager.instance.mgrInGameManager.canvMain.transform);
                    objclearFail.transform.GetChild(0).transform.position = new Vector2(characterMove.transform.GetChild(0).transform.position.x, characterMove.transform.GetChild(0).transform.position.y + 1);
                    DiePlayer(objclearFail.transform.GetChild(0).GetComponent<Animator>());
                }
                else
                {
                    GameObject objclearFail = Instantiate(objClearFail, GameObject.Find("Canvas").transform);
                    objclearFail.transform.GetChild(0).transform.localPosition = new Vector3(characterMove.transform.GetChild(0).transform.localPosition.x, characterMove.transform.GetChild(0).transform.localPosition.y -300, -9);
                    DiePlayer(objclearFail.transform.GetChild(0).GetComponent<Animator>());

                    characterMove.cameraMain.transform.position = new Vector3(38, -464, -10); 
                }

           }
        }
        else
        {
            if(Mgrmanager.instance.mgrCharacterManager.CharacterNowNodeIndex == gameClearNodeIndex)
            {         
               GameObject objClear= Instantiate(objClearChain, FindObjectOfType<Canvas>().transform);

                objClear.gameObject.SetActive(true);
                StartCoroutine(CoNextStage(objClear.GetComponent<Animator>()));
                Debug.Log("클리어 성공");

            }

          
        }
    }

    IEnumerator CoNextStage(Animator _animClearPlayer)
    {   

        yield return new WaitUntil(() => _animClearPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        string strNowScence = SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.Length -1);

        int indexNextScence = int.Parse(strNowScence) + 1;

        string strNextScence = indexNextScence.ToString();

        string LastIndex = "10";

        if (LastIndex != strNextScence)
        {

            AsyncOperation op = SceneManager.LoadSceneAsync("Chapter" + strNextScence);
        }
        else
        {
            Image objFade;

           
            for (int i = 0; i <canvMain.transform.childCount; i++)
            {
                if(canvMain.transform.GetChild(i).name =="Fade")
                {
                   objFade = canvMain.transform.GetChild(i).GetComponent<Image>();

                    objFade.gameObject.SetActive(true);

                    objFade.transform.SetSiblingIndex(canvMain.transform.childCount);

                    while (objFade.color.a < 1)
                    {
                        objFade.color = new Color(0, 0, 0,objFade.color.a + 0.01f);
                        yield return new WaitForSeconds(0.01f);
                    }

                }
            }
           
          
            
         
           

        }
        
    }

    public void Onbtn_Save()
    {
        PlayerPrefs.SetInt("PlayerMoveCount", characterMoveCount);
 
        PlayerPrefs.SetInt("PlayerNowNodeIndex", Mgrmanager.instance.mgrCharacterManager.CharacterNowNodeIndex);

        PlayerPrefs.SetString("NowStage", SceneManager.GetActiveScene().name);

        PlayerPrefs.SetString("HasKey", characterMove.hasKey.ToString());


        if(GameObject.Find("GoldBox") == null)
        {
            PlayerPrefs.SetString("GoldBox", "False");
        }

        else
        {
            PlayerPrefs.SetString("GoldBox", "True");
        }

        Debug.Log(PlayerPrefs.GetString("NowStage"));

        
        for(int i = 0; i <listMoveObj.Count;i++)
        {
            PlayerPrefs.SetInt(listMoveObj[i].gameObject.name , listMoveObj[i].NowNodeIndex);
        }

        for(int i = 0; i < listThronObj.Count; i++)
        {
            PlayerPrefs.SetString(listThronObj[i].gameObject.name, listThronObj[i].onSpike.ToString());

        }
     
    }

    public void OnBtn_Lobby()
    {
        SceneManager.LoadSceneAsync("LobbyScnece");

       Destroy(GameObject.Find("LobbyManager"));

    }

    public void Onbtn_ReStart()
    {
        listMoveObj.RemoveRange(0, listMoveObj.Count);
        
        animReload.gameObject.SetActive(true);

        StartCoroutine(CoReStart());
    }

    public void DiePlayer(Animator _animDiePlayer)
    {
        StartCoroutine(CoPlayerDie(_animDiePlayer));
    }

    IEnumerator CoPlayerDie( Animator _animDiePlayer)
    {
        listMoveObj.RemoveRange(0, listMoveObj.Count);

        yield return new WaitUntil(() => _animDiePlayer.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);

        animReload.gameObject.SetActive(true);
        StartCoroutine( CoReStart());

    }


    IEnumerator CoReStart()
    {
      
        yield return new WaitUntil(() => animReload.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f);

        characterMove.hasKey = false;
        
        if(characterMove.cameraMain != null)
        characterMove.cameraMain.transform.position = new Vector3(38, -464, -10);

        for (int i = 0; i < canvMain.transform.childCount; i++)
        {
            if (canvMain.transform.GetChild(i).name == "Key")
            {
                canvMain.transform.GetChild(i).gameObject.SetActive(true);
            }

            if (canvMain.transform.GetChild(i).name == "GoldBox")
            {
                canvMain.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < objMove.transform.childCount; i++)
        {
            if (objMove.transform.GetChild(i).gameObject.activeInHierarchy == false)
            {
                objMove.transform.GetChild(i).gameObject.SetActive(true);
                
            }

            listMoveObj.Add(objMove.transform.GetChild(i).GetComponent<MoveObjectProPerty>());

            listMoveObj[i].NowNodeIndex = listMoveObj[i].StartIndex;
            listMoveObj[i].gameObject.transform.position = listTileMap[listMoveObj[i].NowNodeIndex].transform.position;

           if( listMoveObj[i].lookDirection == false)
           {
                listMoveObj[i].transform.GetChild(0).transform.localRotation = Quaternion.EulerRotation(0, 0, 0);
           }
           else
           {
                listMoveObj[i].transform.GetChild(0).transform.Rotate(new Vector2(0, -180), 180);
           }
            //listMoveObj[i].ResetMoveObj();
        }

        Debug.Log(nowStage);

        for (int i = 0; i < listStartStagePlayerIndex.Count; i++)
        {
            if (nowStage -1 == i)
            {
                Debug.Log("들어왔지?");
                characterMoveCount = listMoveStageCount[i ];
                characterMove.CharacterStartNodeIndex = listStartStagePlayerIndex[i ];
                characterMove.CharacterNowNodeIndex = characterMove.CharacterStartNodeIndex;
               
                break;
            }

        }

        for(int i = 0; i< listThronObj.Count; i++)
        {
            listThronObj[i].Reset_Spike();
        }

        Init_MoveObj();
        //characterMove.ResetMoveObj();
        characterMove.transform.position = listTileMap[characterMove.CharacterStartNodeIndex].transform.position;
        

        yield return new WaitUntil(() => animReload.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        animReload.gameObject.SetActive(false);

       
    }

}
