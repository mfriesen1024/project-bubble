using Assets.Scripts.Managers.Helpers;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public Action<UIState> StateChanged = delegate { };
        public UIState State { get => state; set { ExecuteStateChange(value); state = value; } }
        UIState state = (UIState)(-1);

        UIHelper uIHelper;

        [SerializeField] Canvas[] UIElements;


        /// <summary>
        /// Called on startup, godot naming.
        /// </summary>
        internal void Ready()
        {
            uIHelper = new UIHelper(this);

            Debug.Log("UIMan Initialized!");

            State = UIState.menu;
        }

        // An internal updater.
        private void ExecuteStateChange(UIState value)
        {
            if (state != value)
            {
                // hide all ui elements, then activate by number.
                Hideall();
                GameObject uiElement = UIElements[(int)value].gameObject;
                uiElement.SetActive(true);
            }
        }

        private void Hideall()
        {
            foreach (var element in UIElements)
            {
                element.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            // Is this the wrong spot for a pause input check? well too bad.
            if(State == UIState.hud && Input.GetKey(KeyCode.Escape)) { State = UIState.pause; }
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