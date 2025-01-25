using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers.Helpers
{
    internal class UIHelper
    {
        // Store refs here.
        UIManager uiman;
        Canvas mainMenu,instructions,hud,pause;

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

        private void InitMainMenu()
        {
            
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
