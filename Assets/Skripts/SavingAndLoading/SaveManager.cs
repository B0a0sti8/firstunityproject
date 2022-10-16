using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    Transform PLAYER;
    PlayerStats playerStats;

    void Start()
    {
        PLAYER = transform.parent;
        playerStats = PLAYER.GetComponent<PlayerStats>();
    }

    void Update()
    {
        
    }

    public void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Create);

            SaveData data = new SaveData();

            SavePlayer(data);

            bf.Serialize(file, data);

            file.Close();


        }
        catch (System.Exception)
        {
            // Handling Errors
        }
    }

    private void SavePlayer(SaveData data)
    {
        data.MyPlayerData = new PlayerData(playerStats.MyCurrentPlayerLvl);
    }




    public void Load()
    {

        Debug.Log("Load2");
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Open);

            SaveData data = (SaveData)bf.Deserialize(file);

            file.Close();

            LoadPlayer(data);


        }
        catch (System.Exception)
        {
            // Handling Errors
        }
    }

    public void LoadPlayer(SaveData data)
    {
        playerStats.MyCurrentPlayerLvl = data.MyPlayerData.MyLevel;
        playerStats.LoadLevel();
    }

}
