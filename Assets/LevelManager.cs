using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager main;

    public Transform[] startPoints;
    public Transform[] path;


    private void Awake(){
        main = this;
    }

}
