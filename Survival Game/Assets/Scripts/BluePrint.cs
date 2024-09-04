using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrint : MonoBehaviour
{
    public string ItemName;
    public string Req1;
    public string Req2;

    public int Req1Amount;
    public int Req2Amount;

    public int numOfRequirements;

    public int numberOfNewCreatedItem;

    public BluePrint(string name, int numberOfNewCreatedItem, string Req1, string Req2, int Req1Amount, int Req2Amount, int numOfRequirements)
    {
        this.ItemName = name;
        this.numberOfNewCreatedItem = numberOfNewCreatedItem;
        this.Req1 = Req1;
        this.Req2 = Req2;
        this.Req1Amount = Req1Amount;
        this.Req2Amount = Req2Amount;
        this.numOfRequirements = numOfRequirements;
    }

    public BluePrint(string name, int numberOfNewCreatedItem, string Req1, int Req1Amount, int numOfRequirements)
    {
        this.ItemName = name;
        this.numberOfNewCreatedItem = numberOfNewCreatedItem;
        this.Req1 = Req1;
        this.Req1Amount = Req1Amount;
        this.numOfRequirements = numOfRequirements;
    }
    
}
