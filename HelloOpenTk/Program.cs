namespace HelloOpenTk;

internal static class Program
{
    public static void Main(string[] args)
    {
        // This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.
        using (var game = new Game(800, 600, "LearnOpenTK"))
        {
            game.Run();
        }
    }
}

/*

https://opentk.net/index.html
https://opentk.net/learn/index.html 
https://github.com/opentk/LearnOpenTK/tree/master 
  
 */