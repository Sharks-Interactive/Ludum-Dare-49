using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SharkUtils
{
    public static class ExtraFunctions
    {
        [Serializable]
        public static class StringKeyedGameObject
        {
            public static string Key;
            public static GameObject Value;
        }


        /// <summary>
        /// Checks wether two Quaternions (A and B) are within the acceptable range, used for comparison.
        /// </summary>
        /// <param name="quatA">Quaternion A</param>
        /// <param name="value">Quaternion B</param>
        /// <param name="acceptableRange">The max acceptable difference between Quaternion A and B</param>
        /// <returns></returns>
        public static bool Approximately(this Quaternion quatA, Quaternion value, float acceptableRange)
        {
            return 1 - Mathf.Abs(Quaternion.Dot(quatA, value)) < acceptableRange;
        }

        /// <summary>
        /// Returns a random vector3 with the x, y, and z all random between the input range.
        /// </summary>
        /// <param name="Range"> The input range. </param>
        /// <returns></returns>
        public static Vector3 RandomRangeVec3(Vector2 Range)
        {
            return new Vector3(
                UnityEngine.Random.Range(Range.x, Range.y),
                UnityEngine.Random.Range(Range.x, Range.y),
                UnityEngine.Random.Range(Range.x, Range.y)
            );
        }

        /// <summary>
        /// Get's the midpoint between two vector2's
        /// </summary>
        /// <param name="vector"> The starting vector2 (extension method) </param>
        /// <param name="secondVector"> The second vector2 </param>
        /// <returns> Midpoint of the two vectors </returns>
        public static Vector2 MidPoint(this Vector2 vector, Vector2 secondVector) => new Vector2(((secondVector.x + vector.x) / 2.0f), ((secondVector.y + vector.y) / 2.0f));

        public static Vector3 Parse(this Vector3 vector, string sVector)
        {
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
                sVector = sVector.Substring(1, sVector.Length - 2);

            string[] sArray = sVector.Split(',');

            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            return result;
        }

        public static Vector3 ScreenToCanvasPosition(this Canvas canvas, Vector3 screenPosition)
        {
            var viewportPosition = new Vector3(screenPosition.x / Screen.width,
                                               screenPosition.y / Screen.height,
                                               0);
            return canvas.ViewportToCanvasPosition(viewportPosition);
        }

        public static Vector3 ViewportToCanvasPosition(this Canvas canvas, Vector3 viewportPosition)
        {
            var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
            var canvasRect = canvas.GetComponent<RectTransform>();
            var scale = canvasRect.sizeDelta;
            return Vector3.Scale(centerBasedViewPortPosition, scale);
        }

        public static Vector2 Parse(this Vector2 vector, string sVector)
        {
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
                sVector = sVector.Substring(1, sVector.Length - 2);

            string[] sArray = sVector.Split(',');

            Vector2 result = new Vector2(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]));

            return result;
        }

        /// <summary>
        /// Returns a vector3 along a circle given the center, diamter and angle.
        /// </summary>
        /// <param name="center"> A Vector3 which defines where the center of the circle is. </param>
        /// <param name="d"> The diameter of the circle. </param>
        /// <param name="angle"> The angle from the center to the point. </param>
        /// <returns></returns>
        public static Vector3 FindPosAlongCircleWithAngle(Vector3 center, float d, float angle)
        {
            Vector3 EndPos = new Vector3();

            EndPos.x = d * Mathf.Cos(angle) + center.x;
            EndPos.y = d * Mathf.Sin(angle) + center.y;
            EndPos.z = center.z;

            return EndPos;
        }

        /// <summary>
        /// Returns a random Vector3 worldspace position within a circle whose center is center and radius is r
        /// </summary>
        /// <param name="center"> The world space center of the circle. </param>
        /// <param name="r"> The radius of the circle. </param>
        /// <returns></returns>
        public static Vector3 RandomPointInsideCircle(Vector3 center, float r)
        {
            return UnityEngine.Random.insideUnitSphere * r + center;
        }

        /// <summary>
        /// Exactly like Random.Range except it takes in a single Vector2 for the range.
        /// </summary>
        /// <param name="Range"> A vector 2 representing the range. </param>
        /// <returns></returns>
        public static float RandomFromRange(Vector2 Range)
        {
            return UnityEngine.Random.Range(Range.x, Range.y);
        }

        /// <summary>
        /// THIS IS BAD DONT USE THIS PLEASE! But it uh... Get's interfaces
        /// </summary>
        /// <typeparam name="T"> Interface type </typeparam>
        /// <param name="resultList"> List of interfaces </param>
        /// <param name="objectToSearch"> The object to check if it has an interface </param>
        public static void GetInterfaces<T>(out List<T> resultList, GameObject objectToSearch) where T : class
        {
            MonoBehaviour[] list = objectToSearch.GetComponents<MonoBehaviour>();
            resultList = new List<T>();
            foreach (MonoBehaviour mb in list)
            {
                if (mb is T)
                {
                    //found one
                    resultList.Add((T)((System.Object)mb));
                }
            }
        }

        /// <summary>
        /// THIS IS HORRENDOUS DONT USE THIS PLEASE! But it uh... Get's interfaces
        /// </summary>
        /// <typeparam name="T"> Interface type </typeparam>
        /// <param name="resultList"> List of interfaces </param>
        /// <param name="objectToSearch"> The object to check if it has an interface </param>
        public static void GetAllInterfaces<T>(out List<T> resultList) where T : class
        {
            MonoBehaviour[] list = GameObject.FindObjectsOfType<MonoBehaviour>(true);
            resultList = new List<T>();
            foreach (MonoBehaviour mb in list)
            {
                if (mb is T)
                {
                    //found one
                    resultList.Add((T)((System.Object)mb));
                }
            }
        }

        /// <summary>
        /// Randomly rotates a transform with the following aixs.
        /// </summary>
        /// <param name="t"> Is an extension method so this should not matter, but the current rotation. </param>
        /// <param name="x"> Should we randomize x rotation. </param>
        /// <param name="y"> Should we randomize y rotation. </param>
        /// <param name="z"> Should we randomize z rotation. </param>
        /// <returns></returns>
        public static Quaternion RandomRotationOnAxis(this Transform t, bool x, bool y, bool z)
        {
            Quaternion Rot = UnityEngine.Random.rotation;
            if (!x) { Rot.x = t.rotation.x; }
            if (!y) { Rot.y = t.rotation.y; }
            if (!z) { Rot.z = t.rotation.z; }

            return Rot;
        }

        /// <summary>
        /// Picks a random item from a list.
        /// </summary>
        /// <typeparam name="T"> The object that will be returned. </typeparam>
        /// <param name="L"> The list to pick from. </param>
        /// <returns></returns>
        public static T RandomFromList<T>(List<T> L)
        {
            var value = L[UnityEngine.Random.Range(0, L.Count)];
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Picks a random item from an array.
        /// </summary>
        /// <typeparam name="T"> The object that will be returned. </typeparam>
        /// <param name="L"> The list to pick from. </param>
        /// <returns></returns>
        public static T Random<T>(this T[] _arr)
        {
            var value = _arr[UnityEngine.Random.Range(0, _arr.Length)];
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Picks a random number in the range excluding the set numbers.
        /// </summary>
        /// <param name="min"> The lower number of the range. </param>
        /// <param name="max"> The higher number in the range. </param>
        /// <param name="exclude"> The list of numbers to exclude. </param>
        /// <returns></returns>
        public static int RandomNumberExcluding(int min, int max, HashSet<int> exclude)
        {
            var range = Enumerable.Range(min, max).Where(i => !exclude.Contains(i));

            var rand = new System.Random();
            int index = rand.Next(min, max - exclude.Count);
            try
            {
                return range.ElementAt(index);
            }
            catch
            {
                return 9001;
            }
        }

        /// <summary>
        /// Moves the specified item to the front of the list.
        /// </summary>
        /// <typeparam name="T"> (Extension Method) </typeparam>
        /// <param name="list"> The list to modify. </param>
        /// <param name="index"> Index of the item to move. </param>
        public static void MoveItemAtIndexToFront<T>(this List<T> list, int index)
        {
            T item = list[index];
            list.RemoveAt(index);
            list.Insert(0, item);
        }

        // <summary>
        // Sets the specified effect to either enabled or disabled on a postprocessvolume
        // </summary>
        // <param name="PManager"> (Extension Method) </param>
        // <param name="Effect"> The effect to modify. </param>
        // <param name="Enabled"> Whether or not said effect should be enabled. </param>
        //public static void SetEffect (this PostProcessVolume PManager, Postpro Effect, bool Enabled)
        //{
        //    PManager.profile.TryGetSettings(out Effect);
        //    Effect.enabled.value = Enabled;
        //}

        /// <summary>
        /// Tries to remove an item from the list at index
        /// </summary>
        /// <typeparam name="T"> (Extension Method) </typeparam>
        /// <param name="l"> (Extension Method) </param>
        /// <param name="index"> The index of the item to remove. </param>
        public static void TryRemoveAt<T>(this List<T> l, int index)
        {
            try
            {
                l.RemoveAt(index);
            }
            catch
            {
                Debug.LogWarning("TryRemove at: " + index + ". On List " + l.ToString() + ". Failed.");
            }
        }
        /// <summary>
        /// Remove element from array by shifting all elements down and resizing
        /// </summary>
        /// <typeparam name="T"> Array type </typeparam>
        /// <param name="arr"> The array to resize </param>
        /// <param name="index"> The index of the item to remove </param>
        public static void RemoveAt<T>(ref T[] arr, int index)
        {
            for (int a = index; a < arr.Length - 1; a++)
            {
                // moving elements downwards, to fill the gap at [index]
                arr[a] = arr[a + 1];
            }
            // finally, let's decrement Array's size by one
            Array.Resize(ref arr, arr.Length - 1);
        }

        /// <summary>
        /// Returns a random position along the edge of a circle.
        /// </summary>
        /// <param name="radius"> The radius of the circle. </param>
        /// <param name="center"> The center of the circle. </param>
        /// <returns></returns>
        public static Vector3 RandomPointOnCircleEdge(float radius, Vector3 center)
        {
            var vector2 = UnityEngine.Random.insideUnitCircle.normalized * radius;
            return new Vector3(vector2.x + center.x, vector2.y + center.y, vector2.y + center.z);
        }

        public static bool IsInsideCircle(Vector2 Point, Vector2 Center, float Radius) => Vector2.Distance(Center, Point) < Radius;

        /// <summary>
        /// Returns a point along a line defined by the vectors A and B where Percentage determines how far from A it is on a scale of 0 - 1
        /// </summary>
        /// <param name="A"> The first Vector of the line. </param>
        /// <param name="B"> The second Vector of the line. </param>
        /// <param name="Percentage"> Where along the line should we return between 0 and 1. </param>
        /// <returns></returns>
        public static Vector3 PointAlongLine(Vector3 A, Vector3 B, float Percentage)
        {
            return (A + Percentage * (B - A));
        }

        /// <summary>
        /// Used for making random chance applications. Given a percentage will return true or false randomly within the percent specified.
        /// </summary>
        /// <param name="Percentage"> The percentage of time the return value will be true. </param>
        /// <returns></returns>
        public static bool Chance(float Percentage)
        {
            return UnityEngine.Random.Range(0, 100) <= Percentage;
        }

        //Defining a public random for the whole library to use
        private static System.Random rng = new System.Random();

        public static float SumOfVector2 (this Vector2 _vector) => _vector.x + _vector.y;
        public static float SumOfVector2(this Vector3 _vector) => _vector.x + _vector.y;

        public static float SumOfVector3(this Vector3 _vector) => _vector.x + _vector.y + _vector.z;

        public static float AbsSumOfVector2(this Vector2 _vector) => Mathf.Abs(_vector.x) + Mathf.Abs(_vector.y);
        public static float AbsSumOfVector2(this Vector3 _vector) => Mathf.Abs(_vector.x) + Mathf.Abs(_vector.y);

        public static float AbsSumOfVector3(this Vector3 _vector) => Mathf.Abs(_vector.x) + Mathf.Abs(_vector.y) + Mathf.Abs(_vector.z);

        public static Vector3 ToVector3 (this Vector2 _vector) => new Vector3(_vector.x, _vector.y, 0);

        public static Vector3 ToVector3(this Vector2 _vector, float zValue) => new Vector3(_vector.x, _vector.y, zValue);

        public static Vector2 ToVector2(this Vector3 _vector) => new Vector2(_vector.x, _vector.y);

        public static void SetArrayActive(this GameObject[] _arr, bool Active)
        {
            for (int i = 0; i < _arr.Length; i++)
                _arr[i].SetActive(Active);
        }

        public static Vector3 NormalizedMousePosition()
        {
            float mouseRatioX = Input.mousePosition.x / Screen.width;
            float mouseRatioY = Input.mousePosition.y / Screen.height;

            return new Vector3(mouseRatioX - 0.5f, mouseRatioY - 0.5f, 0f);
        }

        public enum Axis
        {
            X,
            Y,
            Z,
            W
        }
        /// <summary>
        /// Updates a single axis of a Transform
        /// </summary>
        /// <param name="_vector"> (Extension Method) </param>
        /// <param name="Value"> The value to set the axis to. </param>
        /// <param name="Constraint"> The axis to set. </param>
        /// <returns> The update vector3. </returns>
        public static Vector3 UpdateAxis(this Transform _vector, float Value, Axis Constraint)
        {
            switch (Constraint)
            {
                case Axis.X:
                    _vector.position.Set(Value, _vector.position.y, _vector.position.z);
                    break;

                case Axis.Y:
                    _vector.position.Set(_vector.position.x, Value, _vector.position.z);
                    break;

                case Axis.Z:
                    _vector.position.Set(_vector.position.x, _vector.position.y, Value);
                    break;
            }
            return _vector.position;
        }

        /// <summary>
        /// Updates a single axis of a Quaternion
        /// </summary>
        /// <param name="_vector"> (Extension Method) </param>
        /// <param name="Value"> The value to set the axis to. </param>
        /// <param name="Constraint"> The axis to set. </param>
        /// <returns> The update vector3. </returns>
        public static Quaternion UpdateAxis(this Quaternion _quaternion, float Value, Axis Constraint)
        {
            switch (Constraint)
            {
                case Axis.X:
                    _quaternion.Set(Value, _quaternion.y, _quaternion.z, _quaternion.w);
                    break;

                case Axis.Y:
                    _quaternion.Set(_quaternion.x, Value, _quaternion.z, _quaternion.w);
                    break;

                case Axis.Z:
                    _quaternion.Set(_quaternion.x, _quaternion.y, Value, _quaternion.w);
                    break;

                case Axis.W:
                    _quaternion.Set(_quaternion.x, _quaternion.y, _quaternion.z, Value);
                    break;
            }
            return _quaternion;
        }

        //// <summary>
        /// Updates a single axis of a Vector3
        /// </summary>
        /// <param name="_vector"> (Extension Method) </param>
        /// <param name="Value"> The value to set the axis to. </param>
        /// <param name="Constraint"> The axis to set. </param>
        /// <returns> The update vector3. </returns>
        public static Vector3 UpdateAxisEuler(this Vector3 _vector, float Value, Axis Constraint)
        {
            switch (Constraint)
            {
                case Axis.X:
                    _vector.Set(Value, _vector.y, _vector.z);
                    break;

                case Axis.Y:
                    _vector.Set(_vector.x, Value, _vector.z);
                    break;

                case Axis.Z:
                    _vector.Set(_vector.x, _vector.y, Value);
                    break;
            }
            return _vector;
        }


        public static Bounds OrthographicBounds(this Camera camera)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = camera.orthographicSize * 2;
            Bounds bounds = new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }

        public static bool RemoveIfContains<T>(this List<T> _list, T element)
        {
            if (!_list.Contains(element)) return false;
                
            _list.Remove(element);
            return true;
        }

        public static bool AddIfNotContains<T>(this List<T> _list, T element)
        {
            if (!_list.Contains(element)) return false;

            _list.Add(element);
            return true;
        }

        /// <summary>
        /// Counts the bounding box corners of the given RectTransform that are visible from the given Camera in screen space.
        /// </summary>
        /// <returns>The amount of bounding box corners that are visible from the Camera.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        private static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
            Vector3[] objectCorners = new Vector3[4];
            rectTransform.GetWorldCorners(objectCorners);

            int visibleCorners = 0;
            Vector3 tempScreenSpaceCorner; // Cached
            for (var i = 0; i < objectCorners.Length; i++) // For each corner in rectTransform
            {
                tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]); // Transform world space position of corner to screen space
                if (screenBounds.Contains(tempScreenSpaceCorner)) // If the corner is inside the screen
                {
                    visibleCorners++;
                }
            }
            return visibleCorners;
        }

        /// <summary>
        /// Determines if this RectTransform is fully visible from the specified camera.
        /// Works by checking if each bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
        /// </summary>
        /// <returns><c>true</c> if is fully visible from the specified camera; otherwise, <c>false</c>.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            return CountCornersVisibleFrom(rectTransform, camera) == 4; // True if all 4 corners are visible
        }

        /// <summary>
        /// Determines if this RectTransform is at least partially visible from the specified camera.
        /// Works by checking if any bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
        /// </summary>
        /// <returns><c>true</c> if is at least partially visible from the specified camera; otherwise, <c>false</c>.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera.</param>
        public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            return CountCornersVisibleFrom(rectTransform, camera) > 0; // True if any corners are visible
        }

        //// <summary>
        /// Updates a single axis of a Col9or
        /// </summary>
        /// <param name="_vector"> (Extension Method) </param>
        /// <param name="Value"> The value to set the axis to. </param>
        /// <param name="Constraint"> The axis to set. </param>
        /// <returns> The update vector3. </returns>
        public static Color UpdateColor(this Color _color, float Value, Axis Constraint)
        {
            switch (Constraint)
            {
                case Axis.X:
                    _color = new Color(Value, _color.g, _color.b, _color.a);
                    break;

                case Axis.Y:
                    _color = new Color(_color.r, Value, _color.b, _color.a);
                    break;

                case Axis.Z:
                    _color = new Color(_color.r, _color.g, Value, _color.a);
                    break;

                case Axis.W:
                    _color = new Color(_color.r, _color.g, _color.b, Value);
                    break;
            }
            return _color;
        }


        public static Color2 GetColor2(this Color[] colors) => new Color2(colors[0], colors[1]);

        public static float Greatest(this float[] _arr)
        {
            float greatest = 0;
            for (int i = 0; i < _arr.Length; i++)
                if (_arr[i] > greatest) greatest = _arr[i];

            return greatest;
        }

        public static int IndexOfGreatest(this float[] _arr)
        {
            float greatest = 0;
            int index = 0;
            for (int i = 0; i < _arr.Length; i++)
                if (_arr[i] > greatest)
                {
                    greatest = _arr[i];
                    index = i;
                }

            return index;
        }

        public static float Least(this float[] _arr)
        {
            float least = 9999999;
            for (int i = 0; i < _arr.Length; i++)
                if (_arr[i] > least) least = _arr[i];

            return least;
        }

        public static int IndexOfLeast(this float[] _arr)
        {
            float least = 9999999;
            int index = 0;
            for (int i = 0; i < _arr.Length; i++)
                if (_arr[i] < least)
                {
                    least = _arr[i];
                    index = i;
                }

            return index;
        }

        /// <summary>
        /// Shuffles all of the elements in a list.
        /// </summary>
        /// <typeparam name="T"> The List type (Is an extension method) </typeparam>
        /// <param name="list"> The list to shuffle (Is and extension method)</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Remaps a given value from a given range into a new range.
        /// </summary>
        /// <param name="Value"> The value to remap. </param>
        /// <param name="From1"> The input values lowest possible amount. </param>
        /// <param name="To1"> The input values highest possible amount. </param>
        /// <param name="From2"> The remap range low. </param>
        /// <param name="To2"> The remap range high. </param>
        /// <returns></returns>
        public static float Remap(this float Value, float From1, float To1, float From2, float To2)
        {
            return (Value - From1) / (To1 - From1) * (To2 - From2) + From2;
        }
    }

    [Serializable]
    public class WorldPoint
    {
        public Vector3 Pos;
        public Quaternion Rot;
    }

    [Serializable]
    public class VectorWorldPoint
    {
        public Vector3 Pos;
        public Vector3 Rot;
    }

    [Serializable]
    public class SquaredVector3
    {
        public Vector3 Value = new Vector3();

        public SquaredVector3(float V)
        {
            Value = new Vector3(V, V, V);
        }

        // Vector3 {return Value; }
        //Vector3 { Value = value; }
        public Vector3 m_SquaredVector3
        {
            get { return Value; }
            set { Value = value; }
        }

        public static implicit operator Vector3(SquaredVector3 v)
        {
            return v.Value;
        }
    }

    [Serializable]
    public class SquaredVector2
    {
        public Vector2 Value = new Vector2();

        public SquaredVector2(float V)
        {
            Value = new Vector2(V, V);
        }

        // Vector3 {return Value; }
        //Vector3 { Value = value; }
        public Vector2 m_SquaredVector2
        {
            get { return Value; }
            set { Value = value; }
        }

        public static implicit operator Vector2(SquaredVector2 v)
        {
            return v.Value;
        }
    }
}