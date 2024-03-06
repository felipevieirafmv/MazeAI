namespace MazeAI;

public class Solver
{
    public int  Option { get; set; }
    public Maze Maze   { get; set; } = null!;

    public string Algorithm
    {
        get
        {
            return (Option % 4) switch
            {
                0 => "DFS",
                1 => "BFS",
                2 => "dijkstra",
                _ => "aStar"
            };
        }
    }

    public void Solve()
    {
        var goal = Maze.Spaces.FirstOrDefault(s => s.Exit);

        if (Maze.Root is null || goal is null)
            return;

        switch (Option % 4)
        {
            case 0:
                DFS(Maze.Root, goal);
                break;
            case 1:
                BFS(Maze.Root, goal);
                break;
            case 2:
                Dijkstra(Maze.Root, goal);
                break;
            case 3:
                AStar(Maze.Root, goal);
                break;
        }
    }

    private static bool DFS(Space space, Space goal)
    {
        if(space.Visited)
            return false;

        space.Visited = true;

        if(space.X == goal.X && space.Y == goal.Y)
        {
            space.IsSolution = true;
            return true;
        }

        if((space.Top is not null && DFS(space.Top, goal))
            || (space.Right is not null && DFS(space.Right, goal))
            || (space.Bottom is not null && DFS(space.Bottom, goal))
            || (space.Left is not null && DFS(space.Left, goal)))
            {
                space.IsSolution = true;
                return true;
            }
        
        space.IsSolution = false;
        return false;
    }

    private static bool BFS(Space start, Space goal)
    {
        var prev = new Dictionary<Space, Space>();
        var queue = new Queue<Space>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var currNode = queue.Dequeue();

            if (currNode.Visited)
                continue;

            currNode.Visited = true;

            if (currNode == goal)
            {
                currNode.IsSolution = true;
                break;
            }

            if(currNode.Top is not null && !currNode.Top.Visited)
            {
                queue.Enqueue(currNode.Top);
                prev[currNode.Top] = currNode;
            }
            if(currNode.Right is not null && !currNode.Right.Visited)
            {
                queue.Enqueue(currNode.Right);
                prev[currNode.Right] = currNode;
            }
            if(currNode.Bottom is not null && !currNode.Bottom.Visited)
            {
                queue.Enqueue(currNode.Bottom);
                prev[currNode.Bottom] = currNode;
            }
            if(currNode.Left is not null && !currNode.Left.Visited)
            {
                queue.Enqueue(currNode.Left);
                prev[currNode.Left] = currNode;
            }
        }

        var attempt = goal;

        while(attempt != start)
        {
            attempt.IsSolution = true;
            if(!prev.ContainsKey(attempt))
                return false;
            
            attempt = prev[attempt];
        }
        attempt.IsSolution = true;

