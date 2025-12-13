using UnityEngine;

[CreateAssetMenu(fileName = "NewTankData", menuName = "Tank Game/Tank Data")]
public class TankData : ScriptableObject
{
    public string tankName;
    public float moveSpeed = 5f;
    public float rotateSpeed = 150f;
    public int health = 100;
    public int damage = 10;
}