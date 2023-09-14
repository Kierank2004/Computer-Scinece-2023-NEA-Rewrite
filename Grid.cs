using System.Security.Cryptography;

struct Grid {
    public int gridWidth { get; set; } 
    public int gridHeight { get; set; }
    public int gridPopulation {get; set;}
    Cell cell = new Cell("", 0, "", false, 0, 0, 0, 0);
    public Grid(int gridWidth, int gridHeight, int gridPopulation){
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        this.gridPopulation = gridPopulation;
    }
    public void drawGrid(){
        System.Console.WriteLine("Drawing grid");
        for (int i = 0; i < gridHeight; i++){
            for (int j = 0; j < gridWidth; j++){
                Console.Write("X");
            }
            Console.WriteLine();
        }
    }
}