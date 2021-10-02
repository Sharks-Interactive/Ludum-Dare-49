using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Chrio.UI
{
    public class BGColorChanger : MonoBehaviour
    {
        public float Time;
        private SpriteRenderer _image;

        [Header("Colors")]
        public Color[] Colors = new Color[2];
        private int _iteration = 0;

        void Start() { _image = GetComponent<SpriteRenderer>(); StartCoroutine(ChangeColor()); }

        private IEnumerator ChangeColor()
        {
            yield return new WaitForSeconds(Time);

            _iteration = (_iteration >= Colors.Length - 1 ? 0 : _iteration + 1);
            _image.DOColor(Colors[_iteration], Time);

            StartCoroutine(ChangeColor());
        }
    }
}
