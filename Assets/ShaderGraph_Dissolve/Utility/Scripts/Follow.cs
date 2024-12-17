using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DissolveExample
{
    /// <summary>
    /// Moves an object up and down in a smooth, repeating motion using the PingPong function.
    /// The speed and height of the movement are configurable.
    /// </summary>
    public class Follow : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("Speed of the vertical movement (range: 0 to 5).")]
        [Range(0, 5f)]
        public float speed;

        [Tooltip("Maximum height offset for the movement.")]
        public float height;

        private Vector3 startPosition; // Initial position of the object

        /// <summary>
        /// Initializes the starting position of the object.
        /// </summary>
        void Start()
        {
            startPosition = transform.position;
        }

        /// <summary>
        /// Updates the object's position each frame, creating an up-and-down motion.
        /// </summary>
        void Update()
        {
            // Calculate the vertical offset using Mathf.PingPong
            float verticalOffset = Mathf.PingPong(Time.time * speed, height);

            // Apply the offset to the object's position
            transform.position = startPosition + verticalOffset * Vector3.up;
        }
    }
}