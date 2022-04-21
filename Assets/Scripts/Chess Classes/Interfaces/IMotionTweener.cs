using UnityEngine;

namespace Chess_Classes {
    public interface IMotionTweener {
        void MoveTo(Transform transform,Vector3 targetPosition);
    }
}