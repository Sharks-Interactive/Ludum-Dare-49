using Chrio;
using SharkUtils;
using UnityEngine;

public class Parrallax : SharksBehaviour
{
    public float ParrallaxFactor;

    public Vector3 Offset;

    void Update()
    {
        transform.position = GlobalState.Game.MainCamera.transform.position * ParrallaxFactor;
        transform.position = transform.position.UpdateAxisEuler(0, ExtraFunctions.Axis.Z);
        transform.position += Offset;
    }

    private void OnDrawGizmosSelected()
    {
        transform.position = Offset;
    }
}
