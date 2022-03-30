using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldKeyBox : MonoBehaviour
{
    [SerializeField]
    int startNodeIndex;

    public int NowNodeIndex
    {
        get { return startNodeIndex; }
        set { startNodeIndex = value; }

    }


}
