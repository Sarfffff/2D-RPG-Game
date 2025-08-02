using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Data;
public class FindDataHandler  //负责文件IO操作，处理数据的序列化和反序列化
{
    private string dataDirPath = "";  //路径
    private string dataFileName = ""; //文件名

    //文件加密
    private bool encryptData= false;  //默认不加密
    private string codeWord = "CoderJ"; //密钥

    public FindDataHandler(string _dataDirPath, string _dataFileName,bool _encryptData)
    {
        this.dataDirPath = _dataDirPath;
        this.dataFileName = _dataFileName;
        this.encryptData = _encryptData;
    }
    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);  //// 组合完整路径 文件地址 + 文件名
        try
        {    //path.GetDirectoryName(fullPath);此方法会从 fullPath 里提取出目录部分,如何创建目录
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));    

            string dataToStore = JsonUtility.ToJson(_data,true); //// 将数据对象转换为JSON字符串
            
            if(encryptData) //提高外部选择是否加密
                dataToStore = EncryptDescrypt(dataToStore); //将数据传入函数进行加密
            
            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) 
            {  //使用Io流写入数据
                using (StreamWriter writer = new StreamWriter(stream)) { 
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error on trying to save data to file"  + fullPath + "\n"+e);
            
        }
    }

    public GameData Load() { 
    
        string fullPath = Path.Combine(dataDirPath,dataFileName);
        GameData loadData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                { //使用流式读取
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadData = JsonUtility.FromJson<GameData>(dataToLoad); //将读取的json格式的数据转换为Gamedata类型的数据

                if(encryptData)
                    dataToLoad = EncryptDescrypt(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error on trying on load data form file " + fullPath+ "\n"+e);
            }
            
        }
        return loadData;
    }

    //快捷删除保存的文件
    public void Delete()
    {
        string fullpath  = Path.Combine(dataDirPath,dataFileName);
        if (File.Exists(fullpath))
            File.Delete(fullpath);
    }

    //数据的加密 ,采用异或的方式
    private string EncryptDescrypt(string _data)
    {
        string modifiedData = "";
        for(int i  =  0; i < _data.Length; i++)
        {
            modifiedData += (char)_data[i]^codeWord[i % codeWord.Length];       
        }
        return modifiedData;
    }
}
