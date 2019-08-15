using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{

    public Dictionary<int, AudioClip> Voice_lib;
    public Dictionary<string, Animation> Ani_lib ;
    public string name;

    public Character()
    {
        Voice_lib = new Dictionary<int, AudioClip>();
        Ani_lib = new Dictionary<string, Animation>();
        name = "";
    }

}
