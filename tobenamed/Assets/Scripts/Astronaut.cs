using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astronaut : MonoBehaviour
{
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private TimeManager timeManager;


    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.gameObject.ToString());
    }
    // Start is called before the first frame update
    void Start()
    {
        timeManager.TimeUpdateEventHandler += TimeUpdate;
    }

    // This TimeUpdate function is from the timeManager class. The event is sent wheenever a frame update occurs and the gameplay is not paused. 
    void TimeUpdate(object sender, TimeUpdateEventArgs e)
    {
        transform.position += (Vector3)(velocity * e.deltaTime);
    }

    public void SetVelocity(Vector2 velocity) {
        this.velocity = velocity;
    }

    public void ReflectAbout(Vector2 surfaceNormal) {
        velocity = 2 * Vector2.Dot(-velocity, surfaceNormal) / Vector2.Dot(surfaceNormal, surfaceNormal) * surfaceNormal + velocity;
    }
    
}
