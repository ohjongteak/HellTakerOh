using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectManager : MonoBehaviour
{
    Animator animReload;

    // Start is called before the first frame update
    private void Awake()
    {
        animReload = this.GetComponent<Animator>();

        StartCoroutine(CoDestroyObj());
    }
   
    // Update is called once per frame
    IEnumerator CoDestroyObj()
    {
        yield return new WaitUntil(() => animReload.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        {
            Destroy(this.gameObject);
            if(this.name == "DieEffect")
            Destroy(this.transform.parent.gameObject);
        }
    }
}
