using DG.Tweening;
using UnityEngine;

namespace Exploration
{
    public static class SequenceLibrary
    {
        public static Sequence GetScaleSequence(Transform t, Vector3 toScale = default, float duration = 1f,
            bool loop = true, Ease ease = Ease.Linear)
        {
            return DOTween.Sequence().Append(t.DOScale(toScale, duration).SetLoops(loop ? -1 : 1).SetEase(ease));
        }
    }
}