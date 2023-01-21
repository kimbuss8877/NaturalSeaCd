using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;

[System.Serializable]
public class _GAME_DATA
{
    public int[] GameInfo_Data;
}

public class GameInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool GameInfo_Exits()
    {
        string filepath = "C:\\AfricanElephantCD\\GameInfo.dat";
        bool retValue = false;
        FileInfo fi = new FileInfo(filepath);        

        if (fi.Exists)
        {
            retValue = true;
        }
        else
        {
            GameInfo_Make();
            MainObj.instance.GameInfo_Arr[3] = 100;
            GameInfo_Write(MainObj.instance.GameInfo_Arr);
            retValue = true;
        }
        return retValue;
    }
    _GAME_DATA saa = new _GAME_DATA();
    public void GameInfo_Read_Process()
    {
        saa.GameInfo_Data = new int[18];
        for (int i = 0; i < 18; i++)
            saa.GameInfo_Data[i] = 0;
        saa = GameInfo_Read();                                        // 파일 읽기
     
        for (int i = 0; i < saa.GameInfo_Data.Length; i++)
            MainObj.instance.GameInfo_Arr[i] = saa.GameInfo_Data[i];
    }
    public _GAME_DATA GameInfo_Read()
    {
        string filepath = "C:\\AfricanElephantCD\\GameInfo.dat";
        _GAME_DATA LoadData = new _GAME_DATA();
        try
        {
            if (!File.Exists(filepath))
            {
                return LoadData;
            }
            string data = null;
            FileStream fs = new FileStream(filepath, FileMode.Open);

            BinaryReader w = new BinaryReader(fs);

            data = w.ReadString();

            if (!string.IsNullOrEmpty(data))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data));

                // 가져온 데이터를 바이트 배열로 변환하고
                // 사용하기 위해 다시 리스트로 캐스팅해줍니다.

                try
                {
                    LoadData = (_GAME_DATA)binaryFormatter.Deserialize(memoryStream);
                }
                catch
                {
                    LoadData = new _GAME_DATA();
                    w.Close();
                    fs.Close();
                    return LoadData;
                }
                w.Close();
                fs.Close();
            }
        }
        catch (UnityException e)
        {
            Debug.Log(e.Message);
        }
        return LoadData;
    }

    public void GameInfo_Write(int[] val)
    {
        string filepath = "C:\\AfricanElephantCD\\GameInfo.dat";
        saa.GameInfo_Data = new int[18];
        for (int i = 0; i < 18; i++)
            saa.GameInfo_Data[i] = MainObj.instance.GameInfo_Arr[i];
        Save_Data_Files(saa, filepath);
    }
    public void Save_Data_Files(_GAME_DATA _Data, string FileName)
    {
        var binaryFormatter = new BinaryFormatter();
        var memoryStream = new MemoryStream();

        binaryFormatter.Serialize(memoryStream, _Data);

        using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
        {
            using (BinaryWriter w = new BinaryWriter(fs))
            {
                w.Write(Convert.ToBase64String(memoryStream.GetBuffer()));

                w.Flush();
                w.Close();

                fs.Close();
            }
        }
    }


    public void GameInfo_Make()
    {
     //   FileStream stream = File.Create(CasherFilePath);
      //  stream.Close();
    }

 //   public int[] TestGameInfo = new int[18];
    public void TestGameInfo_Read()
    {
        string filepath = "C:\\AfricanElephantCD\\GameInfo1.dat";
        FileStream br = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        BinaryReader valRead = new BinaryReader(br);

        //for (int i = 0; i < TestGameInfo.Length; i++)
        //    TestGameInfo[i] = valRead.ReadInt32();

        valRead.Close();
        br.Close();


    }
}
