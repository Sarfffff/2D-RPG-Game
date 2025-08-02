using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Data;
public class FindDataHandler  //�����ļ�IO�������������ݵ����л��ͷ����л�
{
    private string dataDirPath = "";  //·��
    private string dataFileName = ""; //�ļ���

    //�ļ�����
    private bool encryptData= false;  //Ĭ�ϲ�����
    private string codeWord = "CoderJ"; //��Կ

    public FindDataHandler(string _dataDirPath, string _dataFileName,bool _encryptData)
    {
        this.dataDirPath = _dataDirPath;
        this.dataFileName = _dataFileName;
        this.encryptData = _encryptData;
    }
    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);  //// �������·�� �ļ���ַ + �ļ���
        try
        {    //path.GetDirectoryName(fullPath);�˷������ fullPath ����ȡ��Ŀ¼����,��δ���Ŀ¼
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));    

            string dataToStore = JsonUtility.ToJson(_data,true); //// �����ݶ���ת��ΪJSON�ַ���
            
            if(encryptData) //����ⲿѡ���Ƿ����
                dataToStore = EncryptDescrypt(dataToStore); //�����ݴ��뺯�����м���
            
            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) 
            {  //ʹ��Io��д������
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
                { //ʹ����ʽ��ȡ
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadData = JsonUtility.FromJson<GameData>(dataToLoad); //����ȡ��json��ʽ������ת��ΪGamedata���͵�����

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

    //���ɾ��������ļ�
    public void Delete()
    {
        string fullpath  = Path.Combine(dataDirPath,dataFileName);
        if (File.Exists(fullpath))
            File.Delete(fullpath);
    }

    //���ݵļ��� ,�������ķ�ʽ
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
