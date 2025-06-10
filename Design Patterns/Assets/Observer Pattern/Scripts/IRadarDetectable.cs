using UnityEngine;


public enum RadarObjectType
{
    Enemy,
    MedicalKit,
    Collectible
}

public interface IRadarDetectable
{
    Transform GetTransform();
    RadarObjectType GetRadarType();
    Sprite GetRadarIcon();
}
