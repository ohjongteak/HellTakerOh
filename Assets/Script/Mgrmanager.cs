using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mgrmanager : MonoBehaviour
{

    public static Mgrmanager instance;
    public InGameUIManager mgrInGameManager;
    public CharacterMove mgrCharacterManager;
    public GameObject objCharacterManager;

    private void Awake()
    {
        if(instance == null )
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
            if (FindObjectOfType<Canvas>().name == "MainCanvas")
            {
                if (GameObject.Find("PlayerCharacter(Clone)") == null)
                {
                    GameObject objCharacter = Instantiate(objCharacterManager, FindObjectOfType<Canvas>().transform);

                    mgrCharacterManager = objCharacter.GetComponent<CharacterMove>();
                }
            }
            else
            {

                if (GameObject.Find("MainCanvas"))
                {
                    Canvas canvMain;

                    GameObject BackGround = GameObject.Find("MainCanvas");
                    canvMain = BackGround.transform.GetChild(0).GetComponent<Canvas>();
                    if (GameObject.Find("PlayerCharacter(Clone)") == null)
                    {
                        GameObject objCharacter = Instantiate(objCharacterManager, BackGround.transform);


                        mgrCharacterManager = objCharacter.GetComponent<CharacterMove>();
                    }
                }
            }
        }

    }


}
