using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Match3Control : MonoBehaviour
{
    [SerializeField] private float speed = 5.5f; // скорость движения объектов
    [SerializeField] private float destroyTimeout = .5f; // пауза в секундах, перед тем как уничтожить совпадения
    [SerializeField] private LayerMask layerMask; // маска узла (префаба)
    [SerializeField] private int countUnit; // набор цветов/id
    [SerializeField] private int gridWidth = 7; // ширина игрового поля
    [SerializeField] private int gridHeight = 10; // высота игрового поля
    [SerializeField] private Match3Node[] Units; // разные юниты
    [SerializeField] private float sampleSize = 1; // размер узла (ширина и высота)

    public Match3Node[,] grid;
    private Match3Node[] nodeArray;
    private List<GameObject> FolseActiveArray;
    private Vector3[,] position;
    private Match3Node current, lastt, last;
    private Vector3 currentPos, lastPos;
    public List<List<Match3Node>> liness;
    private bool isLines, isMove;
    private float timeout;
    private bool isClick = false;
    public AudioSource Step;
    public AudioSource stack;

    private void OnEnable()
    {
        SystemEvent.MuteAudio += AudioMute;
        SystemEvent.PlayAudio += AudioPlay;
    }
    private void OnDisable()
    {
        SystemEvent.MuteAudio -= AudioMute;
        SystemEvent.PlayAudio -= AudioPlay;
    }
    void Start()
    {
        // создание игрового поля (2D массив) с заданными параметрами
        Create2DGrid(Units, gridWidth, gridHeight, sampleSize, transform);

        if (IsLine())
        {
            timeout = 0;
            isLines = true;
        }
    }
    public void delete(Match3Node match3Node)
    {
        match3Node.gameObject.SetActive(false);
        grid[match3Node.x, match3Node.y] = null;
        FolseActiveArray.Add(match3Node.gameObject);
        isMove = true;
        isLines = false;
    }
    void DestroyLines() // уничтожаем совпадения с задержкой
    {
        if (!isLines) return;

        timeout += Time.deltaTime;

        if (timeout > destroyTimeout)
        {
            for (int i = 0; i < liness.Count; i++)
            {
                if (current != null)
                {
                    for (int j = 0; j < liness[i].Count; j++)
                    {
                        if (liness[i][j] == current || liness[i][j] == lastt)
                        {
                            liness[i][j].AddLvL();
                            current = null;
                        }
                        else
                        {
                            liness[i][j].gameObject.SetActive(false);
                            grid[liness[i][j].x, liness[i][j].y] = null;
                            FolseActiveArray.Add(liness[i][j].gameObject);
                        }
                        if(j > 2)
                            SystemEvent.DoAddStep(1);
                    }
                }
                else
                {
                    for (int j = 0; j < liness[i].Count; j++)
                    {
                        if (j == 0)
                        {
                            liness[i][j].AddLvL();
                        }
                        else
                        {
                            liness[i][j].gameObject.SetActive(false);
                            grid[liness[i][j].x, liness[i][j].y] = null;
                            FolseActiveArray.Add(liness[i][j].gameObject);
                        }
                        if (j > 2)
                            SystemEvent.DoAddStep(1);
                    }
                }
                stack.Play();
            }

            isMove = true;
            isLines = false;
        }
    }

    void MoveNodes() // передвижение узлов и заполнение поля, после проверки совпадений
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
        SystemEvent.DoCloseStep();
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

        if(!isLines)
            SystemEvent.DoFullStep();
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

    void GridUpdate() // обновление игрового поля с помощью рейкаста
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
        if (FolseActiveArray.Count > 0)
        {
            for (int i = 0; i < FolseActiveArray.Count; i++)
            {
                Destroy(FolseActiveArray[i].gameObject);
                FolseActiveArray.RemoveAt(i);
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

    void SetNode(Match3Node node, bool value) // метка для узлов, которые находятся рядом с выбранным (чтобы нельзя было выбрать другие)
    {
        if (node == null) return;

        if (node.x - 1 >= 0) grid[node.x - 1, node.y].ready = value;
        if (node.y - 1 >= 0) grid[node.x, node.y - 1].ready = value;
        if (node.x + 1 < gridWidth) grid[node.x + 1, node.y].ready = value;
        if (node.y + 1 < gridHeight) grid[node.x, node.y + 1].ready = value;
    }

    void Control() // управление ЛКМ
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);
            
            if (hit.transform != null && current == null)
            {
                if (hit.transform.GetComponentInChildren<step>() != null)
                {
                    step step = hit.transform.GetComponentInChildren<step>();
                    if (step.IsOpen)
                    {
                        isLines = true;
                        step.SetOpen();
                        delete(current);
                        SystemEvent.DoCloseStep();
                    }
                    else
                    {
                        SystemEvent.DoCloseStep();
                        step.SetOpen();
                    }
                }
                else
                {
                    SystemEvent.DoCloseStep();
                }

                current = hit.transform.GetComponent<Match3Node>();
                SetNode(current, true);
                current.highlight.SetActive(true);
            }
            else if (hit.transform != null && current != null)
            {
                if(hit.transform.GetComponentInChildren<step>() != null)
                {
                    //current = hit.transform.GetComponent<Match3Node>();
                    step step = hit.transform.GetComponentInChildren<step>();
                    if (step.IsOpen)
                    {
                        isLines = true;
                        step.SetOpen();
                        delete(current);
                        SystemEvent.DoCloseStep();
                    }
                    else
                    {
                        SystemEvent.DoCloseStep();
                        step.SetOpen();
                    }
                }
                else
                {
                    SystemEvent.DoCloseStep();
                }

                last = hit.transform.GetComponent<Match3Node>();
                lastt = hit.transform.GetComponent<Match3Node>();

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

                Step.Play();
                current.highlight.SetActive(false);
                currentPos = current.transform.position;
                lastPos = last.transform.position;
            }
            isClick = true;
        }
    }

    bool IsLine() // поиск совпадений по горизонтали и вертикали
    {
        liness = new List<List<Match3Node>>();

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (x + 2 < gridHeight && grid[x + 1, y].id == grid[x, y].id && grid[x + 2, y].id == grid[x, y].id && liness.Any(list => list.Contains(grid[x, y])) == false)
                {
                    List<Match3Node> innerList = new List<Match3Node>();
                    int start = grid[x, y].x;
                    for (int xx = start; xx < gridWidth; xx++)
                    {
                        if (grid[xx, y].id == grid[start, y].id)
                        {
                            innerList.Add(grid[xx, y]);
                        }
                        else
                        {
                            break;
                        }
                    }
                    liness.Add(innerList);
                    break;
                }
            }
        }

        for (int y = 0; y < gridWidth; y++)
        {
            for (int x = 0; x < gridHeight; x++)
            {
                if (x + 2 < gridHeight && grid[y, x + 1].id == grid[y, x].id && grid[y, x + 2].id == grid[y, x].id && liness.Any(list => list.Contains(grid[y, x])) == false)
                {
                    List<Match3Node> innerList = new List<Match3Node>();
                    int start = grid[x, y].x;
                    for (int xx = start; xx < gridHeight; xx++)
                    {
                        if (grid[y, xx].id == grid[y, start].id)
                        {
                            innerList.Add(grid[y, xx]);
                        }
                        else
                        {
                            break;
                        }
                    }
                    liness.Add(innerList);
                    break;
                }
            }
        }
        //for (int y = 0; y < liness.Count; y++)
        //for (int x = 0; x < liness[y].Count; x++)
        //    Debug.Log(y + " " + x + " " + liness[y][x].name);
        return (liness.Count > 0) ? true : false;
    }

    // функция создания 2D массива на основе шаблона
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
    public void AudioMute()
    {
        Step.mute = true;
        stack.mute = true;
    }
    public void AudioPlay()
    {
        Step.mute = false;
        stack.mute = false;
    }
}
