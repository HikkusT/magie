using System;
using System.Collections.Generic;
using System.Linq;
using Magie.Elements;
using UnityEngine;

[Serializable]
public struct ElementCombination : IEquatable<ElementCombination>
{
    [SerializeField] private List<Element> elements;
    
    public List<Element> Elements => elements;

    public ElementCombination(int _)
    {
        this.elements = new();
    }

    public void Add(Element element)
    {
        elements.Add(element);
    }

    public bool Equals(ElementCombination other)
    {
        if (elements == null && other.elements == null)
            return true;
        if (elements == null || other.elements == null)
            return false;
        if (elements.Count != other.elements.Count)
            return false;

        var countsA = GetElementCounts(elements);
        var countsB = GetElementCounts(other.elements);

        return countsA.Count == countsB.Count && !countsA.Except(countsB).Any();
    }

    public override bool Equals(object obj)
    {
        return obj is ElementCombination other && Equals(other);
    }

    public override int GetHashCode()
    {
        if (elements == null || elements.Count == 0)
            return 0;
        
        int hash = 17;
        var counts = GetElementCounts(elements);

        foreach (var kvp in counts.OrderBy(kvp => kvp.Key.DebugName)) // Order ensures consistent hash
        {
            hash = hash * 31 + kvp.Key.GetHashCode();
            hash = hash * 31 + kvp.Value;
        }

        return hash;
    }

    private static Dictionary<Element, int> GetElementCounts(List<Element> list)
    {
        var dict = new Dictionary<Element, int>();
        foreach (var e in list)
        {
            if (!dict.TryAdd(e, 1))
                dict[e]++;
        }
        return dict;
    }
}