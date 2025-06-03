using UnityEngine;

public abstract class CardBase : MonoBehaviour, I_Touchable
{
    public int cost;
    public UnitBase unit;
    public GameManager gm;
    protected static Camera mainCamera;
    private float distanceFromCamera;
    private Transform cardStartPosition;
    public LayerMask mask;

    [SerializeField] private ScriptableCardBase card;
    public ScriptableCardBase cardProperty
    {
        get
        {
            return card;
        }
        set
        {
            card = value;
            cost = card.cost;
            unit = card.unit;
        }
    }

    protected void Awake()
    {
        mainCamera = Camera.main;
        cardProperty = card;
        gm = FindFirstObjectByType<GameManager>();
    }

    public void OnTouchBegin(Vector2 touchPosition, float distance)
    {
        if (gm.state == GameState.Pause)
        {
            return;
        }
        Debug.Log("You clicked on a card");
        distanceFromCamera = distance;
    }

    public void OnTouchEnd(Vector2 touchPosition)
    {
        if(gm.state == GameState.Pause)
        {
            return;
        }
        transform.position = cardStartPosition.position;
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        gameObject.GetComponent<Collider>().enabled = false;
        if (Physics.Raycast(ray, out hit, 200f, mask))
        {
            if (hit.collider != null)
            {
                Vector3 screenCoordinates = new Vector3(touchPosition.x, touchPosition.y, hit.transform.position.z);
                Vector3 newPosition = mainCamera.ScreenToWorldPoint(screenCoordinates);
                switch (gm.state)
                {
                    case GameState.Deploy:
                        gm.playerManager.RemoveSpawnedCard(this);
                        SpawnUnit(hit.point);
                        break;
                    case GameState.Battle:
                        gm.playerManager.RemoveSpawnedCard(this);
                        UseAbility();
                        break;
                    case GameState.Pause:
                        break;
                    default:
                        break;
                }
                if (gm.state == GameState.Battle)
                {
                    
                }
                else if (gm.state == GameState.Deploy)
                {
                    
                }
                else
                {
                    print("You can't do anything yet");
                }
                
            }
        }
        gameObject.GetComponent<Collider>().enabled = true;
        


    }

    public void OnTouchStay(Vector2 touchPosition)
    {
        if (gm.state == GameState.Pause)
        {
            return;
        }
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, distanceFromCamera)); //mainCamera.nearClipPlane));
        //print(worldPoint);
        worldPoint.y = transform.position.y;
        transform.position = worldPoint;

    }

    public void SpawnUnit(Vector3 spawnPosition)
    {
        Destroy(gameObject);
        var unit = Instantiate(card.unit, new Vector3(spawnPosition.x, spawnPosition.y + 0.5f, spawnPosition.z), Quaternion.identity);
        GameManager.gm.friendlyUnits.Add(unit);

    }

    public abstract void UseAbility();

    public void SetOriginalCardPosition(Transform positionInHand)
    {
        cardStartPosition = positionInHand;
    }
}
