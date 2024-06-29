using System;
using System.Collections;
using UnityEngine;

namespace Toph.Utilities
{
    /// <summary>
    /// Provides public interpolation functionality that can be used to interpolate values.
    /// The "Lerp" function calls an "OnFinish" action when the interpolation is done.
    /// </summary>
    public abstract class Lerper : MonoBehaviour
    {
        protected float lerpTime { get; private set; }

        protected Coroutine lerpCoroutine { get; private set; }

        public void Lerp(bool inOut, float duration, Action<float> actionOnFinish = null)
        {
            if (lerpCoroutine != null)
            {
                StopCoroutine(lerpCoroutine);
            }
            if (isActiveAndEnabled)
            {
                lerpCoroutine = StartCoroutine(LerpCoroutine(inOut, duration, actionOnFinish));
            }
        }

        protected IEnumerator LerpCoroutine(bool inOut, float duration, Action<float> actionOnFinish = null)
        {
            lerpTime = 0f;
            while (lerpTime < duration)
            {
                lerpTime += Time.deltaTime;
                float t = lerpTime / duration;
                OnLerp(inOut ? 1f - t : t);
                yield return null;
            }

            lerpTime = duration;

            OnLerpFinished(inOut ? 0f : 1f);
            actionOnFinish?.Invoke(inOut ? 0f : 1f);

            lerpCoroutine = null;
        }

        protected abstract void OnLerp(float t);

        protected virtual void OnLerpFinished(float t)
        {

        }
    }
}