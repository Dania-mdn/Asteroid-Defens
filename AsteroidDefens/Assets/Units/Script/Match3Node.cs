using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Node : MonoBehaviour
{
    private parametrs parametrs;
    public GameObject highlight; // объект подсветки узла
    public int id;
    private int baseId;
    public bool ready { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public GameObject Active;

    private void Start()
    {
        parametrs = GetComponent<parametrs>();
        baseId = id;
    }
    public void AddLvL()
    {
        id = id + 10;
        parametrs.LvL = (id - baseId) / 10;
        if(Active.activeSelf == false)
            Active.SetActive(true);
        else
            SystemEvent.DoUpgrade();
    }
}
