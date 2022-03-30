using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Soundmanager : MonoBehaviour
{
    [SerializeField]
    public  AudioClip adioLoginBackGround;
    [SerializeField]
    public  AudioClip adioInGameBackGround;

    [SerializeField]
    AudioSource adioSource;

   public static Soundmanager instance;

  
    public AudioClip adioPlayerKick;

   
    public AudioClip adioPlayerMove;

   
    public AudioClip adioPlayerSpike;

    public AudioClip adioKeyShy;

    public AudioClip adioGoldBox;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

      

    }
    // Start is called before the first frame update
    void Start()
    {
        adioSource = this.GetComponent<AudioSource>();

        adioSource.clip = adioLoginBackGround;

        adioSource.PlayOneShot(adioSource.clip);

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (SceneManager.GetActiveScene().name == "LobbyScnece")
        {
            if (adioSource != null)
                adioSource.Stop();

            adioSource.clip = adioLoginBackGround;
        }

        else
        {
            if (adioSource != null)
            {
                if (adioSource.clip != adioInGameBackGround)
                {
                    adioSource.Stop();

                    adioSource.clip = adioInGameBackGround;

                    adioSource.Play();
                }
            }
        }

    }

  

           

       
    

}
