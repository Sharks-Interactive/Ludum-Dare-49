using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Chrio.Entities;
using static Chrio.World.Game_State;
using Chrio.World.Loading;
using Chrio.Controls;

namespace Chrio.Interaction
{
    public class MouseInteraction2D : SharksBehaviour
    {
        private EventSystem _es;

        private IBaseEntity prevSelected;
        private IBaseEntity curSelected;

        public override void OnLoad(State _gameState, ILoadableObject.CallBack _callback)
        {
            _es = GetComponent<EventSystem>();

            Assert.AreNotEqual(null, _es);
            base.OnLoad(_gameState, _callback);
            Assert.AreNotEqual(null, GlobalState);
        }

        void Update()
        {
            if (_es.currentSelectedGameObject != null)
            {
                Assert.IsTrue(GlobalState.Game.Entities.WorldEntities.TryGetValue(_es.currentSelectedGameObject, out curSelected));

                if (prevSelected != null)
                {
                    if (prevSelected.Equals(curSelected)) curSelected.WhileSelected();
                    else
                    {
                        prevSelected.OnDeseleceted();
                        prevSelected = curSelected;
                    }
                }
                else
                {
                    curSelected.OnSelected();
                    prevSelected = curSelected;
                }
            }
            else if (curSelected != null && prevSelected != null)
            {
                curSelected = null;
                prevSelected = null;
            }


            if (Input.GetMouseButton(0) && prevSelected != null)
                prevSelected.OnCommand(new Command( // Issue move order
                    CommandType.Move,
                    GlobalState.Game.MainCamera.ScreenToWorldPoint(Input.mousePosition).ToString("F6")
                    ));
        }
    }
}
