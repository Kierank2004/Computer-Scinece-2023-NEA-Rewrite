using System;
using System.Collections.Generic;
class Program{
    static void Main(string[] args){
        string Start; 
        Console.WriteLine("Please enter whether you would like to Import (1) a generation or create your own (2).");
        do
        {
            Start = Console.ReadLine();
            if (Start == "1")
            {
       //         Import();
            }
            else if (Start == "2")
            {
              CreateOwnGrid();
            }
            if (Start != "1" | Start != "2")
            {
                Console.WriteLine("Please enter an integer Value either being 1 or 2");
            }
        } while (Start != "1" | Start != "2");
    }
    static void CreateOwnGrid()
    {
        int height; int width; int population;
        Console.WriteLine("Please enter the height of the grid");
        height = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Please enter the width of the grid");
        width = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Please enter the population of the grid");
        population = Convert.ToInt32(Console.ReadLine());
        Grid grid = new Grid(height, width, population);
        Cell celly = new Cell("CBAQA", 0, "S", false, 0, 0, 0, 0);
        grid.drawGrid();
    }
}
