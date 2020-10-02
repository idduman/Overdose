using OverdoseTheGame;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonBehaviour : MonoBehaviour
{
    private GameManager _gameManager;

    private Level _level;
    public Level Level
    {
        get
        {
            return _level;
        }
        set
        {
            if (Level == value) return;
            _level = value;
            GetComponentInChildren<Text>().text = Level.Number.ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Canvas").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectLevel()
    {
        _gameManager.SelectLevel(Level.Number);
    }
}
