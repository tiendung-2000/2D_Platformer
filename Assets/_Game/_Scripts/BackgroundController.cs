using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public SpriteRenderer layer1, layer2, layer3;
    //public float width = 100f, height = 10.8f;
    private void Start()
    {
        
    }

    public void SetUpBackground(int curLevel)
    {
        layer1.sprite = LevelManager.Ins.level[curLevel].layer1Spr;
        layer2.sprite = LevelManager.Ins.level[curLevel].layer2Spr;
        layer3.sprite = LevelManager.Ins.level[curLevel].layer3Spr;

        //layer1.drawMode.; 

        LevelManager.Ins.isHasBg = true;
    }
}
