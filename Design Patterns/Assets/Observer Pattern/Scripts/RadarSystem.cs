using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RadarSystem : MonoBehaviour
{
    private static RadarSystem Instance { get; set; }
    
    private List<IRadarDetectable> _detectables = new List<IRadarDetectable>();
    private List<GameObject> _blips = new List<GameObject>();
    
    [Header("Radar Settings")]
    public Transform player;
    public RectTransform radarUI;
    public GameObject playerIcon;
    public float radarRange = 50f;
    public float radarSize = 100f;

    [Header("Blip Prefabs")]
    public GameObject blipPrefab;
    
    private void Awake()
    {
        Instance = this;
    }
    public static void Register(IRadarDetectable target)
    {
        if (Instance != null && !Instance._detectables.Contains(target))
        {
            Debug.Log("Registered target: " + target.GetTransform().name);
            Instance._detectables.Add(target);
        }
        else if (Instance == null)
        {
            Debug.LogError("RadarSystem.Instance is null during registration!");
        }
        
        
    }

    public static void Unregister(IRadarDetectable target)
    {
        if (Instance != null && Instance._detectables.Contains(target))
            Instance._detectables.Remove(target);
    }
    private void Update()
    {
        UpdatePlayerIcon();
        DrawRadarBlips();
    }

    private void UpdatePlayerIcon()
    {
        if (playerIcon != null && player != null)
            return;
        
        // Use the player's forward direction and rotate it into radar space
        Vector3 playerForward = player.forward;
        Vector3 radarForward = Quaternion.Euler(0, -player.eulerAngles.y, 0) * playerForward;

        // Convert radarForward (XZ) to 2D UI space (XY)
        Vector2 forward2D = new Vector2(radarForward.x, radarForward.z).normalized;

        // Calculate angle in degrees
        float angle = Mathf.Atan2(forward2D.y, forward2D.x) * Mathf.Rad2Deg;

        // Set the rotation of the playerIcon (rotate around Z in UI)
        playerIcon.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private void DrawRadarBlips()
    {
        ClearBlips();
        foreach (var target in _detectables)
        {
            Vector3 worldOffset = target.GetTransform().position - player.position;

            // Rotate offset based on player's current Y rotation
            Vector3 rotatedOffset = Quaternion.Euler(0, -player.eulerAngles.y, 0) * worldOffset;

            // Convert world to radar space (XZ to XY)
            Vector2 radarOffset = new Vector2(rotatedOffset.x, rotatedOffset.z);

            if (radarOffset.magnitude <= radarRange)
            {
                // Scale to radar size
                float scaledX = (radarOffset.x / radarRange) * radarSize;
                float scaledY = (radarOffset.y / radarRange) * radarSize;
                Vector2 radarPos = new Vector2(scaledX, scaledY);

                GameObject blip = Instantiate(blipPrefab, radarUI);
                blip.GetComponent<RectTransform>().anchoredPosition = radarPos;

                Image img = blip.GetComponent<Image>();
                if (img != null)
                {
                    img.sprite = target.GetRadarIcon();
                }

                _blips.Add(blip);
            }
        }
    }

    private void ClearBlips()
    {
        foreach (GameObject blip in _blips)
        {
            Destroy(blip);
        }
        _blips.Clear();
    }
}

