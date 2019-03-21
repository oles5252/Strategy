using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataHandler
{
    // Convert an object to a byte array
    public static byte[] ObjectToByteArray(System.Object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    // Convert a byte array to an Object
    public static System.Object ByteArrayToObject(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }
    }
}

[System.Serializable]
public enum Alignment
{
    Terrain,
    Player,
    Enemy,
    Ally
}

[System.Serializable]
public class Character
{
    public Character(Alignment alignment, string name, int maxhp, int currenthp, int atk, int def, int mov, int rng)
    {
        this.name = name;
        this.alignment = alignment;
        this.maxhp = maxhp;
        this.currenthp = currenthp;
        this.atk = atk;
        this.def = def;
        this.mov = mov;
        this.rng = rng;
    }

    public Alignment alignment;
    public bool turnTaken = false;
    public string name;
    public int maxhp;
    public int currenthp;
    public int atk;
    public int def;
    public int mov;
    public int rng;

    /*public void setCharacter(string name, int hp, int atk, int def, int mov, int rng)
    {
        this.name = name;
        this.hp = hp;
        this.atk = atk;
        this.def = def;
        this.mov = mov;
        this.rng = rng;
    }*/
}

[System.Serializable]
public class SaveData{

    public string levelName;

    public List<Character> characters;    

    public SaveData()
    {
        levelName = "";
        characters = new List<Character>();
    }

    public bool Load()
    {
        FileStream file;
        if (this.Exists())
        {
            //check if player wants to overwrite save
            file = File.OpenRead(GameController.Instance.fileName);
            SaveData loaded = (SaveData)DataHandler.ByteArrayToObject(File.ReadAllBytes(GameController.Instance.fileName));
            this.levelName = loaded.levelName;
            this.characters = loaded.characters;
            file.Close();
        }
        else
        {
            //if file does not exist, save new!
            SaveNew();
        }

        return true;
    }

    public bool Save(int level)
    {
        FileStream file;
        if(this.Exists())
        {
            //check if player wants to overwrite save
            file = File.Create(GameController.Instance.fileName);
            byte[] save = DataHandler.ObjectToByteArray(this);
            file.Write(save, 0, save.Length);
            file.Close();
        }
        else
        {
            //if file does not exist, save new!
            SaveNew();
        }

        return true;
    }

    public bool SaveNew()
    {
        FileStream file;
        if (this.Exists())
        {
            //check if player wants to overwrite save

            file = File.Create(GameController.Instance.fileName);

            Character phos = new Character(Alignment.Player, "Phos", 3, 3, 3, 0, 3, 1); //human
            Character tisiphone = new Character(Alignment.Player, "Tisiphone", 3, 3, 2, 0, 3, 2); //elf
            Character sisyphus = new Character(Alignment.Enemy, "Sisyphus", 3, 3, 3, 1, 2, 2); //elf king
            Character iphi = new Character(Alignment.Player, "Iphi", 3, 3, 2, 1, 2, 1); //dwarf
            Character dartfoot = new Character(Alignment.Player, "Dartfoot", 3, 3, 2, 0, 4, 1); //halfling
            characters.Add(phos);
            characters.Add(tisiphone);
            characters.Add(sisyphus);
            characters.Add(iphi);
            characters.Add(dartfoot);
        }
        else
        {
            //check if player wants to overwrite save
            file = File.Create(GameController.Instance.fileName);
        }
        byte[] save = DataHandler.ObjectToByteArray(this);
        file.Write(save, 0, save.Length);

        file.Close();

        return true;
    }

    public bool Exists()
    {
        if (File.Exists(GameController.Instance.fileName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

