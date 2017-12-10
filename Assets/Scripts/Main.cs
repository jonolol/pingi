using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{ 
    GameObject player;
    Map map;

    // Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        map = MapManager.CreateRandomMap(50, 50, 100);

        //map = new Map(10, 10);

        //map.AddCorridor(0, 0);
        //map.AddCorridor(0, 1);
        //map.AddCorridor(1, 0);
        //map.AddCorridor(2, 0);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
