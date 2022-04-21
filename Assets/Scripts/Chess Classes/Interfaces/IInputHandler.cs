using System;
using UnityEngine;
namespace Chess_Classes {
    public interface IInputHandler {
        void ProcessInput(Vector3 inputPosition, GameObject selectedObject, Action onClick);
    }
}