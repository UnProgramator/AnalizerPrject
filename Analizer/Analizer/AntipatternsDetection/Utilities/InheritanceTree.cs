using DRSTool.CommonModels;
using System.Collections.Generic;

namespace DRSTool.Analizer.AntipatternsDetection.Utilities;

class InheritanceTree
{
    private IDictionary<int, Node>? data;

    public bool isInitialized() { return data != null; }

    public int count() { return data != null ? data.Count() : 0; }

    public bool inherits(int _derived, int _base)
    {
        if (data == null)
            throw new Exception("Inheritance Tree used but not initialized!");

        if(!data.ContainsKey(_derived) || !data.ContainsKey(_base)) //derived or base not in a hierarchy
            return false;

        var children = data[_base].childrens;

        if (children == null)
            return false;

        if (children.Contains(_derived))
            return true;

        return false;
    }

    public ISet<int> getChildren(int baseIdx)
    {
        if (data == null)
            throw new Exception("Inheritance Tree used but not initialized!");

        var childrens = data[baseIdx].childrens;

        return childrens != null ? childrens : new SortedSet<int>();
    }

    public void init(AnalizerModel source)
    {
        if (data != null)
            throw new Exception("Reinitialization atempt");
        data = new Dictionary<int, Node>();

        for(int d=0; d<= source.Entities.Length; d++)
            for(int b=0; b< source.Entities.Length; b++)
                if( source.SRelations[d, b] != null &&
                    source.SRelations[d, b].Properties.ContainsKey("inheritance"))
                {
                    if (!data.ContainsKey(d))
                        data.Add(d, new Node(b));
                    data[d].addParent(b);

                    if(!data.ContainsKey(b))
                        data.Add(b, new Node(null));
                    data[b].addChildren(d); 
                }

        foreach((var key, var value) in data){
            if (value.parents == null) // the update is from the root of the tree to the leaves.
                update(key);
        }
    }

    private void update(int index)
    {
        if (data == null)
            return;

        Node crt = data[index];

        if (crt.isMerged)
            return;

        if (crt.childrens == null)
            return;

        var initSet = new SortedSet<int>(crt.childrens);

        foreach (int chldIdx in initSet)
        {
            update(chldIdx);
            var children = data[chldIdx].childrens;
            if (children != null)
                crt.childrens.UnionWith(children);
        }
              

        crt.isMerged = true;
    }
}

internal class Node
{
    //public int index; this is the dictionary key
    public SortedSet<int>? parents = null;  // parent class and interfaces; direct inheritance only
    public SortedSet<int>? childrens = null; // derived classes/interfaces; both direct and indirect inheritance

    public bool isMerged = false;

    public Node(int? parent)
    {
        if (parent != null) {
            parents = new SortedSet<int>();
            parents.Add((int)parent);
        }
    }

    public void addChildren(int childrenIdx)
    {
        if(childrens == null)
            childrens = new SortedSet<int>();
        childrens.Add(childrenIdx);
    }

    public void addParent(int parentIdx)
    {
        if (childrens == null)
            childrens = new SortedSet<int>();
        childrens.Add(parentIdx);
    }
}