        return false;
    }

    private static bool Dijkstra(Space start, Space goal)
    {
        var queue = new PriorityQueue<Space, int>();
        var dist = new Dictionary<Space, int>();
        var prev = new Dictionary<Space, Space>();

        queue.Enqueue(start, 0);
        dist[start] = 0;

        while(queue.Count > 0)
        {
            var currNode = queue.Dequeue();

            if(currNode == goal)
                break;

            currNode.Visited = true;

            if(currNode.Top is not null)
            {
                int newDistance = dist[currNode] + 1;
                if(!dist.ContainsKey(currNode.Top))
                {
                    dist[currNode.Top] = int.MaxValue;
                    prev[currNode.Top] = null!;
                }

                if(newDistance < dist[currNode.Top])
                {
                    dist[currNode.Top] = newDistance;
                    prev[currNode.Top] = currNode;
                    queue.Enqueue(currNode.Top, newDistance);
                }
            }

            if(currNode.Right is not null)
            {
                int newDistance = dist[currNode] + 1;
                if(!dist.ContainsKey(currNode.Right))
                {
                    dist[currNode.Right] = int.MaxValue;
                    prev[currNode.Right] = null!;
                }

                if(newDistance < dist[currNode.Right])
                {
                    dist[currNode.Right] = newDistance;
                    prev[currNode.Right] = currNode;
                    queue.Enqueue(currNode.Right, newDistance);
                }
            }

            if(currNode.Bottom is not null)
            {
                int newDistance = dist[currNode] + 1;
                if(!dist.ContainsKey(currNode.Bottom))
                {
                    dist[currNode.Bottom] = int.MaxValue;
                    prev[currNode.Bottom] = null!;
                }

                if(newDistance < dist[currNode.Bottom])
                {
                    dist[currNode.Bottom] = newDistance;
                    prev[currNode.Bottom] = currNode;
                    queue.Enqueue(currNode.Bottom, newDistance);
                }
            }

            if(currNode.Left is not null)
            {
                int newDistance = dist[currNode] + 1;
                if(!dist.ContainsKey(currNode.Left))
                {
                    dist[currNode.Left] = int.MaxValue;
                    prev[currNode.Left] = null!;
                }

                if(newDistance < dist[currNode.Left])
                {
                    dist[currNode.Left] = newDistance;
                    prev[currNode.Left] = currNode;
                    queue.Enqueue(currNode.Left, newDistance);
                }
            }
        }

        var attempt = goal;

        while(attempt != start)
        {
            attempt.IsSolution = true;
            if(!prev.ContainsKey(attempt))
                return false;
            
            attempt = prev[attempt];
        }
        attempt.IsSolution = true;

        return true;
    }

    private static bool AStar(Space start, Space goal)
    {
        var queue = new PriorityQueue<Space, int>();
        var dist = new Dictionary<Space, int>();
        var prev = new Dictionary<Space, Space>();

        queue.Enqueue(start, 0);
        dist[start] = 0;

        while(queue.Count > 0)
        {
            var currNode = queue.Dequeue();

            if(currNode == goal)
                break;

            currNode.Visited = true;

            if(currNode.Top is not null)
            {
                var dx = currNode.Top.X - goal.X;
                var dy = currNode.Top.Y - goal.Y;
                var penalty = dx * dx + dy * dy;

                int newDistance = dist[currNode] + 1 + penalty;
                
                if(!dist.ContainsKey(currNode.Top))
                {
                    dist[currNode.Top] = int.MaxValue;
                    prev[currNode.Top] = null!;
                }

                if(newDistance < dist[currNode.Top])
                {

                    dist[currNode.Top] = newDistance;
                    prev[currNode.Top] = currNode;
                    queue.Enqueue(currNode.Top, newDistance);
                }
            }

            if(currNode.Right is not null)
            {
                var dx = currNode.Right.X - goal.X;
                var dy = currNode.Right.Y - goal.Y;
                var penalty = dx * dx + dy * dy;

                int newDistance = dist[currNode] + 1 + penalty;

                if(!dist.ContainsKey(currNode.Right))
                {
                    dist[currNode.Right] = int.MaxValue;
                    prev[currNode.Right] = null!;
                }

                if(newDistance < dist[currNode.Right])
                {
                    dist[currNode.Right] = newDistance;
                    prev[currNode.Right] = currNode;
                    queue.Enqueue(currNode.Right, newDistance);
                }
            }

            if(currNode.Bottom is not null)
            {
                var dx = currNode.Bottom.X - goal.X;
                var dy = currNode.Bottom.Y - goal.Y;
                var penalty = dx * dx + dy * dy;

                int newDistance = dist[currNode] + 1 + penalty;

                if(!dist.ContainsKey(currNode.Bottom))
                {
                    dist[currNode.Bottom] = int.MaxValue;
                    prev[currNode.Bottom] = null!;
                }

                if(newDistance < dist[currNode.Bottom])
                {
                    dist[currNode.Bottom] = newDistance;
                    prev[currNode.Bottom] = currNode;
                    queue.Enqueue(currNode.Bottom, newDistance);
                }
            }

            if(currNode.Left is not null)
            {
                var dx = currNode.Left.X - goal.X;
                var dy = currNode.Left.Y - goal.Y;
                var penalty = dx * dx + dy * dy;

                int newDistance = dist[currNode] + 1 + penalty;

                if(!dist.ContainsKey(currNode.Left))
                {
                    dist[currNode.Left] = int.MaxValue;
                    prev[currNode.Left] = null!;
                }

                if(newDistance < dist[currNode.Left])
                {
                    dist[currNode.Left] = newDistance;
                    prev[currNode.Left] = currNode;
                    queue.Enqueue(currNode.Left, newDistance);
                }
            }
        }

        var attempt = goal;

        while(attempt != start)
        {
            attempt.IsSolution = true;
            if(!prev.ContainsKey(attempt))
                return false;
            
            attempt = prev[attempt];
        }
        attempt.IsSolution = true;

        return true;
    }
}