using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public TextMeshProUGUI texttest;


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
    public void SpawnSingleMapObject(WeaponEnum This_B_Type,int Level, int Row, int Column)
    {
        switch (This_B_Type)
        {
            case WeaponEnum.Armor:
                GenBlock(ArmorBlocks, Level, Row, Column);
                break;
            case WeaponEnum.Slash:
                GenBlock(SlashBlocks, Level, Row, Column);
                break;
            case WeaponEnum.Lunge:
                GenBlock(LungeBlocks, Level, Row, Column);
                break;
            case WeaponEnum.Hit:
                GenBlock(HitBlocks, Level, Row, Column);
                break;
            case WeaponEnum.Penetrate:
                GenBlock(PenetrateBlocks, Level, Row, Column);
                break;
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
                //Debug.Log("AssHole");
            }
            else
            {
                //Debug.Log(ThisMap[(int)(Origin.x + Rune[i].x)].ThisRow[(int)(Origin.y + Rune[i].y)].m_ThisBlockObject.name);
                ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
            }
        }
        
    }
    public void BlockImFocus(Vector2 Origin,int FocusCount,Vector2[] BlockFocused)
    {
        for (int i = 0; i < ThisMap.Length; i++)
        {
            for (int j = 0; j < ThisMap[i].ThisRow.Length; j++)
            {
                ThisMap[i].ThisRow[j].m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1);
            }
        }
        ThisMap[(int)Origin.y].ThisRow[(int)Origin.x].m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
        for (int i = 0; i < FocusCount; i++)
        {
            ThisMap[(int)BlockFocused[i].y].ThisRow[(int)BlockFocused[i].x].m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
        }
    }
    public void FingerLifted(Vector2 Origin)
    {
        ThisMap[(int)Origin.y].ThisRow[(int)Origin.x].m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
    }
    //public void MixBlock(Vector2[] Rune, Vector2 Origin,WeaponEnum Type)
    //{
    //    float EnergyCount = 0;
    //    for (int i = 0; i < Rune.Length; i++)
    //    {
    //        if (Origin.x + Rune[i].x < 0 || Origin.y + Rune[i].y < 0 || Origin.x + Rune[i].x > 4 || Origin.y + Rune[i].y > 4)
    //        {
    //            //Debug.Log("AssHole");
    //        }
    //        else
    //        {
    //            if (ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType == Type)
    //            {
    //                EnergyCount += ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockEnergy;
    //            }
    //            else
    //            {
    //                EnergyCount += ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockEnergy*0.5f;
    //            }
    //        }
    //    }
    //    for (int i = 0; i < Rune.Length; i++)
    //    {
    //        if (Origin.x + Rune[i].x < 0 || Origin.y + Rune[i].y < 0 || Origin.x + Rune[i].x > 4 || Origin.y + Rune[i].y > 4)
    //        {
    //            //Debug.Log("AssHole");
    //        }
    //        else
    //        {
    //            Destroy(ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].m_ThisBlockObject);
    //            if ((int)(Origin.y + Rune[i].y) == (int)Origin.y && (Origin.x + Rune[i].x) == Origin.x)
    //            {
    //                ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockEnergy = EnergyCount;
    //                ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType = Type;
    //                SpawnSingleMapObject(Type, Mathf.FloorToInt(Mathf.Log(EnergyCount,2f)), (int)(Origin.y + Rune[i].y), (int)(Origin.x + Rune[i].x));
    //            }
    //            else
    //            {
    //                ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].SetRandomMapBlock();
    //                SpawnSingleMapObject(ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType, 0, (int)(Origin.y + Rune[i].y), (int)(Origin.x + Rune[i].x));
    //            }
                
    //        }
    //    }
    //    Debug.Log(EnergyCount.ToString());
    //}
    public void MixBlock2(Vector2[] Rune, Vector2 Origin)
    {
        int[] BlockType = new int[5];        
        WeaponEnum MostType = WeaponEnum.None ;
        int HighestWeaponLevel = 0;
        for (int i = 0; i < Rune.Length; i++)//確認條件
        {
            if (Origin.x + Rune[i].x < 0 || Origin.y + Rune[i].y < 0 || Origin.x + Rune[i].x > 4 || Origin.y + Rune[i].y > 4)
            {
                //Debug.Log("AssHole");
            }
            else
            {
                BlockType[(int)ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType]++;                
            }
        }
        for (int i = 0; i < BlockType.Length; i++)//確認滿足條件種類
        {
            if (BlockType[i] >= 3)
            {
                MostType = (WeaponEnum)i;
            }
        }
        if (MostType == WeaponEnum.None)
        {
            for (int i = 0; i < Rune.Length; i++)
            {
                if (Origin.x + Rune[i].x < 0 || Origin.y + Rune[i].y < 0 || Origin.x + Rune[i].x > 4 || Origin.y + Rune[i].y > 4)
                {
                    //Debug.Log("AssHole");
                }
                else
                {
                    if (ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockLevel < 1)
                    {
                        Destroy(ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].m_ThisBlockObject);
                        ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].SetRandomMapBlock();
                        SpawnSingleMapObject(ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType, 0, (int)(Origin.y + Rune[i].y), (int)(Origin.x + Rune[i].x));
                    }                                        
                }
            }
        }
        else
        {
            for (int i = 0; i < Rune.Length; i++)//確認最高等級
            {
                if (Origin.x + Rune[i].x < 0 || Origin.y + Rune[i].y < 0 || Origin.x + Rune[i].x > 4 || Origin.y + Rune[i].y > 4)
                {
                    //Debug.Log("AssHole");
                }
                else
                {
                    if (ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType == MostType && ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockLevel >= HighestWeaponLevel)
                    {
                        HighestWeaponLevel = (int)ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockLevel;
                    }
                }
            }
            for (int i = 0; i < Rune.Length; i++)
            {
                if (Origin.x + Rune[i].x < 0 || Origin.y + Rune[i].y < 0 || Origin.x + Rune[i].x > 4 || Origin.y + Rune[i].y > 4)
                {
                    //Debug.Log("AssHole");
                }
                else
                {
                    if (ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockLevel < 1 || ((int)(Origin.y + Rune[i].y) == (int)Origin.y && (Origin.x + Rune[i].x) == Origin.x) || ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType == MostType)
                    {
                        Destroy(ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].m_ThisBlockObject);
                    }
                    if ((int)(Origin.y + Rune[i].y) == (int)Origin.y && (Origin.x + Rune[i].x) == Origin.x)
                    {
                        ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockLevel = Mathf.Clamp(HighestWeaponLevel + 1, 0, 5);
                        ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType = MostType;
                        SpawnSingleMapObject(MostType, Mathf.Clamp(HighestWeaponLevel + 1, 0, 5), (int)(Origin.y + Rune[i].y), (int)(Origin.x + Rune[i].x));
                    }
                    else
                    {
                        if (ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockLevel < 1 || ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType == MostType)
                        {
                            ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].SetRandomMapBlock();
                            SpawnSingleMapObject(ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType, 0, (int)(Origin.y + Rune[i].y), (int)(Origin.x + Rune[i].x));
                        }
                    }

                }
            }
        }
        
        Debug.Log(MostType.ToString());
    }
    public bool MixTwoBlock(Vector2 FirstBlock, Vector2 SecondBlock)
    {
        int ThisBlockLevel = 0;
        if (FindBlock(FirstBlock).ThisBlockLevel == FindBlock(SecondBlock).ThisBlockLevel&& FindBlock(FirstBlock).ThisBlockType == FindBlock(SecondBlock).ThisBlockType)
        {
            //Debug.Log("SameBlock");            
            //確認等級  
            ThisBlockLevel = (int)FindBlock(FirstBlock).ThisBlockLevel;
            //破壞方塊       
            Destroy(FindBlock(FirstBlock).m_ThisBlockObject);
            Destroy(FindBlock(SecondBlock).m_ThisBlockObject);
            //修正等級       
            FindBlock(SecondBlock).ThisBlockLevel = Mathf.Clamp(ThisBlockLevel + 1, 0, 6);
            SpawnSingleMapObject(FindBlock(SecondBlock).ThisBlockType, Mathf.Clamp(ThisBlockLevel + 1, 0, 6), (int)SecondBlock.y, (int)SecondBlock.x);
            //生成雜件
            ThisMap[(int)FirstBlock.y].ThisRow[(int)FirstBlock.x].SetRandomMapBlock();
            SpawnSingleMapObject(ThisMap[(int)FirstBlock.y].ThisRow[(int)FirstBlock.x].ThisBlockType, 0, (int)FirstBlock.y, (int)FirstBlock.x);
            return true;
        }
        else
        {           
            RefreshMap();
            return false;
        }
    }
    public MapBlockClass FindBlock(Vector2 Position)
    {
        return ThisMap[(int)Position.y].ThisRow[(int)Position.x];
    }
    public void RefreshMap()
    {
        for (int i = 0; i < ThisMap.Length; i++)
        {
            for (int j = 0; j < ThisMap[i].ThisRow.Length; j++)
            {
                ThisMap[i].ThisRow[j].m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1);
            }
        }
    }
    public int TurnPointGain(int BasicTurnPoint)
    {
        int Lv5turrent = 0;
        for (int i = 0; i < ThisMap.Length; i++)
        {
            for (int j = 0; j < ThisMap[i].ThisRow.Length; j++)
            {
                if (ThisMap[i].ThisRow[j].ThisBlockLevel==6)
                {
                    Lv5turrent++;
                }                               
            }
        }
        return BasicTurnPoint + Lv5turrent;
    }
    public void TextTest(MapBlockRow[] TM)
    {
        texttest.text = "";
        for (int i = 4; i >-1; i--)
        {
            texttest.text +=  "\r\n";
            for (int j = 0; j < TM[i].ThisRow.Length; j++)
            {
                texttest.text += ThisMap[i].ThisRow[j].ThisBlockLevel + " ";
            }
        }
    }
}
[System.Serializable]
public class MapBlockRow
{
    public MapBlockClass[] ThisRow = new MapBlockClass[5];
}