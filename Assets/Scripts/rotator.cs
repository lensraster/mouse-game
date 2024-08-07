
using UnityEngine;

public class rotator : MonoBehaviour
{
    enum Axises { x, y, z }

    [SerializeField] Axises axises;
    [SerializeField] float speed;

    void Update()
    {
      Vector3 localRot = transform.localEulerAngles;
        float rotation = speed * Time.deltaTime;
      switch (axises)
        {
            case Axises.x:localRot.x += rotation; break;
            case Axises.y:localRot.y += rotation; break;
            case Axises.z:localRot.z += rotation; break;
        }
        transform.localEulerAngles = localRot;
    }
}
