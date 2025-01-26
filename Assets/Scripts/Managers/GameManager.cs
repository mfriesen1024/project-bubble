using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    internal class GameManager:MonoBehaviour
    {
        public static GameManager instance { get; private set; }
        [SerializeField] UIManager uiManager;
        public UIManager UIManager { get => uiManager; private set => uiManager = value; }

        private void Start()
        {
            if(instance == null) { instance = this; }
            else { Destroy(gameObject); }

            /*
            // Get uiman and be angry if it doesnt exist.
            UIManager = GetComponentInChildren<UIManager>();
            */
            if (UIManager == null) { Debug.LogError("LF UIMan! Help!"); }
            else { UIManager.Ready(); }

            Debug.Log("GM Initialized!");

            // Once we're done initialization, tell the UIMan to load the game.
            UIManager.State=UIState.menu;
        }

        // No idea what to do here, idk if we need anything here yet.
    }
}
