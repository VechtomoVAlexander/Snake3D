using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGeneration : MonoBehaviour
{
    public float XSize = 24.5f;
    public float ZSize = 24.5f;
    public GameObject foodPrefab;
    public GameObject curFood;
    public Vector3 curPos;
    void SirCanIGetFood()
    {
        RandomPos();
        curFood = GameObject.Instantiate(foodPrefab, curPos, Quaternion.Euler(90f, 0f, 0f)) as GameObject;
    }
    void RandomPos()
    {
        curPos = new Vector3(Random.Range(XSize * -1, XSize), 0f, Random.Range(ZSize * -1, ZSize));
    }

    void Update()
    {
        if(!curFood)
        {
            SirCanIGetFood();
        }
        else
        {
            return;
        }
    }
}
