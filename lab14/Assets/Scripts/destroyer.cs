using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void Update()
    {
        DestroyGameObject();
    }
    void DestroyGameObject()
    {
        Destroy(gameObject,0.5f);
    }
}
