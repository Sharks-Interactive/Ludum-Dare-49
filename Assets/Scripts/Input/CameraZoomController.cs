using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chrio.Controls
{
    public class CameraZoomController : SharksBehaviour
    {
        private Camera _camera;
        public float Sensitivity;
        public float DirSensitivity;

        [Tooltip("MinSize, MaxSize")]
        public Vector2 SizeConstraints = new Vector2(5, 70);

        private enum Dir
        {
            Up = -1,
            Down = 1,
        }    

        void Start() { _camera = GetComponent<Camera>(); }

        void Update()
        {
            if (Input.mouseScrollDelta.y == 1 && _camera.orthographicSize < SizeConstraints.y)
            {
                MoveCamera(Dir.Up);
                _camera.orthographicSize += Sensitivity;
            }
            else if (Input.mouseScrollDelta.y == -1 && _camera.orthographicSize > SizeConstraints.x)
            {
                MoveCamera(Dir.Down);
                _camera.orthographicSize -= Sensitivity;
            }
        }

        private void MoveCamera(Dir _direcction)
        {
            float _normalizedMousePos = Input.mousePosition.y / Screen.height;
            if (_normalizedMousePos < 0.7 && _normalizedMousePos > 0.3) return; // Middle of screen

            float _mousePos = (_normalizedMousePos < 0.5 ? (float)Dir.Up : (float)Dir.Down);
            _camera.transform.position += new Vector3(0, _mousePos * DirSensitivity * (float)_direcction, 0);
        }
    }
}
