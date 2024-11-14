Console.WriteLine("*** Dijkstra Pathfinder Test Application ***");
Console.WriteLine("");
Console.WriteLine("");
Console.WriteLine("Generate maze, input size.");
Console.Write("Width : ");
var width = Console.ReadLine();
Console.Write("Height : ");
var height = Console.ReadLine();
Console.Write("Input 'magic number' : ");
var magicNumber = Console.ReadLine();

var maze = GenerateMaze(int.Parse(width), int.Parse(height), int.Parse(magicNumber));
Console.WriteLine("");
Console.WriteLine("");
DrawMaze(maze);
Console.WriteLine("Input start position");
Console.Write("Row : ");
var y = Console.ReadLine();
Console.Write("Column: ");
var x = Console.ReadLine();
Console.WriteLine("Input end position");
Console.Write("Row : ");
var endy = Console.ReadLine();
Console.Write("Column: ");
var endx = Console.ReadLine();
var result = TraverseMaze(maze, (int.Parse(y), int.Parse(x)), (int.Parse(endy), int.Parse(endx)));

if(result.HasReachedTheGoal)
{
    DrawMaze(maze, result.SeenLocations);
    Console.WriteLine();
    Console.WriteLine($"Shortest path was {result.NumberOfSteps} steps, {result.SeenLocations.Count} locations was visted.");
}
else
{
    DrawMaze(maze, result.SeenLocations);
    Console.WriteLine();
    Console.WriteLine($"The goal was not reached, {result.SeenLocations.Count} locations was visted.");
}

static PathFindResult TraverseMaze(string[,] maze, (int y, int x) startPosition, (int y, int x) endPosition)
{
    int numberOfSteps = 0;
    bool hasReachedTheGoal = false;
    var seen = new HashSet<(int y, int x)>();
    var pq = new PriorityQueue<(int y, int x, int numOfSteps), int>();

    pq.Enqueue((startPosition.y, startPosition.x, 0), 0);

    while (pq.Count > 0)
    {
        var next = pq.Dequeue();

        if((next.y,next.x) == endPosition)
        {
            hasReachedTheGoal = true;
            numberOfSteps = next.numOfSteps;
            break;
        }

        foreach (var (y, x) in new[] { (0, 1), (1, 0), (0, -1), (-1, 0) })
        {
            var dy = next.y + y;
            var dx = next.x + x;

            if (dy >= 0 && dy < maze.GetLength(0) && dx >= 0 && dx < maze.GetLength(1))
            {
                if (maze[dy, dx] != "#" && !seen.Contains((dy, dx)))
                {
                    seen.Add((dy, dx));
                    pq.Enqueue((dy, dx, next.numOfSteps + 1), next.numOfSteps + 1);
                } 
            }
        }
    }

    return new PathFindResult(numberOfSteps, seen, hasReachedTheGoal);
}


static void DrawMaze(string[,] mazeArr, HashSet<(int y, int x)> seenLocations = null)
{
    Console.Write("   ");
    for (int i = 0; i < mazeArr.GetLength(1); i++)
    {
        if (i < 10)
        {
            Console.Write($"{i} "); 
        }
        else
        {
            Console.Write($"{i}");
        }

    }
    Console.WriteLine();
    for (int y = 0; y < mazeArr.GetLength(0); y++)
    {
        for (int x = 0; x < mazeArr.GetLength(1); x++)
        {
            if (x == 0)
            {
                if (y < 10)
                {
                    if (seenLocations != null && seenLocations.Contains((y, x)))
                    {
                        Console.Write($"{y}  0");
                    }
                    else
                    {
                        Console.Write($"{y}  {mazeArr[y, x]}");
                    } 
                }
                else
                {
                    if (seenLocations != null && seenLocations.Contains((y, x)))
                    {
                        Console.Write($"{y} 0");
                    }
                    else
                    {
                        Console.Write($"{y} {mazeArr[y, x]}");
                    }
                }
            }
            else 
            { 
                if (seenLocations != null && seenLocations.Contains((y, x)))
                {
                    Console.Write($" 0");
                }
                else
                {
                    Console.Write($" {mazeArr[y, x]}");
                } 
            }
        }
        Console.WriteLine();
    }
}

static string[,] GenerateMaze(int width, int height, int magicNumber)
{
    var mazeArr = new string[height,width];

    for (int y = 0;y < height;y++)
    {
        for (int x = 0;x < width;x++)
        {
            mazeArr[y,x] = IsOpen(y,x,magicNumber) ? "." : "#";
        }
    }

    return mazeArr;
}

static bool IsOpen(int y, int x, int magicNumber)
{
    int num = (x * x) + (3 * x) + (2 * x * y) + y + (y * y) + magicNumber;
    var binary = Convert.ToString(num, 2);

    var oneCount = binary.Count(x => x == '1');
    var isEven = oneCount % 2 == 0;
    if (isEven)
        return true;

    return false;
}

record PathFindResult(int NumberOfSteps, HashSet<(int y, int x)> SeenLocations, bool HasReachedTheGoal);
