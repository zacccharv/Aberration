using System;
using System.Collections.Generic;
using UnityEngine;

public class OneDemensionalObject : MonoBehaviour
{
    public string text;
    public List<char> BaseItems = new();
    public List<List<char>> Items = new();

    void Start()
    {
        Items = new();
        BaseItems.Capacity = text.Length;

        for (int j = 0; j < text.Length; j++)
        {
            BaseItems.Add(text[j]);
        }
        Items.Add(BaseItems);

        for (int i = 0; i < BaseItems.Count; i++)
        {
            Items.Add(BaseItems);
        }

        Debug.Log(PermutationAmount());
        Debug.Log(OnePermutation());
    }
    public OneDemensionalObject(List<char> items)
    {
        BaseItems = items;
        Items.Capacity = BaseItems.Count;

        for (var i = 0; i < BaseItems.Count; i++)
        {
            Items.Add(BaseItems);
        }
    }

    public int PermutationAmount()
    {
        int end = BaseItems.Count;
        int amount = 1;

        for (int i = 1; i < end + 1; i++)
        {
            amount *= i;
        }

        return amount;
    }

    public string OnePermutation()
    {
        List<List<char>> items = new();
        List<char> newBaseItem = new();

        for (var i = 0; i < Items.Count; i++)
        {
            items.Add(Items[i]);
        }

        for (int i = 0; i < items.Count; i++)
        {
            System.Random rand = new();
            int randIndex = rand.Next(0, Items.Count - i);

            char item = items[i][randIndex];

            newBaseItem.Add(item);

            for (int j = i; j < items.Count; j++)
            {
                List<char> letters = new();

                for (var k = 0; k < items[j].Count; k++)
                {
                    if (items[j][k] != item)
                    {
                        letters.Add(items[j][k]);
                    }
                }
                items[j] = letters;
            }
        }

        return string.Join("", newBaseItem);
    }
}
