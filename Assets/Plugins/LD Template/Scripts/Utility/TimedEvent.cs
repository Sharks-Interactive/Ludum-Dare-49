using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Chrio.Utilities
{
    public class TimedEvent : MonoBehaviour
    {
        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }

        [FormerlySerializedAs("onTime")]
        [SerializeField]
        public ButtonClickedEvent m_OnTime = new ButtonClickedEvent();

        [Tooltip("Time before the event is run")]
        public float Time;

        [Tooltip("The max number of times that the event can run")]
        public int MaxRuns;
        private int _repetitions = 0;

        // Update is called once per frame
        void OnAwake()
        {
            if (_repetitions < MaxRuns) StartCoroutine("Wait");
            _repetitions++;
        }
        //Wait time before disabling the text
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(Time);
            m_OnTime.Invoke();
        }
    }
}
