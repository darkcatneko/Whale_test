using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMap : MonoBehaviour
{
    public MapBlockRow[] ThisMap = new MapBlockRow[5];
    //之後改成addressable
    public GameObject[] ArmorBlocks = new GameObject[5];
    public GameObject[] SlashBlocks = new GameObject[5];
    public GameObject[] LungeBlocks = new GameObject[5];
    public GameObject[] HitBlocks = new GameObject[5];
    public GameObject[] PenetrateBlocks = new GameObject[5];
    //開始點
    public Transform Board;
    public Transform MapStartPoint;



    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    //遊戲開始時生成第一階段地圖
    public void SpawnMap()
    {
        for (int i = 0; i < ThisMap.Length; i++)
        {
            for (int j = 0; j < ThisMap[i].ThisRow.Length; j++)
            {
                ThisMap[i].ThisRow[j].SetRandomMapBlock();
            }
        }
    }
    public void SpawnMapObject(MapBlockRow[] TM)
    {
        for (int i = 0; i < TM.Length; i++)
        {
            for (int j = 0; j < TM[i].ThisRow.Length; j++)
            {
                switch(TM[i].ThisRow[j].ThisBlockType)
                {
                    case WeaponEnum.Armor:
                        GenBlock(ArmorBlocks, (int)TM[i].ThisRow[j].ThisBlockLevel, i, j);
                        break;
                    case WeaponEnum.Slash:
                        GenBlock(SlashBlocks, (int)TM[i].ThisRow[j].ThisBlockLevel, i, j);
                        break;
                    case WeaponEnum.Lunge:
                        GenBlock(LungeBlocks, (int)TM[i].ThisRow[j].ThisBlockLevel, i, j);
                        break;
                    case WeaponEnum.Hit:
                        GenBlock(HitBlocks, (int)TM[i].ThisRow[j].ThisBlockLevel, i, j);
                        break;
                    case WeaponEnum.Penetrate:
                        GenBlock(PenetrateBlocks, (int)TM[i].ThisRow[j].ThisBlockLevel, i, j);
                        break;
                }
            }
        }
    }
    public void GenBlock(GameObject[] Array, int Level,int Row, int Column)
    {
        GameObject B =  Instantiate(Array[Level], new Vector3(MapStartPoint.transform.position.x + 1.2f * Column, 0, MapStartPoint.transform.position.z + 1.2f * Row), Quaternion.identity, Board);
        ThisMap[Row].ThisRow[Column].m_ThisBlockObject = B;
        B.GetComponent<BlockIdentity>().ThisRow = Row;
        B.GetComponent<BlockIdentity>().ThisColumn = Column;
    }
    public void BlockInRange(Vector2[] Rune,Vector2 Origin)
    {
        for (int i = 0; i < ThisMap.Length; i++)
        {
            for (int j = 0; j < ThisMap[i].ThisRow.Length; j++)
            {
                ThisMap[i].ThisRow[j].m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1);
            }
        }
        for (int i = 0; i < Rune.Length; i++)
        {
            if (Origin.x + Rune[i].x < 0 || Origin.y + Rune[i].y < 0 || Origin.x + Rune[i].x > 4 || Origin.y + Rune[i].y > 4)
            {
                Debug.Log("AssHole");
            }
            else
            {
                //Debug.Log(ThisMap[(int)(Origin.x + Rune[i].x)].ThisRow[(int)(Origin.y + Rune[i].y)].m_ThisBlockObject.name);
                ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
            }
        }
        
    }
}
[System.Serializable]
public class MapBlockRow
{
    public MapBlockClass[] ThisRow = new MapBlockClass[5];
}