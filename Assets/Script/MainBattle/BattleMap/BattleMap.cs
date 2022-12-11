using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleMap : MonoBehaviour
{
    [SerializeField]private GameController GM;
    private MapBlockClass False = new MapBlockClass(WeaponEnum.None,0,0,0);

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
    public int StartAmmo;
    #region 破壞特效
    [SerializeField] GameObject DestroyBlockPrefab;
    [SerializeField] GameObject SpawnBlockPrefab;
    #endregion
    #region 一回合一次
    public bool CanActivateWeapon14 = true;
    public bool CanActivateWeapon12 = true;
    #endregion


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
                        GenBlock(ArmorBlocks, (int)TM[i].ThisRow[j].ThisBlockLevel, i, j,0,0,0,0, WeaponEnum.Armor);
                        break;
                    case WeaponEnum.Slash:
                        GenBlock(SlashBlocks, (int)TM[i].ThisRow[j].ThisBlockLevel, i, j, 0,0, 0, 0, WeaponEnum.Slash);
                        break;
                    case WeaponEnum.Lunge:
                        GenBlock(LungeBlocks, (int)TM[i].ThisRow[j].ThisBlockLevel, i, j, 0,0, 0, 0, WeaponEnum.Lunge);
                        break;
                    case WeaponEnum.Hit:
                        GenBlock(HitBlocks, (int)TM[i].ThisRow[j].ThisBlockLevel, i, j, 0,0, 0, 0, WeaponEnum.Hit);
                        break;
                    case WeaponEnum.Penetrate:
                        GenBlock(PenetrateBlocks, (int)TM[i].ThisRow[j].ThisBlockLevel, i, j, 0,0, 0, 0, WeaponEnum.Penetrate);
                        break;
                }
            }
        }
    }
    public void SpawnSingleMapObject(WeaponEnum This_B_Type,int Level, int Row, int Column, int Ammo,int Shield, int BuffRound , float BuffAmount )
    {       
        switch (This_B_Type)
        {
            case WeaponEnum.Armor:
                GenBlock(ArmorBlocks, Level, Row, Column, Ammo, Shield, BuffRound, BuffAmount, WeaponEnum.Armor);
                break;
            case WeaponEnum.Slash:
                GenBlock(SlashBlocks, Level, Row, Column, Ammo, Shield, BuffRound, BuffAmount, WeaponEnum.Slash);
                break;
            case WeaponEnum.Lunge:
                GenBlock(LungeBlocks, Level, Row, Column, Ammo, Shield, BuffRound, BuffAmount, WeaponEnum.Lunge);
                break;
            case WeaponEnum.Hit:                
                GenBlock(HitBlocks, Level, Row, Column, Ammo, Shield, BuffRound, BuffAmount, WeaponEnum.Hit);
                break;
            case WeaponEnum.Penetrate:
                GenBlock(PenetrateBlocks, Level, Row, Column, Ammo, Shield, BuffRound, BuffAmount, WeaponEnum.Penetrate);
                break;
        }
    }
    
    public void GenBlock(GameObject[] Array, int Level,int Row, int Column,int Ammo,int Shield,int BuffRound,float BuffAmount,WeaponEnum ThisType)
    {
        ThisMap[Row].ThisRow[Column].ThisBlockBuff = new List<BlockBuff>();
        ///
        bool[] Checker = new bool[GM.W_Data.WeaponDataList.Count];        
        for (int i = 0; i < GM.m_MainPlayer.BringingWeaponID.Length; i++)
        {
            if (GM.m_MainPlayer.BringingWeaponID[i]!=999)
            {
                Checker[GM.m_MainPlayer.BringingWeaponID[i]] = true;
            }            
        }
        if (Checker[12]&&Level>0&&ThisType == WeaponEnum.Hit && CanActivateWeapon12)
        {        
            int r = UnityEngine.Random.Range(0, 100);
            if (r < 15)
            {
                Level = Mathf.Clamp(Level + 1, 0, 5);
                CanActivateWeapon12 = false;
            }
        }
        if (Checker[14]&&Level>0&&ThisType == WeaponEnum.Armor&&CanActivateWeapon14)
        {

            if (GM.WeaponSkillActivation[14])
            {
                Level = Mathf.Clamp(Level + 1, 0, 5);
                GM.WeaponSkillActivation[14] = false;
                CanActivateWeapon14 = false;
            }
            else
            {
                int r = UnityEngine.Random.Range(0, 100);               
                if (r <15)
                {
                    Level = Mathf.Clamp(Level + 1, 0, 5);
                    GM.WeaponSkillActivation[14] = false;
                    CanActivateWeapon14 = false;
                }
            }
        }
        ///
        GameObject B =  Instantiate(Array[Level], new Vector3(MapStartPoint.transform.position.x + 1.2f * Column, 0, MapStartPoint.transform.position.z + 1.2f * Row), Quaternion.identity, Board);
        ThisMap[Row].ThisRow[Column].m_ThisBlockObject = B;
        ThisMap[Row].ThisRow[Column].ThisBlockLevel = Level;        
        ThisMap[Row].ThisRow[Column].AmmoLeft = Ammo;       
        if (GM.m_MainPlayer.ThisRound_MainCharacter_ID == 1 && Level > 0&&ThisType == WeaponEnum.Hit)
        {
            ThisMap[Row].ThisRow[Column].ShieldLeft = Shield+1;
        }
        else
        {
            ThisMap[Row].ThisRow[Column].ShieldLeft = Shield;
        }        
        ThisMap[Row].ThisRow[Column].ThisBlockBuff.Add(new BlockBuff(BuffRound, BuffAmount));
        B.GetComponent<BlockIdentity>().ThisRow = Row;
        B.GetComponent<BlockIdentity>().ThisColumn = Column;
        
        ///
        if (Checker[10])
        {
            switch (GM.W_Data.WeaponDataList[10].Weapon_BreakLevel+1)
            {
                case 1:
                    if (Level>=4)
                    {
                        ThisMap[Row].ThisRow[Column].ShieldLeft += 1;
                    }
                    break;
                case 2:
                    if (Level >= 4)
                    {
                        ThisMap[Row].ThisRow[Column].ShieldLeft += 1;
                    }
                    break;
                case 3:
                    if (Level >= 3)
                    {
                        ThisMap[Row].ThisRow[Column].ShieldLeft += 1;
                    }
                    break;
                case 4:
                    if (Level >= 3)
                    {
                        ThisMap[Row].ThisRow[Column].ShieldLeft += 1;
                    }
                    break;
                case 5:
                    if (Level >= 2)
                    {
                        ThisMap[Row].ThisRow[Column].ShieldLeft += 1;
                    }
                    break;
            }

            
        }
        ///
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
    //public void MixBlock2(Vector2[] Rune, Vector2 Origin)
    //{
    //    int[] BlockType = new int[5];        
    //    WeaponEnum MostType = WeaponEnum.None ;
    //    int HighestWeaponLevel = 0;
    //    for (int i = 0; i < Rune.Length; i++)//確認條件
    //    {
    //        if (Origin.x + Rune[i].x < 0 || Origin.y + Rune[i].y < 0 || Origin.x + Rune[i].x > 4 || Origin.y + Rune[i].y > 4)
    //        {
    //            //Debug.Log("AssHole");
    //        }
    //        else
    //        {
    //            BlockType[(int)ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType]++;                
    //        }
    //    }
    //    for (int i = 0; i < BlockType.Length; i++)//確認滿足條件種類
    //    {
    //        if (BlockType[i] >= 3)
    //        {
    //            MostType = (WeaponEnum)i;
    //        }
    //    }
    //    if (MostType == WeaponEnum.None)
    //    {
    //        for (int i = 0; i < Rune.Length; i++)
    //        {
    //            if (Origin.x + Rune[i].x < 0 || Origin.y + Rune[i].y < 0 || Origin.x + Rune[i].x > 4 || Origin.y + Rune[i].y > 4)
    //            {
    //                //Debug.Log("AssHole");
    //            }
    //            else
    //            {
    //                if (ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockLevel < 1)
    //                {
    //                    Destroy(ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].m_ThisBlockObject);
    //                    ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].SetRandomMapBlock();
    //                    SpawnSingleMapObject(ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType, 0, (int)(Origin.y + Rune[i].y), (int)(Origin.x + Rune[i].x), StartAmmo,0);
    //                }                                        
    //            }
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < Rune.Length; i++)//確認最高等級
    //        {
    //            if (Origin.x + Rune[i].x < 0 || Origin.y + Rune[i].y < 0 || Origin.x + Rune[i].x > 4 || Origin.y + Rune[i].y > 4)
    //            {
    //                //Debug.Log("AssHole");
    //            }
    //            else
    //            {
    //                if (ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType == MostType && ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockLevel >= HighestWeaponLevel)
    //                {
    //                    HighestWeaponLevel = (int)ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockLevel;
    //                }
    //            }
    //        }
    //        for (int i = 0; i < Rune.Length; i++)
    //        {
    //            if (Origin.x + Rune[i].x < 0 || Origin.y + Rune[i].y < 0 || Origin.x + Rune[i].x > 4 || Origin.y + Rune[i].y > 4)
    //            {
    //                //Debug.Log("AssHole");
    //            }
    //            else
    //            {
    //                if (ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockLevel < 1 || ((int)(Origin.y + Rune[i].y) == (int)Origin.y && (Origin.x + Rune[i].x) == Origin.x) || ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType == MostType)
    //                {
    //                    Destroy(ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].m_ThisBlockObject);
    //                }
    //                if ((int)(Origin.y + Rune[i].y) == (int)Origin.y && (Origin.x + Rune[i].x) == Origin.x)
    //                {
    //                    ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockLevel = Mathf.Clamp(HighestWeaponLevel + 1, 0, 5);
    //                    ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType = MostType;
    //                    SpawnSingleMapObject(MostType, Mathf.Clamp(HighestWeaponLevel + 1, 0, 5), (int)(Origin.y + Rune[i].y), (int)(Origin.x + Rune[i].x), StartAmmo);
    //                }
    //                else
    //                {
    //                    if (ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockLevel < 1 || ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType == MostType)
    //                    {
    //                        ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].SetRandomMapBlock();
    //                        SpawnSingleMapObject(ThisMap[(int)(Origin.y + Rune[i].y)].ThisRow[(int)(Origin.x + Rune[i].x)].ThisBlockType, 0, (int)(Origin.y + Rune[i].y), (int)(Origin.x + Rune[i].x), StartAmmo);
    //                    }
    //                }

    //            }
    //        }
    //    }

    //    Debug.Log(MostType.ToString());
    //}
    public bool MixTwoBlock(Vector2 FirstBlock, Vector2 SecondBlock, GameController GM)
    {
        int ThisBlockLevel = 0;
        if (FindBlock(FirstBlock).ThisBlockLevel == FindBlock(SecondBlock).ThisBlockLevel && FindBlock(FirstBlock).ThisBlockType == FindBlock(SecondBlock).ThisBlockType)
        {
            //主戰者被動
            switch(GM.m_MainPlayer.ThisRound_MainCharacter_ID)
            {
                case 2:
                    if (FindBlock(FirstBlock).ThisBlockLevel == 4 || FindBlock(FirstBlock).ThisBlockLevel == 5)
                    {
                        GM.TurnPoint++;
                    }
                    break;
            }            
            //確認等級  
            ThisBlockLevel = (int)FindBlock(FirstBlock).ThisBlockLevel;
            //破壞方塊       
            Destroy(FindBlock(FirstBlock).m_ThisBlockObject);
            Destroy(FindBlock(SecondBlock).m_ThisBlockObject);
            //修正等級       
                //FindBlock(SecondBlock).ThisBlockLevel = Mathf.Clamp(ThisBlockLevel + 1, 0, 6);
            SpawnSingleMapObject(FindBlock(SecondBlock).ThisBlockType, Mathf.Clamp(ThisBlockLevel + 1, 0, 6), (int)SecondBlock.y, (int)SecondBlock.x, StartAmmo,0, 0, 0);
            //生成雜件
            ThisMap[(int)FirstBlock.y].ThisRow[(int)FirstBlock.x].SetRandomMapBlock();
            SpawnSingleMapObject(ThisMap[(int)FirstBlock.y].ThisRow[(int)FirstBlock.x].ThisBlockType, 0, (int)FirstBlock.y, (int)FirstBlock.x,0,0, 0, 0);
            return true;
        }
        else
        {
            MapBlockClass temp1; 
            if ((Mathf.Abs(FirstBlock.x - SecondBlock.x)==1&&FirstBlock.y == SecondBlock.y)|| (Mathf.Abs(FirstBlock.y - SecondBlock.y) == 1 && FirstBlock.x == SecondBlock.x))
            {
                temp1 = FindBlock(FirstBlock);
                int tempLev = (int)FindBlock(SecondBlock).ThisBlockLevel; WeaponEnum tempType = FindBlock(SecondBlock).ThisBlockType; int tempAmmo = FindBlock(SecondBlock).AmmoLeft;
                int tempShield = FindBlock(SecondBlock).ShieldLeft;List<BlockBuff> tempBuff = new List<BlockBuff>();
                foreach (var item in FindBlock(SecondBlock).ThisBlockBuff)
                {
                    tempBuff.Add(item);
                }
                if (tempLev > 0)
                {
                    tempAmmo = StartAmmo;
                }
                if (temp1.ThisBlockLevel>0)
                {
                    temp1.AmmoLeft = StartAmmo;
                }

                Destroy(FindBlock(SecondBlock).m_ThisBlockObject);
                //FindBlock(SecondBlock).ThisBlockLevel = temp1.ThisBlockLevel;
                FindBlock(SecondBlock).ThisBlockType = temp1.ThisBlockType;
                SpawnSingleMapObject(temp1.ThisBlockType, (int)temp1.ThisBlockLevel, (int)SecondBlock.y, (int)SecondBlock.x, temp1.AmmoLeft,0, 0, 0);
                FindBlock(SecondBlock).ShieldLeft = temp1.ShieldLeft;
                FindBlock(SecondBlock).ThisBlockBuff = temp1.ThisBlockBuff;

                Destroy(FindBlock(FirstBlock).m_ThisBlockObject);
                //FindBlock(FirstBlock).ThisBlockLevel = tempLev;
                FindBlock(FirstBlock).ThisBlockType = tempType;
                SpawnSingleMapObject(tempType, tempLev, (int)FirstBlock.y, (int)FirstBlock.x, tempAmmo,0, 0, 0);
                FindBlock(FirstBlock).ShieldLeft = tempShield;
                FindBlock(FirstBlock).ThisBlockBuff = tempBuff;
                return true;
            }
            RefreshMap();
            return false;
        }
    }
    public void DestroyAndRefreshSingleBlock(Vector2 pos)
    {
        StartCoroutine("DestroyAndRefreshSingleBlockIEnumerator", pos);
    }
    public IEnumerator DestroyAndRefreshSingleBlockIEnumerator(Vector2 pos)
    {       
        FindBlock(pos).m_ThisBlockObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(GM.M_BossController.AttackUsedTime);
        Destroy(FindBlock(pos).m_ThisBlockObject);
        GameObject B = Instantiate(DestroyBlockPrefab, new Vector3(MapStartPoint.transform.position.x + 1.2f * pos.x, 0.6f, MapStartPoint.transform.position.z + 1.2f * pos.y), Quaternion.identity);
        GM.M_BossController.DestroyWarning();
        yield return new WaitForSeconds(1f);
        ThisMap[(int)pos.y].ThisRow[(int)pos.x].SetRandomMapBlock();
        SpawnSingleMapObject(ThisMap[(int)pos.y].ThisRow[(int)pos.x].ThisBlockType, 0, (int)pos.y, (int)pos.x,0,0, 0, 0);
        GameObject A = Instantiate(SpawnBlockPrefab, new Vector3(MapStartPoint.transform.position.x + 1.2f * pos.x, 0.6f, MapStartPoint.transform.position.z + 1.2f * pos.y), Quaternion.identity);
        Destroy(B);
    }
    public MapBlockClass FindBlock(Vector2 Position)
    {
        if (Position.x < 5 && Position.y < 5 && Position.x > -1 && Position.y > -1)
        {
            return ThisMap[(int)Position.y].ThisRow[(int)Position.x];
        }
        else return False;
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
    public int TurnPointGain(int BasicTurnPoint,GameController Controller)
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
        if (GM.m_MainPlayer.ThisRound_MainCharacter_ID == 2)
        {
            
            bool CanAddTurn = true;
            for (int i = 0; i < Controller.GameMap.ThisMap.Length; i++)
            {
                for (int j = 0; j < Controller.GameMap.ThisMap[i].ThisRow.Length; j++)
                {

                    if (Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel == 4 || Controller.GameMap.ThisMap[i].ThisRow[j].ThisBlockLevel == 5)
                    {
                        CanAddTurn = false;
                    }
                }
            }
            if (CanAddTurn)
            {
                BasicTurnPoint++;
            }
        }
        return BasicTurnPoint + Lv5turrent;
    }
    //public void TextTest(MapBlockRow[] TM)
    //{
    //    texttest.text = "";
    //    for (int i = 4; i >-1; i--)
    //    {
    //        texttest.text +=  "\r\n";
    //        for (int j = 0; j < TM[i].ThisRow.Length; j++)
    //        {
    //            texttest.text += ThisMap[i].ThisRow[j].ThisBlockLevel + " ";
    //        }
    //    }
    //}
}
[System.Serializable]
public class MapBlockRow
{
    public MapBlockClass[] ThisRow = new MapBlockClass[5];
}
[System.Serializable]

public class BlockBuff
{
    public int BuffRound;
    public float BuffAmount;
    public string BuffSpecialName;
    //強化種類
    public BlockBuff(int R,float A)
    {
        BuffRound = R;
        BuffAmount = A;
        BuffSpecialName = "";
    }
    public BlockBuff(int R, float A,string N)
    {
        BuffRound = R;
        BuffAmount = A;
        BuffSpecialName = N;
    }
}