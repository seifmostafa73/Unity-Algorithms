using UnityEngine;

namespace Chess_Classes.Interfaces {
    public class InstantMotionTweener : MonoBehaviour, IMotionTweener {

        public void MoveTo(Transform transform, Vector3 targetPosition) {
            transform.position = targetPosition;
        }
    }
}