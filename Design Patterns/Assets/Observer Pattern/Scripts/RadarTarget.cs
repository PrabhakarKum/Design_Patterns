using UnityEngine;
public class RadarTarget : MonoBehaviour, IRadarDetectable
{
    public RadarObjectType type;
    public Sprite objectIcon;
    private void Start()
    {
        RadarSystem.Register(this);
    }

    private void OnDestroy()
    {
        RadarSystem.Unregister(this);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public RadarObjectType GetRadarType()
    {
        return type;
    }
    
    public Sprite GetRadarIcon()
    {
        return objectIcon;
    }
}
