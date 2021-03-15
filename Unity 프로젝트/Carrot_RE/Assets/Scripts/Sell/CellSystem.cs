using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 화면 상 나타나는 Cell GO에 관련된 Class
public class CellSystem : MonoBehaviour
{
    PathMaker pathMaker;

    Bounds CellArea;
    GameObject[] Cells;

    int stage;

    // Start is called before the first frame update
    void Start()
    {
        pathMaker = GameObject.Find("System").transform.Find("Path Maker").GetComponent<PathMaker>();
        CellArea = GameObject.Find("Cell").transform.Find("Cell Area").GetComponent<BoxCollider2D>().bounds;
        
    }

    public void setCellData(List<int> _path, int _size)
    {
        // 기존 정보 삭제
        DelCells();

        setStage();

        MakeCellGO(_size);
        setCellInfo(_path, _size);
    }
    void setStage()
    {
        stage = Random.Range(0, 10);
    }

    #region Cell GO
    void MakeCellGO(int _size)
    {
        // Cell GO 관련 변수
        Cells = new GameObject[_size * _size];
        GameObject cell;
        GameObject parentGO = GameObject.Find("Cell"); // 부모 GO

        // Position 관련 변수
        Vector3 DivisionOfArea = (CellArea.max - CellArea.min) / _size;
        float revisionX = DivisionOfArea.x / 2, revisionY = DivisionOfArea.y / 2; // GO 위치의 경우, GO의 center 기준으로 설정됨. 중앙 기준인만큼, 일정량의 보정이 필요
        Vector3 CellPos = new Vector3();

        // Cell GO 생성
        for (int i = 0; i < _size * _size; i++)
        {
            // Generate Cell GO
            cell = Instantiate(Resources.Load("Prefabs/Cell")) as GameObject;
            //cell.name = "Cell" + i / _size + i % _size;
            cell.name = "Cell" + i;
            cell.transform.SetParent(parentGO.transform);

            // Position of GO
            CellPos.x = (CellArea.min.x + revisionX) + DivisionOfArea.x * (i % _size);
            // Stage따라 시작/도착 지점 보이는 것만 변경 >>> 수정 필요
            if(stage % 2 == 0)
                CellPos.y = (CellArea.max.y - revisionY) - DivisionOfArea.y * (i / _size);
            else
                CellPos.y = (CellArea.min.y + revisionY) + DivisionOfArea.y * (i / _size);
            CellPos.z = 0f;
            cell.transform.position = CellPos;

            // Add in Cell List
            Cells[i] = cell;
        }
    }
    // 기존 Cell GO들 모두 삭제
    void DelCells()
    {
        GameObject Cell = GameObject.Find("Cell");
        for (int i = 0; i < Cell.transform.childCount; i++)
        {
            if (Cell.transform.GetChild(i).name != "Cell Area")
                GameObject.Destroy(Cell.transform.GetChild(i).gameObject);
        }
    }
    #endregion

    #region Set Cell Data
    void setCellInfo(List<int> _path, int _size)
    {
        CellInfo cellInfo;
        int AmountOfSpecialObj = Random.Range((_size * _size - _path.Count) / 4, (_size*_size - _path.Count) / 2); // 파괴 불가능 || 아이템 Object의 개수

        // Path Maker에서 만들어 주는 Path 정보에는 '시작 지점'인 IDX [0]은 제외돼 있음. IDX [0]도 Path에 추가
        // [참고: 왜 0 빠져있나] Path Maker의 Path는 '수레가 다음에 이동할 곳'이 되기도 하는 만큼, 0이 들어갈 이유가 없음.
        _path.Insert(0, 0);

        for(int i = 0; i < Cells.Length; i++)
        {
            cellInfo = Cells[i].GetComponent<CellInfo>();

            // Classify Path Cell
            if (_path.Contains(i))
            {
                cellInfo.setRoad();
                if(i != 0 && i != Cells.Length - 1)
                {
                    int nowIDX = _path.IndexOf(i);
                    cellInfo.setPathDir(_path[nowIDX] - _path[nowIDX - 1], _path[nowIDX + 1] - _path[nowIDX]);
                }
            }
            else // 길이 아닌 경우
            {
                if (AmountOfSpecialObj > 0) // 생성할 특별 Obj의 수가 남은 경우
                {
                    if (Random.Range(0, 2) == 1) // 임의로 특별 Obj를 배치
                    {
                        cellInfo.setSpecialObj();
                        AmountOfSpecialObj--;
                    }
                }
            }
            // Set PathCell`s Object
            if (i == 0)
                cellInfo.setStart();
            else if (i == Cells.Length - 1)
                cellInfo.setDestination();
            else
                cellInfo.setObj();
        }

        System.GC.Collect();
    }

    void setCellArray()
    {

    }
    #endregion
}
