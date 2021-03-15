using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{

    // for Move
    public float WolfSpeed;
    Rigidbody2D rb;

    // for Targeting
    bool hasTarget = false;
    public float SearchDistance;

    public LayerMask TargetLayer;
    Collider2D[] Sheeps;
    Sheep TargetSheep;

    float SearchCoolTime = 0.5f;
    float searchTimer;

    // for Kill
    bool killing = false;
    public float KillTime;
    float killTimer;

    // Start is called before the first frame update
    void Start()
    {
        //TargetLayer.value = LayerMask.NameToLayer("Sheep");
        rb = this.GetComponent<Rigidbody2D>();

        searchTimer = 0; killTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTarget)
        {
            // 도착 감지
            // Killing

            // 타겟을 향해 이동
            Move();
            if (killing)
            {
                Kill();
            }
        }
        else
        {
            // 임의 방향으로 이동

            searchTimer += Time.deltaTime;
            if(searchTimer >= SearchCoolTime)
            {
                searchTimer = 0;
                SearchTarget();
            }
        }
    }

    #region Target
    void SearchTarget()
    {
        Sheeps = Physics2D.OverlapCircleAll(this.transform.position, SearchDistance, TargetLayer);

        if(Sheeps.Length > 0)
        {
            float shortestDistance = Mathf.Infinity;
            Collider2D closestSheep = null;

            float sheepDistance;
            Sheep candidateSheep;

            for (int i = 0; i < Sheeps.Length; i++)
            {
                candidateSheep = Sheeps[i].GetComponent<Sheep>();

                // 1) 다른 늑대의 Target이 아닌가 판별
                if (!candidateSheep.IsTargetted()) 
                {
                    // 2) 더 가까운 양인지 판별
                    sheepDistance = (this.transform.position - Sheeps[i].transform.position).magnitude;
                    if (shortestDistance > sheepDistance)
                    {
                        // 3) 시간 내 도착해 처치할 수 있는지 판별
                        if (CanReachInTime(candidateSheep, sheepDistance))
                        {
                            shortestDistance = sheepDistance;
                            closestSheep = Sheeps[i];
                        }
                    }
                }
            }

            if(closestSheep == null) // Target 가능한 양 X
            {
                hasTarget = false;
            }
            else
            {
                TargetSheep = closestSheep.GetComponent<Sheep>();
                TargetSheep.Targetted();
                hasTarget = true;
            }
        }
    }

    // (양과의 거리 / 늑대 이동 속도) 로 나오는 시간 < 실제 도달까지 소요되는 시간 :: 시간 보정값을 추가해서, 가는 도중에 사라지는 일 없도록 함 (실 소요시간 - 계산 시간 사이에 양이 사라질 수도 있음. 이 간극을 최대한 적게)
    // 2.0f :: Wolf Speed 400일 때, 2.0f를 시간 보정값으로 설정하면 "계산 시간과 실제 소요 시간의 차가 거의 미미"
    // Wolf Speed > 400 -> 계산 시간 < 실소요시간 || Wolf Speed < 400 -> 계산 시간 > 실소요시간 (차이는 크게 없음)
    const float CORRECTIONTIME = 2.0f;
    bool CanReachInTime(Sheep _candidate_sheep, float _sheep_distance)
    {
        // 양의 잔여 시간 - (양과의 거리 / 늑대 이동 속도 + 처치 시간) :: +면 도착&처치 가능, -면 처치 불가
        float ReachTime = _candidate_sheep.getRemainingTime() - (_sheep_distance / WolfSpeed + KillTime + CORRECTIONTIME);

        return true;

        if (ReachTime >= 0)
            return true;
        else
            return false;
    }
    #endregion

    #region Move
    bool hasDestination = false;

    Vector3 Destination;
    Vector2 velocity = Vector2.zero;
    void Move()
    {
        if(!hasDestination)
        {
            Destination = FindClosestSide();
            hasDestination = true;
        }
        else
        {
            this.transform.position = Vector2.SmoothDamp(this.transform.position, Destination, ref velocity, 1f, WolfSpeed);

            // 도착 판정
            if (Mathf.Abs(this.transform.position.x - Destination.x) <= 5f && 
                Mathf.Abs(this.transform.position.y - Destination.y) <= 5f)
            {
                // Kill
                killing = true;
            }
        }
        
        // Move to Destination
    }

    // Sheep.BoxCollider의 4변 중 가장 가까운 지점 Search
    Vector3 FindClosestSide()
    {
        Bounds bound = TargetSheep.GetComponent<BoxCollider2D>().bounds;
        Vector2 closestSide = new Vector2();

        // 각 Side 위치 설정
        Vector2 middle = (bound.max - bound.min)/2;
        Vector3[] sides = new Vector3[4];
        sides[0] = new Vector3(bound.min.x + middle.x, bound.max.y);
        sides[1] = new Vector3(bound.min.x + middle.x, bound.min.y);
        sides[2] = new Vector3(bound.max.x, bound.min.y + middle.y);
        sides[3] = new Vector3(bound.min.x, bound.min.y + middle.y);

        // 가장 가까운 Side 탐색
        float shortestDistance = Mathf.Infinity;
        float distance;
        for(int i = 0; i < sides.Length; i++)
        {
            distance = Vector2.SqrMagnitude(this.transform.position - sides[i]);
            if(shortestDistance > distance)
            {
                shortestDistance = distance;
                closestSide = sides[i];
            }
        }

        return closestSide;
    }
    #endregion

    #region Kill
    void Kill()
    {
        killTimer += Time.deltaTime;
        if (killTimer >= KillTime)
        {
            TargetSheep.Kill();
            resetVar();
        }
    }
    #endregion

    #region Change Target
    public void ChangeTarget(string _sheep_name)
    {
        if (TargetSheep.name == _sheep_name)
            resetVar();
    }
    #endregion

    #region Get
    public bool getHasTarget()
    {
        return hasTarget;
    }
    #endregion

    #region Set
    void resetVar()
    {
        hasTarget = false;
        hasDestination = false;
        killing = false;
        TargetSheep = null;
    }
    #endregion
}
