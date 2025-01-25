using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers.Helpers
{
    internal class UIHelper
    {
        // Store refs here.
        UIManager uiman;
        Canvas mainMenu, instructions, hud, pause;

        public UIHelper(UIManager uIManager)
        {
            // Set refs.
            uiman = uIManager;
            InitUIElements();
            InitMainMenu();
            InitInstructions();
            InitHud();
            InitPause();
        }

        private void InitUIElements()
        {
            Canvas[] elements = uiman.GetComponentsInChildren<Canvas>();
            mainMenu = elements[0];
            instructions = elements[1];
            hud = elements[2];
            pause = elements[3];
        }

        void InitMainMenu()
        {
            Button[] buttons = mainMenu.GetComponentsInChildren<Button>();
            buttons[0].onClick.AddListener(OnPlayClicked);
            buttons[1].onClick.AddListener(OnCreditsClicked);
            buttons[2].onClick.AddListener(OnQuitClicked);

            void OnPlayClicked()
            {
                uiman.State = UIState.instructions;
            }

            void OnCreditsClicked()
            {
                throw new NotImplementedException("We dont have credits yet, so shut it.");
            }

            void OnQuitClicked()
            {
                Application.Quit();
            }
        }

        private void InitInstructions()
        {
            Button[] buttons = instructions.GetComponentsInChildren<Button>();
            buttons[0].onClick.AddListener(OnAccept);

            void OnAccept() { uiman.State = UIState.hud; }
        }

        private void InitHud()
        {
            throw new NotImplementedException();
        }

        private void InitPause()
        {
            throw new NotImplementedException();
        }
    }
}
