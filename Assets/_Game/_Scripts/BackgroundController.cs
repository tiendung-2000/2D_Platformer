using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public SpriteRenderer layer1, layer2, layer3;
    //public Vector2 size;
    private void Start()
    {

    }

    public void SetUpBackground(int curLevel)
    {
        layer1.sprite = LevelManager.Ins.level[curLevel].layer1Spr;
        layer2.sprite = LevelManager.Ins.level[curLevel].layer2Spr;
        layer3.sprite = LevelManager.Ins.level[curLevel].layer3Spr;

        //layer1.size = new Vector2(100f, 10.8f);
        //layer2.size = new Vector2(100f, 10.8f);
        //layer3.size = new Vector2(100f, 10.8f);

        LevelManager.Ins.isHasBg = true;
    }
}
