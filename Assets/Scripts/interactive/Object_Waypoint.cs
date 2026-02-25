using UnityEngine;
using UnityEngine.SceneManagement;



public class Object_Waypoint : MonoBehaviour, ISaveable
{
    [SerializeField] private string transferToScene;
    [Space]
    [SerializeField] private RespawnType waypointType;
    [SerializeField] private RespawnType conntedWaypoint;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private bool canBeTriggered = true;

    public void SetCanBeTriggered(bool canBeTriggered)
    {
        this.canBeTriggered = canBeTriggered;
    }

    public RespawnType GetWaypointType()
    {
        return waypointType;
    }

    public Vector3 GetPosition()
    {
        return respawnPoint == null ? transform.position : respawnPoint.position;
    }

    private void OnValidate()
    {
        gameObject.name = "Object_Waypoint - " + waypointType.ToString() + " - " + transferToScene;
        
        if (waypointType == RespawnType.Enter)
            conntedWaypoint = RespawnType.Exit;

        if (waypointType == RespawnType.Exit)
            conntedWaypoint = RespawnType.Enter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeTriggered)
            return;

        GameManager.instance.ChangeScene(transferToScene, conntedWaypoint);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggered = true;
    }

    public void SaveData(ref GameData data)
    {
        
    }

    public void LoadData(GameData data)
    {
        
    }
}
