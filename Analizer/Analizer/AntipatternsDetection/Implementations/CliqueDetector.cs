using DRSTool.Analizer.Models;
using DRSTool.CommonModels;
using DRSTool.FileHelper;

namespace DRSTool.Analizer.AntipatternsDetection.Implementations;

internal class CliqueDetector : IAntipatternDetector
{
    public const string AntipatternName = "clique";
    public void detect(AnalizerModel dataModel, ResultModel results)
    {
        model = dataModel;

        computeCicle(0);
        for (int i = 1; i < model.entityCount; i++)
            if (!partOfACicle.Contains(i) && !partOfADeadEnd.Contains(i))
            {
                Console.WriteLine($"Compute for starting node {i}");
                computeCicle(i);
            }
        saveToResults(dataModel, results);
    }

    public CliqueDetector()
    {
        partOfACicle = new SortedSet<int>();
        partOfADeadEnd = new SortedSet<int>();
        
    }

    private void computeCicle(int startingNode)
    {
        List<int> lastVisited = new List<int> (model.entityCount/10); // possible prediction: a dependency graph zone contains 10% of the existing files/classes

        int head = 0;

        lastVisited.Add(startingNode);
        int lastVisitedNeighbour = -1;
        while(true)
        {
            int newNeighbour = computeNextNeighbour(lastVisited[head], lastVisitedNeighbour+1);
            if(newNeighbour != -1) // found a neighbour
            {
                int cicleStart = lastVisited.IndexOf(newNeighbour);  //check if it is visited: if it is, get the position in the list of the occurence, if not get -1
                if (cicleStart != -1) // element already visited in the current neighbour list
                {
                    markCycle(lastVisited, cicleStart);
                    lastVisitedNeighbour = newNeighbour;
                    continue; // we do not proceed further into the cycle
                }
                lastVisited.Add(newNeighbour);
                head++;
                lastVisitedNeighbour = -1;
            }
            else // no new neighbour for the current head
            {
                lastVisitedNeighbour = lastVisited[head];
                lastVisited.RemoveAt(head);
                head--;
                if (!partOfACicle.Contains(lastVisitedNeighbour))
                    partOfADeadEnd.Add(lastVisitedNeighbour);
                if (head < 0)
                    break; // we visited all the neighbours of startingNode
            }
        }
        
    }

    public void saveResults(string filePath)
    {
        new FileHelperFactory().getFileHelper(filePath).writeContent(filePath, partOfACicle);
    }

    public void loadPreviouseResults(string filePath, AnalizerModel dataModel, ResultModel results)
    {
        partOfACicle = new SortedSet<int>(new FileHelperFactory().getFileHelper(filePath).getArrayContent<int>(filePath));
        saveToResults(dataModel, results);
    }

    private void saveToResults(AnalizerModel dataModel, ResultModel results)
    {
        foreach(int index in partOfACicle)
        {
            var entity = dataModel.Entities[index].Name;

            var value = new Dictionary<string, object>(); // no values, here only to not produce errors 

            results.add(index, AntipatternName, value);
        }
    }

    private int computeNextNeighbour(int lastNode, int nextNeighbour)
    {
        for(; nextNeighbour < model.entityCount; nextNeighbour++)
            if (model.SRelations[lastNode, nextNeighbour] is not null) //potential neighbour
                if(!partOfADeadEnd.Contains(nextNeighbour)) // the next neighbour is not part of a known dead end
                    return nextNeighbour;
        return -1;
    }


    private void markCycle(List<int> visitedList, int start)
    {
        for(; start < visitedList.Count(); start++)
            partOfACicle.Add(visitedList[start]);
    }

    private SortedSet<int> partOfACicle;
    private SortedSet<int> partOfADeadEnd;
    private AnalizerModel? model;

    
}
