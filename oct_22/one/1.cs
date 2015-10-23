using System;
using System.Linq;

public static class Test
{
    public static void Times(this int n, Action action)
    {
        while (--n >= 0)
        {
            action();
        }
    }

    public static int? ToInt(this String s)
    {
        int result;
        return int.TryParse(s, out result) ? (int?)result : null;
    }

    static int[] ReadArray()
    {
        var numberOfAttractions = Console.ReadLine().ToInt();
        if (numberOfAttractions == null)
        {
            throw new InvalidOperationException("Invalid number of attractions");
        }

        var distances = Console.
                            ReadLine().
                            Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries).
                            Select(x => x.ToInt()).
                            Where(y => y.HasValue).
                            Select(z => z.Value).
                            ToArray();

        if (distances.Length != numberOfAttractions.Value)
        {
            throw new InvalidOperationException("Number of attractions differ from declaration");
        }

        return distances;
    }

    static int[,]  MinDistances(this int[] distances)
    {
        var reachAround = distances.Last();
        var n = distances.Length;
        var result = new int[n, n];
        var prefixSums = new int[n];
        prefixSums[0] = 0;
        for (var i = 1; i < n; i++)
        {
            prefixSums[i] = prefixSums[i - 1] + distances[i - 1];
        }

        for (var i = 0; i < n; i++)
        {
            for (var j = i + 1; j < n; j++)
            {
                result[i, j] = result[j, i] = Math.Min(prefixSums[j] - prefixSums[i],
                                                       prefixSums[i] + prefixSums.Last() - prefixSums[j] + reachAround);
            }
        }

        return result;
    }

    public static void Main()
    {
        var attractionDistances = ReadArray();
        var numberOfQueries = Console.ReadLine().ToInt();
        if (numberOfQueries == null)
        {
            throw new InvalidOperationException("Invalid number of queries");
        }

        var minDistances = MinDistances(attractionDistances);

        numberOfQueries.Value.Times(() => {
            var queryAttractions = ReadArray();
            Console.WriteLine(
                        queryAttractions.
                            Zip(
                              queryAttractions.Skip(1), 
                              (a, b) => new { a, b }).
                            Aggregate(0, (acc, x) => {
                                return acc + minDistances[x.a, x.b];
                            }));
        });
    }
}

