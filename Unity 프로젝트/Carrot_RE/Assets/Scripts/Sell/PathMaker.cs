using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

class PathCell
{
    public PathCell(int _idx, bool _road, bool _arrive)
    {
        this.IDX = _idx;
        this.IsRoad = _road;
        this.CanArrive = _arrive;
    }

    public int IDX;
    public bool IsRoad;
    public bool CanArrive;
}

public class PathMaker : MonoBehaviour
{
    const int LEFT = 0, RIGHT = 1, UP = 2, DOWN = 3;

    CellSystem cellSystem;

    WagonSystem wagonSystem;

    // Start is called before the first frame update
    void Start()
    {
        cellSystem = GameObject.Find("System").transform.Find("Cell System").GetComponent<CellSystem>();

        wagonSystem = GameObject.Find("Wagon").GetComponent<WagonSystem>();
    }

    List<int> PathIDX;
    public void Make(int _size)
    {
        PathIDX = MakePath(_size);

        cellSystem.setCellData(PathIDX, size);

        wagonSystem.setWagon(PathIDX);

        System.GC.Collect();
    }

    #region Making Path
    // 길 만들기 수행 함수
    // 시작 지점이 0, 도착 지점이 size^2-1인 배열(PathCells)이 만들어 짐
    // 해당 배열에서 길의 경로인 Path를 반환(수레의 진행 경로와 동일하게 만들어 짐)
    int size;
    PathCell[] PathCells;
    List<int> MakePath(int _size)
    {
        List<int> Path = new List<int>();
        size = _size;

        // PathCells = 미니게임이 이루어질 배열의 데이터화
        PathCells = new PathCell[size * size];
        PathCell tmp;
        for (int i = 0; i < PathCells.Length; i++)
        {
            tmp = new PathCell(i, false, true);
            PathCells[i] = tmp;
        }
        PathCells[0].IsRoad = true;

        // 길 만들기 반복 수행
        // 가장 최근 만들어진 길에서, 다음 길이 될 Cell에 대한 판별 수행
        int nowIDX = 0;
        while (nowIDX != size * size - 1)
        {
            nowIDX = SetDir(nowIDX);
            Path.Add(nowIDX);
        }

        return Path;
    }
    #endregion

    #region Set Direction of Next Path
    // 다음 길이 될 Cell을 결정하는 함수
    // 다음 길이 될 수 있는 Cell들 중 임의의 Cell을 추출, 그 IDX를 return
    int SetDir(int _nowIDX)
    {
        int nextIDX = -1;
        int[] adjacCell;
        
        // 인접 Cell들에 대해 도착 지점과 연결돼 있는지 판별
        // 해당 과정을 거치면 1) 길이 아니고 2) 도착 지점과 연결된 Cell들만 멤버 CanArrive == true인 상태가 됨
        adjacCell = FindAdjacent(_nowIDX);
        for(int i = 0; i < adjacCell.Length; i++)
        {
            if (adjacCell[i] == -1)
                continue;

            FindCanArrive(adjacCell[i]);
        }

        // 길을 만들 Cell 임의 설정
        int randDir;
        while (true)
        {
            randDir = Random.Range(0, 4);

            // 해당 방향의 Cell이 길인 경우
            if (adjacCell[randDir] == -1)
                continue;
            // 해당 방향의 Cell이 도착 지점과 연결되지 않아, 해당 방향으로 길을 낼 수 없는 경우
            if (PathCells[adjacCell[randDir]].CanArrive == false)
                continue;
            break;
        }

        // 선정된 Cell을 길로 설정
        nextIDX = adjacCell[randDir];
        PathCells[nextIDX].IsRoad = true;

        return nextIDX;
    }
    #endregion

    #region Finder of Proper Cells
    // 특정 idx의 Cell을 넣었을 때, 해당 Cell이 도착 지점과 연결돼 있는지 확인하는 함수
    // 해당 Cell로 길을 만들었을 때, 길이 도착 지점까지 나아갈 수 있는지 확인하는 함수
    // BFS 이용
    Queue Q = new Queue();
    void FindCanArrive(int idx)
    {
        // 방문 현황 Checking용 배열
        bool[] visit = new bool[PathCells.Length];
        for(int i = 0; i < PathCells.Length; i++)
        {
            if (PathCells[i].IsRoad == true)
                visit[i] = true;
        }
        // idx Cell 초기 설정
        visit[idx] = true;
        Q.Enqueue(idx);

        // BFS 파트
        int r;
        int[] adjac;
        bool arrive = false; // 해당 idx의 Cell이 도착 지점과 연결돼 있는지 값을 저장하는 변수
        while (Q.Count > 0)
        {
            r = (int)Q.Dequeue();

            if(r == size*size - 1)
            {
                // 도착
                PathCells[idx].CanArrive = true;
                arrive = true;
                break;
            }

            // 인접 Cell 중 '갈 수 있는 곳'만 큐에 push
            // 따라서 FindAdjacent 이후, 값이 -1이 아닌 방향의 Cell들을 visit 판별 → Q에 push
            adjac = FindAdjacent(r);
            for(int i = 0; i < adjac.Length; i++)
            {
                if (adjac[i] == -1)
                    continue;

                if(visit[adjac[i]] == false)
                {
                    visit[adjac[i]] = true;
                    Q.Enqueue(adjac[i]);
                }
            }
        }

        // arrive == false: 도착 지점과 연결돼 있지 않다는 뜻
        if (arrive == false)
            PathCells[idx].CanArrive = false;

        Q.Clear();
    }
    // idx Cell 근처의 '길'이 아닌 Cell들의 IDX을 찾아주는 함수
    int[] FindAdjacent(int idx)
    {
        int[] AdjaIDXArr = new int[4];

        // 벽인 경우
        if (idx % size != 0)
            AdjaIDXArr[LEFT] = idx - 1;
        else
            AdjaIDXArr[LEFT] = -1;
        if (idx % size != size - 1)
            AdjaIDXArr[RIGHT] = idx + 1;
        else
            AdjaIDXArr[RIGHT] = -1;
        if (idx / size != 0)
            AdjaIDXArr[UP] = idx - size;
        else
            AdjaIDXArr[UP] = -1;
        if (idx / size != size - 1)
            AdjaIDXArr[DOWN] = idx + size;
        else
            AdjaIDXArr[DOWN] = -1;

        // 길인 경우
        for(int i = 0; i < AdjaIDXArr.Length; i++)
        {
            if(AdjaIDXArr[i] != -1)
            {
                if (PathCells[AdjaIDXArr[i]].IsRoad == true)
                    AdjaIDXArr[i] = -1;
            }
        }

        return AdjaIDXArr;
    }
    #endregion
}
