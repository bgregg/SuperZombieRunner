﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private bool gameStarted;
    private TimeManager timeManager;

    private GameObject player;
    private GameObject floor;
    private Spawner spawner;

    void Awake()
    {
        floor = GameObject.Find("Foreground");
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        timeManager = GetComponent<TimeManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        var floorHeight = floor.transform.localScale.y;
        var pos = floor.transform.position;
        pos.x = 0;
        pos.y = -((Screen.height/PixelPerfectCamera.pixelsToUnits) / 2) + (floorHeight / 2);

        floor.transform.position = pos;

        spawner.active = false;

        ResetGame();

        //here's a comment to track changes
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameStarted && Time.timeScale == 0)
        {
            if(Input.anyKeyDown)
            {
                timeManager.ManipulateTime(1, 1f);
                ResetGame();
            }
        }
    }

    void OnPlayerKilled()
    {
        spawner.active = false;

        var playerDestroyScript = player.GetComponent<DestroyOffScreen>();
        playerDestroyScript.DestroyCallback -= OnPlayerKilled;

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        timeManager.ManipulateTime(0, 5.5f);

        gameStarted = false;

    }

    void ResetGame()
    {
        spawner.active = true;

        player = GameObjectUtility.Instantiate(playerPrefab, new Vector3(0, (Screen.height/PixelPerfectCamera.pixelsToUnits) / 2 + 100, 0));

        var playerDestroyScript = player.GetComponent<DestroyOffScreen>();
        playerDestroyScript.DestroyCallback += OnPlayerKilled;

        gameStarted = true;
    }
}
