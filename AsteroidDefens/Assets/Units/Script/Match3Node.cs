using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3Node : MonoBehaviour
{
    public SpriteRenderer sprite; // ������ ����
    public GameObject highlight; // ������ ��������� ����
    public int id;
    public bool ready { get; set; }
    public int x { get; set; }
    public int y { get; set; }
}
