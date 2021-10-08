using Chrio;
using UnityEngine;

public class SizeModifier : SharksBehaviour
{
    public Vector2 ScaleMultiplier;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(GlobalState.Game.MainCamera.orthographicSize * ScaleMultiplier.x, GlobalState.Game.MainCamera.orthographicSize * ScaleMultiplier.y);
    }
}
