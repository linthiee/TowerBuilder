using UnityEngine;

[CreateAssetMenu(fileName = "BlockSettings", menuName = "TowerBuilder/BlockSettings")]
public class BlockSettingsSO : ScriptableObject
{
    [Header("Audio")]
    public AudioClip blockDropped;
    public AudioClip groundCollision;
    public AudioClip onSlopeCollision;

    [Header("Physics and playability")]
    public float snapTolerance = 0.5f;
    public int normalPoints = 100;
    public int perfectPoints = 250;
}