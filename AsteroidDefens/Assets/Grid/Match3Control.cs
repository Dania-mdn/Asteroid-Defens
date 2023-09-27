using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Control : MonoBehaviour
{
    private enum Mode { MatchOnly, FreeMove };

    [SerializeField] private float speed = 5.5f; // скорость движени€ объектов
    [SerializeField] private float destroyTimeout = .5f; // пауза в секундах, перед тем как уничтожить совпадени€
    [SerializeField] private LayerMask layerMask; // маска узла (префаба)
    [SerializeField] private int countUnit; // набор цветов/id
    [SerializeField] private int gridWidth = 7; // ширина игрового пол€
    [SerializeField] private int gridHeight = 10; // высота игрового пол€
    [SerializeField] private Match3Node[] Units; // разные юниты
    [SerializeField] private float sampleSize = 1; // размер узла (ширина и высота)

    public Match3Node[,] grid;
    private Match3Node[] nodeArray;
    private List<GameObject> FolseActiveArray;
    private Vector3[,] position;
    private Match3Node current, last;
    private Vector3 currentPos, lastPos;
    private List<Match3Node> lines;
    private bool isLines, isMove;
    private float timeout;
    private bool isClick = false;

    void Start()
    {
        // создание игрового пол€ (2D массив) с заданными параметрами
        Create2DGrid(Units, gridWidth, gridHeight, sampleSize, transform);

        if (IsLine())
        {
            timeout = 0;
            isLines = true;
        }
    }
    void DestroyLines() // уничтожаем совпадени€ с задержкой
    {
        if (!isLines) return;

        timeout += Time.deltaTime;

        if (timeout > destroyTimeout)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                // здесь можно подсчитывать очки +1
                if (current != null)
                {
                    if (lines[i] != current)
                    {
                        lines[i].gameObject.SetActive(false);
                        grid[lines[i].x, lines[i].y] = null;
                        FolseActiveArray.Add(lines[i].gameObject);
                    }
                    else
                    {
                        current.AddLvL();
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        lines[0].AddLvL();
                    }
                    else
                    {
                        lines[i].gameObject.SetActive(false);
                        grid[lines[i].x, lines[i].y] = null;
                        FolseActiveArray.Add(lines[i].gameObject);
                    }
                }
            }

            isMove = true;
            isLines = false;
        }
    }

    void MoveNodes() // передвижение узлов и заполнение пол€, после проверки совпадений
    {
        if (!isMove) return;

        for (int y = gridHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (grid[x, gridHeight-1] == null)
                {
                    bool check = true;

                    for (int i = 0; i < gridWidth; i++)
                    {
                        if (grid[i, gridHeight-1] == null)
                        {
                            grid[i, gridHeight-1] = GetFree(position[i, gridHeight-1]);
                        }
                    }

                    for (int i = 0; i < nodeArray.Length; i++)
                    {
                        if (!nodeArray[i].gameObject.activeSelf) check = false;
                    }

                    if (check)
                    {
                        isMove = false;
                        GridUpdate();

                        if (IsLine())
                        {
                            timeout = 0;
                            isLines = true;
                        }
                    }
                }
                if (grid[x, y] != null && y > 0 && grid[x, y].gameObject.activeSelf && grid[x, y - 1] == null)
                {
                    grid[x, y].transform.position = Vector3.MoveTowards(grid[x, y].transform.position, position[x, y - 1], speed * Time.deltaTime);

                    if (grid[x, y].transform.position == position[x, y - 1])
                    {
                        grid[x, y - 1] = grid[x, y];
                        grid[x, y] = null;
                    }
                }
            }
        }
    }

    void Update()
    {
        DestroyLines();

        MoveNodes();

        if (isLines || isMove) return;

        if (last == null)
        {
            Control();
        }
        else
        {
            MoveCurrent();
        }

        if (FolseActiveArray.Count > 0)
        {
            for (int i = 0; i < FolseActiveArray.Count; i++)
            {
                Destroy(FolseActiveArray[i].gameObject);
                FolseActiveArray.RemoveAt(i);
            }
            SystemEvent.DoFullStep();
        }
    }

    Match3Node GetFree(Vector3 pos) // возвращает неактивный узел
    {
        for (int i = 0; i < nodeArray.Length; i++)
        {
            if (!nodeArray[i].gameObject.activeSelf)
            {
                int j = Random.Range(0, countUnit);
                nodeArray[i] = Instantiate(Units[j], pos, Quaternion.identity, transform);
                nodeArray[i].transform.position = pos;
                nodeArray[i].gameObject.SetActive(true);
                return nodeArray[i];
            }
        }

        return null;
    }

    void GridUpdate() // обновление игрового пол€ с помощью рейкаста
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                RaycastHit2D hit = Physics2D.Raycast(position[x, y], Vector2.zero, Mathf.Infinity, layerMask);

                if (hit.transform != null)
                {
                    grid[x, y] = hit.transform.GetComponent<Match3Node>();
                    grid[x, y].ready = false;
                    grid[x, y].x = x;
                    grid[x, y].y = y;
                }
            }
        }
        if (isClick)
        {
            SystemEvent.DoStep();
            isClick = false;
        }
    }

    void MoveCurrent() // перемещение выделенного мышкой узла
    {
        current.transform.position = Vector3.MoveTowards(current.transform.position, lastPos, speed * Time.deltaTime);
        last.transform.position = Vector3.MoveTowards(last.transform.position, currentPos, speed * Time.deltaTime);

        if (current.transform.position == lastPos && last.transform.position == currentPos)
        {
            GridUpdate();
            last = null;

            if (IsLine())
            {
                timeout = 0;
                isLines = true;
            }
        }
    }

    void SetNode(Match3Node node, bool value) // метка дл€ узлов, которые наход€тс€ р€дом с выбранным (чтобы нельз€ было выбрать другие)
    {
        if (node == null) return;

        if (node.x - 1 >= 0) grid[node.x - 1, node.y].ready = value;
        if (node.y - 1 >= 0) grid[node.x, node.y - 1].ready = value;
        if (node.x + 1 < gridWidth) grid[node.x + 1, node.y].ready = value;
        if (node.y + 1 < gridHeight) grid[node.x, node.y + 1].ready = value;
    }

    void Control() // управление Ћ ћ
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

            if (hit.transform != null && current == null)
            {
                current = hit.transform.GetComponent<Match3Node>();
                SetNode(current, true);
                current.highlight.SetActive(true);
            }
            else if (hit.transform != null && current != null)
            {
                last = hit.transform.GetComponent<Match3Node>();

                if (last != null && !last.ready)
                {
                    current.highlight.SetActive(false);
                    last.highlight.SetActive(true);
                    SetNode(current, false);
                    SetNode(last, true);
                    current = last;
                    last = null;
                    return;
                }

                current.highlight.SetActive(false);
                currentPos = current.transform.position;
                lastPos = last.transform.position;
            }
            isClick = true;
        }
    }

    bool IsLine() // поиск совпадений по горизонтали и вертикали
    {
        int j = -1;

        lines = new List<Match3Node>();

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (x + 2 < gridWidth && j < 0 && grid[x + 1, y].id == grid[x, y].id && grid[x + 2, y].id == grid[x, y].id)
                {
                    j = grid[x, y].id;
                }

                if (j == grid[x, y].id)
                {
                    lines.Add(grid[x, y]);
                }
                else
                {
                    j = -1;
                }
            }

            j = -1;
        }

        j = -1;

        for (int y = 0; y < gridWidth; y++)
        {
            for (int x = 0; x < gridHeight; x++)
            {
                if (x + 2 < gridHeight && j < 0 && grid[y, x + 1].id == grid[y, x].id && grid[y, x + 2].id == grid[y, x].id)
                {
                    j = grid[y, x].id;
                }

                if (j == grid[y, x].id)
                {
                    lines.Add(grid[y, x]);
                }
                else
                {
                    j = -1;
                }
            }

            j = -1;
        }

        return (lines.Count > 0) ? true : false;
    }

    // функци€ создани€ 2D массива на основе шаблона
    private void Create2DGrid(Match3Node[] sample, int width, int height, float size, Transform parent)
    {
        grid = new Match3Node[width, height];

        float posX = -size * width / 2f - size / 2f;
        float posY = size * height - size;

        position = new Vector3[gridWidth, gridHeight];
        nodeArray = new Match3Node[gridWidth * gridHeight];
        FolseActiveArray = new List<GameObject>();

        float Xreset = posX;

        int i = 0;
        int id = -1;
        int step = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int j = Random.Range(0, countUnit);
                if (id != j) id = j; else step++;
                if (step > 2)
                {
                    step = 0;
                    id = (id + 1 < countUnit - 1) ? id + 1 : id - 1;
                }

                posX += size;
                grid[x, y] = Instantiate(sample[id], new Vector3(posX, posY, 0), Quaternion.identity, parent);
                grid[x, y].x = x;
                grid[x, y].y = y;
                grid[x, y].ready = false;
                grid[x, y].gameObject.SetActive(true);
                grid[x, y].highlight.SetActive(false);
                position[x, y] = grid[x, y].transform.position;
                nodeArray[i] = grid[x, y];
                i++;
            }
            posY -= size;
            posX = Xreset;

            current = null;
            last = null;
        }
    }
}
