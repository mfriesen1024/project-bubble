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
                throw new NotImplementedException();
            }

            void OnCreditsClicked()
            {
                throw new NotImplementedException();
            }

            void OnQuitClicked()
            {
                throw new NotImplementedException();
            }
        }



        private void InitInstructions()
        {
            throw new NotImplementedException();
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
