using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Chrio.Entities;
using static Chrio.World.Game_State;
using Chrio.World.Loading;
using Chrio.Controls;
using static SharkUtils.ExtraFunctions;

namespace Chrio.Interaction
{
    public class MouseInteraction2D : SharksBehaviour
    {
        private EventSystem _es;

        private IBaseEntity prevSelected;
        private IBaseEntity curSelected;

        private PointerEventData _pointerEventData;
        private GameObject _selected;

        public override void OnLoad(State _gameState, ILoadableObject.CallBack _callback)
        {
            _es = GetComponent<EventSystem>();

            Assert.AreNotEqual(null, _es);
            _pointerEventData = new PointerEventData(_es);
            base.OnLoad(_gameState, _callback);
            Assert.AreNotEqual(null, GlobalState);
        }

        void Update()
        {
            if (GlobalState.Game.Entities.Selected.Count > 0)
                foreach (IBaseEntity t_ent in GlobalState.Game.Entities.Selected)
                    t_ent.WhileSelected();

            if (!Input.GetMouseButtonDown(0)) return;

            _selected = GetObjectUnderCursor();
            if (_selected != null)
            {
                if (!GlobalState.Game.Entities.WorldEntities.TryGetValue(_selected, out curSelected)) return; // Selected is not an entity
                if (prevSelected == null) { OnDifferentSelected(); return; }

                if (prevSelected.Equals(curSelected)) OnSameSelected(); // If we are already in the list
                else OnDifferentSelected();
            }
            else OnNothingSelected();
        }

        private GameObject GetObjectUnderCursor()
        {
            List<RaycastResult> _raycastResults = new List<RaycastResult>();
            _pointerEventData.position = Input.mousePosition;
            _es.RaycastAll(_pointerEventData, _raycastResults);

            if (_raycastResults.Count > 0) return _raycastResults[0].gameObject;
            else return null;
        }

        private void OnNothingSelected()
        {
            // Check if we already have stuff selected
            if (GlobalState.Game.Entities.Selected.Count > 0)
                MoveOrderToMouse(true);
        }

        private void OnDifferentSelected()
        {
            // Check if the user is holding down shift
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                // If we are holding shift check if the entity is already selected
                if (GlobalState.Game.Entities.Selected.Contains(curSelected))
                {
                    // If we have already selected this entity deselect it but nothing else
                    GlobalState.Game.Entities.Selected.Remove(curSelected);
                    curSelected.OnDeselected();
                }
                else
                {
                    // If we are holding down shift and this is a new entity add it to the list of selected entities
                    GlobalState.Game.Entities.Selected.Add(curSelected);
                    curSelected.OnSelected();
                }
            }
            else
            {
                // If we are not holding down shift check if this entity is already selected
                if (GlobalState.Game.Entities.Selected.Contains(curSelected))
                    DeselectAll(); // If it is already selected then deselect everything
                else
                {
                    if (curSelected.GetOwnerID() != 0) // If the selected unit is an enemy we want our ships to attack it
                    {
                        MoveOrderToMouse(false);

                        GlobalState.Game.Entities.Selected.Add(curSelected);
                        curSelected.OnSelected(); // Select the enemy unit

                        return;
                    }

                    // If it is not already selected then let's select it and deselect everything else
                    // Deselect everything that exists so far
                    DeselectAll();

                    GlobalState.Game.Entities.Selected.Add(curSelected);
                    curSelected.OnSelected();
                }
            }

            // This is now the previously selected item
            prevSelected = curSelected;
        }

        private void OnSameSelected()
        {
            // Check if the user is holding down shift
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                // If we are holding shift check if the entity is already selected
                if (GlobalState.Game.Entities.Selected.Contains(curSelected))
                {
                    // If we have already selected this entity deselect it but nothing else
                    GlobalState.Game.Entities.Selected.Remove(curSelected);
                    curSelected.OnDeselected();
                }
            }
            else
            {
                // If we are not holding down shift check if this entity is already selected
                if (GlobalState.Game.Entities.Selected.Contains(curSelected))
                    DeselectAll(); // If it is already selected then deselect everything
                else
                {
                    // If it is not already selected then let's select it and deselect everything else
                    // Deselect everything that exists so far
                    DeselectAll();

                    GlobalState.Game.Entities.Selected.Add(curSelected);
                    curSelected.OnSelected();
                }
            }
        }

        private void DeselectAll()
        {
            foreach (IBaseEntity t_ent in GlobalState.Game.Entities.Selected)
                t_ent.OnDeselected();

            GlobalState.Game.Entities.Selected.Clear();
        }

        private void MoveOrderToMouse(bool ClearAfter)
        {
            foreach (IBaseEntity ent in GlobalState.Game.Entities.Selected) // If we do loop through each one deselect it and issue a move command
            {
                ent.OnDeselected();
                ent.OnCommand(new Command( // Issue move order
                CommandType.Move,
                RandomPointInsideCircle(GlobalState.Game.MainCamera.ScreenToWorldPoint(Input.mousePosition), 3).ToString("F6"), // Randomize for multi select
                0
                ));
            }

            GlobalState.Game.Entities.Selected.Clear();
            prevSelected = null;
            if (ClearAfter)
                curSelected = null;
        }
    }
}
