using System;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Door : MonoBehaviour
    {
        public PotionType RequiredPotion;
        public Action OnDoorMelt = delegate { };
        [SerializeField] private Collider wallCollider;

        [Header("Destruction Sound")]
        [SerializeField] private AudioClip destructionSound; // Single sound for destruction
        private AudioSource audioSource;

        private void Awake()
        {
            // Add or get an AudioSource component
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // Configure the AudioSource
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }

        public void DoThings()
        {
            OnDoorMelt();

            // Disable wall collider
            if (wallCollider != null)
            {
                wallCollider.enabled = false;
            }

            // Play the destruction sound using a temporary GameObject
            PlayDestructionSound();

            // Destroy the door object immediately
            Destroy(gameObject);
        }

        private void PlayDestructionSound()
        {
            if (destructionSound != null)
            {
                // Create a temporary GameObject to play the sound
                GameObject tempAudioObject = new GameObject("TempAudio");
                AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();

                // Configure the AudioSource settings
                tempAudioSource.clip = destructionSound;
                tempAudioSource.volume = audioSource.volume;
                tempAudioSource.pitch = audioSource.pitch;
                tempAudioSource.spatialBlend = audioSource.spatialBlend; // Adjust spatial sound if needed
                tempAudioSource.Play();

                // Destroy the temporary GameObject after the sound finishes playing
                Destroy(tempAudioObject, destructionSound.length);
            }
            else
            {
                Debug.LogWarning("No destruction sound assigned!");
            }
        }
    }
}
