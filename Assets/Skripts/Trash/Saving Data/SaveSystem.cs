using UnityEngine;
using System.IO; // for working with files on the operating system
using System.Runtime.Serialization.Formatters.Binary; // access binary formatter

// inspired by: Brackeys - SAVE & LOAD SYSTEM in Unity

// a static class can't be instanciated (to not accidantlly create multible versions of the saveSystem)
//public static class SaveSystem
//{
//    public static void SavePlayer (Player player)
//    {
//        BinaryFormatter formatter = new BinaryFormatter();

//        string path = Application.persistentDataPath + "/player.fun"; // where the file should be saved
//        // + "player.fun"  : create subfile. own binary file -> use any filetype you want (.fun for fun ;)
//        FileStream stream = new FileStream(path, FileMode.Create); // create new file

//        PlayerData data = new PlayerData(player); // from the PlayerData Skript

//        formatter.Serialize(stream, data); // write data to the file
//        stream.Close(); 
//    }

//    // no void since we want to load/return data
//    public static PlayerData LoadPlayer()
//    {
//        string path = Application.persistentDataPath + "/player.fun";
//        if (File.Exists(path))
//        {
//            BinaryFormatter formatter = new BinaryFormatter();
//            FileStream stream = new FileStream(path, FileMode.Open);

//            PlayerData data = formatter.Deserialize(stream) as PlayerData; // change back from binary 
//            // as PlayerData to tell what type of data we are working with
//            stream.Close();

//            return data;

//        } else
//        {
//            Debug.LogError("Save file not found in " + path);
//            return null;
//        }
//    }
//}
