using Chrio;

public class LaserBolt : SharksBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward;
    }
}
