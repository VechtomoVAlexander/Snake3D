using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    // Settings
    [SerializeField] private float baseMoveSpeed = 10f;   
    [SerializeField] private float baseRotateSpeed = 180f;
    [SerializeField] public float BodySpeed = 10f;        
    [SerializeField] public int Gap = 10;
    [SerializeField] public GameObject BodyPrefab;

    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();
    private List<Vector3> BodyPositionsSnapshot = new List<Vector3>(); 

    private float moveSpeed;
    private float rotateSpeed;

    private bool isMovementStopped = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        BodyParts.Clear();
        PositionsHistory.Clear();
    }

    void Start()
    {
        moveSpeed = baseMoveSpeed;
        rotateSpeed = baseRotateSpeed;
    }

    void Update()
    {
        if (!this) return;

        
        PositionsHistory.Insert(0, transform.position);

        if (isMovementStopped)
        {
           
            return;
        }

        
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        float rotation = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * rotation * rotateSpeed * Time.deltaTime);

        
        int i = 0;
        foreach (var body in BodyParts)
        {
            if (body == null) continue;

            int historyIndex = Mathf.Clamp(i * Gap, 0, PositionsHistory.Count - 1);
            if (PositionsHistory.Count == 0) continue;

            Vector3 point = PositionsHistory[historyIndex];
            Vector3 positionforward = point - body.transform.position;

            body.transform.position += positionforward * BodySpeed * Time.deltaTime;
            body.transform.LookAt(point);
            i++;
        }

        CheckBodyCollision();
    }

    public void StopMovement(bool stop)
    {
        if (stop == isMovementStopped) return; 

        if (stop)
        {
            
            BodyPositionsSnapshot.Clear();
            foreach (var body in BodyParts)
            {
                if (body != null)
                {
                    BodyPositionsSnapshot.Add(body.transform.position);
                }
            }
        }

       
        isMovementStopped = stop;

        if (!stop)
        {
           
            if (BodyParts.Count == BodyPositionsSnapshot.Count)
            {
                for (int i = 0; i < BodyParts.Count; i++)
                {
                    if (BodyParts[i] != null)
                    {
                        BodyParts[i].transform.position = BodyPositionsSnapshot[i];
                        BodyParts[i].transform.LookAt(transform.position);
                    }
                }
            }

            BodyPositionsSnapshot.Clear();

            
            ValidateBody();
        }
    }

    private void ValidateBody()
    {
        
        for (int i = 1; i < BodyParts.Count; i++)
        {
            GameObject part = BodyParts[i];
            if (part == null) continue;

            float distFromHead = Vector3.Distance(transform.position, part.transform.position);

            
            if (distFromHead < 0.3f)
            {
                Vector3 dirFromHead = (part.transform.position - transform.position).normalized;
                Vector3 safePosition = transform.position - dirFromHead * 0.5f;
                part.transform.position = safePosition;
                part.transform.LookAt(transform.position);
            }
        }
    }

    public void UpdateSpeedByScore(int score)
    {
        if (score < 125)
        {
            moveSpeed = 15f;
            BodySpeed = 15f;
        }
        else if (score < 200)
        {
            moveSpeed = 10f;
            BodySpeed = 10f;
        }
        else
        {
            moveSpeed = 5f;
            BodySpeed = 5f;
        }

        rotateSpeed = baseRotateSpeed;
    }

    public void GrowSnake()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(1);

        Vector3 spawnPosition;

        if (BodyParts.Count == 0)
        {
            spawnPosition = transform.position - transform.forward * 1f;
        }
        else
        {
            GameObject tail = BodyParts[BodyParts.Count - 1];
            if (tail == null)
            {
                return;
            }
            spawnPosition = tail.transform.position - tail.transform.forward * 1f;
        }

        GameObject body = Instantiate(BodyPrefab, spawnPosition, Quaternion.identity);
        if (body != null)
        {
            BodyParts.Add(body);
        }
    }

    public void CheckBodyCollision()
    {
        if (BodyParts == null || BodyParts.Count <= 1)
            return;

        if (isMovementStopped) return; 

        for (int i = 1; i < BodyParts.Count; i++)
        {
            GameObject part = BodyParts[i];
            if (part == null) continue;

            float dist = Vector3.Distance(transform.position, part.transform.position);
            if (dist < 0.5f)
            {
                if (SceneManager.GetActiveScene().buildIndex >= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                return;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == null || isMovementStopped) return;

        if (other.CompareTag("SnakeBody"))
        {
            if (BodyParts == null || BodyParts.Count <= 1)
                return;

            for (int i = 1; i < BodyParts.Count; i++)
            {
                GameObject part = BodyParts[i];
                if (part == null) continue;

                if (part == other.gameObject)
                {
                    if (SceneManager.GetActiveScene().buildIndex >= 0)
                    {
                        SceneManager.LoadScene(0);
                    }
                    return;
                }
            }
        }
    }
}