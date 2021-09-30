using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace SharkUtils
{
    public static class ExtraFunctions
    {
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

        /// <summary>
        /// Returns a point along a line defined by the vectors A and B where Percentage determines how far from A it is on a scale of 0 - 1
        /// </summary>
        /// <param name="A"> The first Vector of the line. </param>
        /// <param name="B"> The second Vector of the line. </param>
        /// <param name="Percentage"> Where along the line should we return between 0 and 1. </param>
        /// <returns></returns>
        public static Vector3 PointAlongLine (Vector3 A, Vector3 B, float Percentage)
        {
            return (A + Percentage * (B - A));
        }

        /// <summary>
        /// Used for making random chance applications. Given a percentage will return true or false randomly within the percent specified.
        /// </summary>
        /// <param name="Percentage"> The percentage of time the return value will be true. </param>
        /// <returns></returns>
        public static bool Chance (float Percentage)
        {
            return UnityEngine.Random.Range(0, 100) <= Percentage;
        }

        //Defining a public random for the whole library to use
        private static System.Random rng = new System.Random();

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

        /// <summary>
        /// (Extension Method) Takes in a quaternion and strips it of everything axis except the Zed
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Quaternion RemoveRotationAxis2D(this Quaternion Val)
        {
            Quaternion _val = Val;

            _val.eulerAngles = new Vector3(0, 0, _val.eulerAngles.z);

            return _val;
        }

        /// <summary>
        /// Sets the specified axis of a quaternion to the value given
        /// </summary>
        /// <param name="_val"> (Extension Method) </param>
        /// <param name="Axis"> What axis to set: x : y : z : w. </param>
        /// <param name="Value"> The value to set the axis to. </param>
        /// <returns></returns>
        public static Quaternion SetAxis(this Quaternion _val, string Axis, float Value)
        {
            Quaternion _rot = _val;

            switch (Axis)
            {
                case "x":
                    _rot.x = Value;
                    break;

                case "y":
                    _rot.y = Value;
                    break;

                case "z":
                    _rot.z = Value;
                    break;

                case "w":
                    _rot.w = Value;
                    break;

                default:
                    break;
            }

            _val = _rot;
            return _rot;
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
