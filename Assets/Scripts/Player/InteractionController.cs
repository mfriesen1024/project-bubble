using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class InteractionController : MonoBehaviour
    {
        public Action OnInteract;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if(TryGetComponent(out Ingredient ingredient))
            {

            }
            if(TryGetComponent(out Beaker beaker))
            {

            }
            if(TryGetComponent(out Door door))
            {

            }
        }
    }
}