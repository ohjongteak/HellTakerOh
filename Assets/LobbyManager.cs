using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    GameObject ReloadObj;

    [SerializeField]
    Animator animReload;

    public static bool ActiveLoad;

    LobbyManager instance;

    // Start is called before the first frame update

    private void Awake()
    {

        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
    }


    // Update is called once per frame
    public void OnBtn_NewGame()
    {
        ReloadObj.gameObject.SetActive(true);

        StartCoroutine(CoStartLoad());
    }

    public void OnBtn_LoadGame()
    {
        ReloadObj.gameObject.SetActive(true);

        ActiveLoad = true;
      
        StartCoroutine(CoStartLoad());

    }

    public void OnBtn_Quit()
    {
        Application.Quit();
    }

    IEnumerator CoStartLoad()
    {
        yield return new WaitUntil(() => animReload.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f); //animReload.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;

        if (ActiveLoad == false)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync("Chapter1");
        }

        else if(ActiveLoad == true)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(PlayerPrefs.GetString("NowStage"));
        }


        StartCoroutine(CoResetActiveload());
     
    }

    IEnumerator CoResetActiveload()
    {
        yield return new WaitForSeconds(1.0f);
        ActiveLoad = false;
    }
}
