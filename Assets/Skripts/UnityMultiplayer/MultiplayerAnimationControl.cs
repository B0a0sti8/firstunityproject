using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Animations;

public class MultiplayerAnimationControl : MonoBehaviour
{
    public List<RuntimeAnimatorController> allClassAnimationControllers;

    // Start is called before the first frame update
    void Start()
    {
        //allClassAnimationControllers.ForEach(el => Debug.Log(el.name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
