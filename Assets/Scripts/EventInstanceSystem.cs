using UnityEngine;

public class EventInstanceSystem : MonoBehaviour
{
    public void Awake()
    {
        EventInstanceSystem[] objs = FindObjectsOfType<EventInstanceSystem>();

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
