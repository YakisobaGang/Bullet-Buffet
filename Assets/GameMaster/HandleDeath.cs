﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameMaster
{
  public class HandleDeath : MonoBehaviour
  {
    private Player.Scripts.Player _player;

    private void Awake()
    {
      var aux =  GameObject.FindWithTag("Player");
      _player = aux.GetComponent<Player.Scripts.Player>();
    }

    private void Start()
    {
      _player.OnPlayerDeth += LoadDeathScreen;
    }

    private void LoadDeathScreen(object sender, EventArgs e)
    {
      SceneManager.LoadScene(3);
      Cursor.visible = true;
    }
  }
}
