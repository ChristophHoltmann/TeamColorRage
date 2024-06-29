using UnityEngine;

namespace Toph.Utilities
{
    /// <summary>
    /// Provides "look at camera" functionality for the gameobject where the component is added to.
    /// The gameobject will look at the chosen camera when the component is enabled.
    /// </summary>
    public class LookAtCamera : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        [Tooltip("Ignore a specific axis when rotating towards the camera.")]
        private bool rotateAroundXAxis, rotateAroundYAxis, rotateAroundZAxis = false;

        [SerializeField]
        [Tooltip("Keep local rotation values when rotating around specific axis.")]
        private bool keepLocals = true;

        [SerializeField]
        [Tooltip("The speed of the rotation.")]
        private float rotationSpeed = 10f;

#pragma warning disable CS0109
        [SerializeField]
        [Tooltip("The camera to look at. Default is Camera.main.")]
        private new Camera camera;
#pragma warning restore

        public bool RotateAroundXAxis { get => rotateAroundXAxis; set => rotateAroundXAxis = value; }
        public bool RotateAroundYAxis { get => rotateAroundYAxis; set => rotateAroundYAxis = value; }
        public bool RotateAroundZAxis { get => rotateAroundZAxis; set => rotateAroundZAxis = value; }

        public bool KeepLocals { get => keepLocals; set => keepLocals = value; }

        public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }

        public Camera Camera
        {
            get
            {
                if (camera == null) camera = Camera.main;
                return camera;
            }
            set => camera = value;
        }

        private void Awake()
        {
            if (!Camera)
            {
                Debug.LogWarning($"No camera is assigned to the {nameof(LookAtCamera)} script " +
                    $"of the object: {gameObject.name}!");
                return;
            }
        }

        private void Update()
        {
            Vector3 lookAtPosition = Camera.transform.position;

            if (!KeepLocals)
            {
                if (RotateAroundXAxis) lookAtPosition.x = transform.position.x;
                if (RotateAroundYAxis) lookAtPosition.y = transform.position.y;
                if (RotateAroundZAxis) lookAtPosition.z = transform.position.z;
            }

            Vector3 lookPosition = transform.position - lookAtPosition;

            if (lookPosition != Vector3.zero)
            {
                var rotation = Quaternion.LookRotation(lookPosition);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);

                if (KeepLocals && (rotateAroundXAxis || RotateAroundYAxis || RotateAroundZAxis))
                {
                    var localRotation = new Quaternion(
                        transform.localRotation.x, transform.localRotation.y,
                        transform.localRotation.z, transform.localRotation.w);

                    transform.localRotation = Quaternion.Euler(
                        !RotateAroundXAxis ? localRotation.x : transform.localRotation.eulerAngles.x,
                        !RotateAroundYAxis ? localRotation.y : transform.localRotation.eulerAngles.y,
                        !RotateAroundZAxis ? localRotation.z : transform.localRotation.eulerAngles.z);
                }
            }
        }
    }
}