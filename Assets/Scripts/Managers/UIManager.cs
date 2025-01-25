using System;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public UIState State { get => state; set { ExecuteStateChange(value); state = value; } }


        UIState state;
        public Action<UIState> StateChanged = delegate { };
        private Canvas[] UIElements;


        /// <summary>
        /// Called on startup, godot naming.
        /// </summary>
        internal void Ready()
        {

        }

        // An internal updater.
        private void ExecuteStateChange(UIState value)
        {
            if (state != value)
            {
                // hide all ui elements, then activate by number.
                Hideall();
                GameObject uiElement = UIElements[(int)value].gameObject;
            }
        }

        private void Hideall()
        {
            foreach (var element in UIElements) { element.gameObject.SetActive(false); }
        }
    }

    public enum UIState
    {
        menu,
        instructions,
        hud,
        pause
    }
}