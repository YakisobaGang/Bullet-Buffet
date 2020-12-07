using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YakisobaGang.Scripts.Data;

public class Inventory
{
  private readonly List<GameObject> _itemsList;
  private int _capacity;

  public List<GameObject> ItemsList => _itemsList;
  public bool IsFull { get; private set; }
  public int Capacity { get; private set; }
  

  public Inventory(int capacity)
  {
    _itemsList = new List<GameObject>(capacity);
    Capacity = capacity;
  }

  public void AddItem(GameObject item)
  {
    if (_itemsList.Count == Capacity)
    {
      IsFull = true;
      return;
    }
    
    _itemsList.Add(item);
  }
}
