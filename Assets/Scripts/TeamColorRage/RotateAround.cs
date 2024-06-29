using UnityEngine;

namespace Toph.Utilities
{
    /// <summary>
    /// Provides "rotate around" functionality for the gameobject where the component is added to.
    /// </summary>
    public class RotateAround : MonoBehaviour
    {
        [SerializeField]
        private float anglePerSecond = 1f;

        [SerializeField]
        private bool rotate = true;

#pragma warning disable 0649
        [SerializeField]
        private bool xAxis, yAxis, zAxis = false;
#pragma warning restore

        public float AnglePerSecond { get { return anglePerSecond; } set { anglePerSecond = value; } }

        public bool Rotate { get { return rotate; } set { rotate = value; } }

        private Quaternion defaultRotation = Quaternion.identity;

        private void Update()
        {
            if (Rotate)
            {
                if (xAxis)
                    transform.RotateAroundAxis_X(AnglePerSecond, 1f);

                if (yAxis)
                    transform.RotateAroundAxis_Y(AnglePerSecond, 1f);

                if (zAxis)
                    transform.RotateAroundAxis_Z(AnglePerSecond, 1f);
            }
        }

        public void SetToDefault()
        {
            transform.localRotation = defaultRotation;
        }

        private void Awake()
        {
            defaultRotation = transform.localRotation;
        }
    }
}