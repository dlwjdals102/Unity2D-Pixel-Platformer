using UnityEngine;
using UnityEngine.SceneManagement;

public enum RespawnType
{
    Enter,
    Exit,
    None
}

public class Object_Waypoint : MonoBehaviour
{
    [SerializeField] private string transferToScene;
    [Space]
    [SerializeField] private RespawnType waypointType;
    [SerializeField] private RespawnType conntedWaypoint;
    [SerializeField] private Transform respwanPoint;
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
        return respwanPoint == null ? transform.position : respwanPoint.position;
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
}
