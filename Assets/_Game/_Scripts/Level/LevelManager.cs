using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Ins;

    public PlayerController player;
    public List<Level> level;
    public int currentLevel;

    public CinemachineConfiner2D confiner2D;
    public CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        Ins = this;
    }

    private void Start()
    {
        SpawnLevel(currentLevel);
        confiner2D.m_BoundingShape2D = level[currentLevel].bouncingCollider;
        player.gameObject.transform.position = level[currentLevel].startPoint.transform.position;
    }

    void SpawnLevel(int curLevel)
    {
        GameObject levelPrefab = Instantiate(level[curLevel].gameObject, transform.position, transform.rotation);
    }
}
