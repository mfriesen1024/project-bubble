using Assets.Scripts.Player;
using System;
using UnityEngine;

namespace Assets.Scripts.Items
{
    internal class Ingredient : MonoBehaviour
    {
        public Action Pop = delegate { }; // Pop action
        [SerializeField] internal IngredientType type; // Type of the ingredient
        [SerializeField] GameObject bubble; // Reference to bubble GameObject
        [SerializeField] AudioClip[] popSounds; // Array of pop sounds
        private AudioSource audioSource; // Reference to AudioSource

        private void Awake()
        {
            // Add an AudioSource component if not already attached
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out InteractionController ignored))
            {
                Debug.Log($"Trying to pop bubble of ingredient {name}.");

                // Disable bubble
                bubble.SetActive(false);

                // Play random pop sound from the array
                if (popSounds != null && popSounds.Length > 0 && audioSource != null)
                {
                    var randomIndex = UnityEngine.Random.Range(0, popSounds.Length);
                    audioSource.PlayOneShot(popSounds[randomIndex]);
                }
            }
            else
            {
                Debug.Log($"Btw, no IC found.");
            }
        }
    }

    public enum IngredientType
    {
        oran,
        idk,
        bird
    }
}
