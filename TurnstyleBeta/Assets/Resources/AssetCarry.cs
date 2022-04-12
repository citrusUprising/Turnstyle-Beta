using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetCarry : MonoBehaviour
{
    private static AssetCarry _i;
    public static AssetCarry i {
        get{
            if (_i ==null) _i = Instantiate(Resources.Load<AssetCarry>("AssetCarry"));
            return _i;
        }
    }

    public Transform popUp;
    public Transform status;
}

