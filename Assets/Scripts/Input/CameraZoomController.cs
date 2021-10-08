using static SharkUtils.ExtraFunctions;
using UnityEngine;

namespace Chrio.Controls
{
    public class CameraZoomController : SharksBehaviour
    {
        private Camera _camera;
        public float Sensitivity;
        public float DirSensitivity;
        private Vector3 _lastFrameMousePos = Vector3.zero;

        private Vector3 _mousePosDelta = Vector3.zero;

        [Tooltip("MinSize, MaxSize")]
        public Vector2 SizeConstraints = new Vector2(5, 70);

        private enum Dir
        {
            Up = -1,
            Down = 1,
        }    

        void Start() { _camera = GetComponent<Camera>(); _lastFrameMousePos = Input.mousePosition; }

        void Update()
        {
            _mousePosDelta = Input.mousePosition - _lastFrameMousePos;
            _lastFrameMousePos = Input.mousePosition;

            if (Input.GetMouseButton(2) || Input.GetMouseButton(1) || (Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))))
                _camera.transform.position += _mousePosDelta / (-62.5f + _camera.orthographicSize);

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
