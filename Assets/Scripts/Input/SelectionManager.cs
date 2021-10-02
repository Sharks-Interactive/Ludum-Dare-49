using UnityEngine.UI;
using UnityEngine;
using Chrio.Entities;
using static SharkUtils.ExtraFunctions;

namespace Chrio.Controls
{
    public class SelectionManager : SharksBehaviour
    {
        public float Delay;
        public Canvas Canvas;

        private Vector3 _startPos;
        private Vector3 _endPos;
        private float _waitTime;
        private Rect _selectionRect;

        private RectTransform _selectionBox;
        private Image _img;

        void Start() { _selectionBox = GetComponent<RectTransform>(); _img = GetComponent<Image>(); }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _waitTime = Time.time + Delay;
                _startPos = GlobalState.Game.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
                if (Time.time > _waitTime)
                {
                    _img.enabled = true;
                    _endPos = GlobalState.Game.MainCamera.ScreenToWorldPoint(Input.mousePosition);
                }

            if (Input.GetMouseButtonUp(0))
                if (Time.time > _waitTime)
                {
                    _img.enabled = false;
                    SelectUnits();
                }

            DrawSelectionBox();
        }

        public void DrawSelectionBox()
        {
            _selectionBox.position = new Vector3(_startPos.x, _startPos.y, 0);
            _selectionBox.sizeDelta = new Vector2(_endPos.x - _startPos.x, _startPos.y - _endPos.y);
        }

        public void SelectUnits()
        {
            // Deselect All
            foreach (IBaseEntity t_ent in GlobalState.Game.Entities.Selected)
                t_ent.OnDeselected();

            GlobalState.Game.Entities.Selected.Clear();

            Vector2 _startPosVec2 = new Vector2(_startPos.x, _startPos.y);
            Bounds _selectionBounds = new Bounds(_startPosVec2.MidPoint(_endPos), _selectionBox.sizeDelta);

            foreach (GameObject Unit in GlobalState.Game.Entities.WorldEntities.Keys)
                if (_selectionBounds.Contains(Unit.transform.position))
                {
                    GlobalState.Game.Entities.Selected.Add(GlobalState.Game.Entities.WorldEntities[Unit]);
                    GlobalState.Game.Entities.WorldEntities[Unit].OnSelected();
                }
        }
    }
}
