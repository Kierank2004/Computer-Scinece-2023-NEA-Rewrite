class Program{
 static void contractTracing(){
        // this is where the contract tracing will be implemented
        
    }
    static void End()
    {
        System.Console.WriteLine("Thank you for using my simulation");
        System.Console.WriteLine("Press 'K' to close the application.");
        while (true)
        {
            char keyPressed = Console.ReadKey().KeyChar;

            if (keyPressed == 'k' || keyPressed == 'K')
            {
                Console.WriteLine("Closing the application...");
                Environment.Exit(0);
            }
        }
    }
    // Gather Neighbour Functions
    static int GatherSusNeighbours(Cell[,] currentGrid, int width, int height)
    {
        int SusNeighbours = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (currentGrid[i,j].IndividualState == "S")
                {
                    SusNeighbours++;
                }
            }
        }
        return SusNeighbours;
    }
    static int GatherExposedNeighbours(Cell[,] currentGrid, int Width, int Height)
    {
        int ExposedNeighbours = 0;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (currentGrid[i, j].IndividualState == "E")
                {
                    ExposedNeighbours++;
                }
            }
        }
        return ExposedNeighbours;
    }
    static int GatherInfectNeighbours(Cell[,] currentGrid, int Width, int Height)
    {
        int InfNeighbours = 0;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (currentGrid[i, j].IndividualState == "I")
                {
                    InfNeighbours++;
                }
            }
        }
        return InfNeighbours;
    }
    static int GatherRecfectNeighbours(Cell[,] currentGrid, int Width, int Height)
    {
        int RecNeighbours = 0;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (currentGrid[i, j].IndividualState == "R")
                {
                    RecNeighbours++;
                }
            }
        }
        return RecNeighbours;
    }
    static void DrawsGrids(int Width, int Height, double Percent, int DaysSimulated, bool MakeFile, string FileName) // I am going to be repeating this alot during the project so I  have turned it into a method so I dont repeat unesscary code.
    {
        int FileWannaMake = 0;
        int[] Susceptible = new int[DaysSimulated];
        int[] Exposed = new int[DaysSimulated];
        int[] Infected = new int[DaysSimulated];
        int[] Recovered = new int[DaysSimulated];
        int generation = 1;
        Virus ActiveVirus = Virusyo(Width, Height, Percent, DaysSimulated, MakeFile, FileName);
        Random random = new Random();
        int population = Width * Height;
        Cell[,] currentGrid = new Cell[Width, Height];
        System.Console.WriteLine("Press enter to carry on with the simulation.");
        Console.WriteLine("This is the Initial generation of cells");
        Console.WriteLine("Virus{0}", ActiveVirus.Virusname);
        Console.WriteLine("-------------------------------------------");        // Creates very first instance of all of the cells and adds in starting infected cells.
        for (int row = 0; row < Width; row++)
        {
            for (int col = 0; col < Height; col++)
            {
                double randomDouble = random.NextDouble();
                double randomHyg = random.NextDouble();
                double randomVax = random.NextDouble();
                double RandMask = random.NextDouble();
                if (randomDouble < Percent)  //  infecting the matrix with non brokies.
                {
                    currentGrid[row, col] = new Exposed("Cell"+Width+Height,0, randomHyg, randomVax, RandMask, 0);
                    Console.Write(currentGrid[row, col].IndividualState);
                }
                else
                {
                    currentGrid[row, col] = new Susceptible("Cell"+Width+Height,0, randomHyg, randomVax, RandMask, 0);
                    Console.Write(currentGrid[row, col].IndividualState);
                }
            }
            Console.WriteLine("");
        }
        Console.WriteLine("Would you like to Sae the intial generation to a file?");
        string FileChoice = Console.ReadLine();
        if (FileChoice == "yes")
        {
            FileWannaMake = 1;
            FileWriter(FileWannaMake, Width, Height, currentGrid, generation, FileName, Susceptible, Exposed, Infected, Recovered);
        }
        Susceptible[generation] = GatherSusNeighbours(currentGrid, Width, Height);
        Exposed[generation] = GatherExposedNeighbours(currentGrid, Width, Height);
        Infected[generation] = GatherInfectNeighbours(currentGrid, Width, Height);
        Recovered[generation] = GatherRecfectNeighbours(currentGrid, Width, Height);
        NextGeneration(currentGrid, Width, Height, population, generation, DaysSimulated, MakeFile, FileName, ActiveVirus, Susceptible, Exposed, Infected, Recovered);
    }
    static void NextGeneration(Cell[,] currentGrid, int Width, int Height, int population, int generation, int DaysSimulated, bool MakeFile, string FileName, Virus ActiveVirus, int[]Susceptible, int[] Exposed, int[] Infected, int[] Recovered)
    {
        Cell[,] Newgrid = new Cell[Width, Height];
        int FileWannaMake;
        int i, j;
        if (generation == DaysSimulated)
        {
            FileWannaMake = 2;
            FileWriter(FileWannaMake, Width, Height, currentGrid, generation, FileName, Susceptible, Exposed, Infected, Recovered);
            End();
        }
        for (i = 0; i < Width; i++)
        {
            for (j = 0; j < Height; j++) // loops through matrix and assigns the grid to newgrid
            {
                Newgrid[i, j] = currentGrid[i, j]; // assiging grid to new grid because the rules are dependent on the state of the grid at the start of the loop
            }
        }

        for (i = 0; i < Width; i++)
        {
            for (j = 0; j < Height; j++) // loops through the matrix and applies the rules!  
            {
                RuleImplamentation(Newgrid, currentGrid, i, j, Width, Height, ActiveVirus);
            }
        }
  // The fomrula for Rnaught is R = B (Infection producing contacts per time) * Mean infectious period: newly added 
        float B = GatherInfectNeighbours(currentGrid, Width, Height) / DaysSimulated;       
        float Mean_infectious_period = ActiveVirus.RecoveryTime / 1; 
        float Rnaught = B * Mean_infectious_period;        
        Console.WriteLine("Virus Stats");
        Console.WriteLine("Virus: {0}", ActiveVirus.Virusname);
        Console.WriteLine("Virus Incubation Period: {0}", ActiveVirus.Incubationtimes);
        Console.WriteLine("Virus Infection Rate: {0}", ActiveVirus.Infectionrate);
        Console.WriteLine("Virs Rocovery Chance; {0}", ActiveVirus.RecoveryChance);
        Console.WriteLine("Virs Recovery Rate: {0}", ActiveVirus.RecoveryTime);
        Console.WriteLine("Virus Fatality Rate: {0}", ActiveVirus.Fatalityrate);
        Console.WriteLine("This is generation {0} of this simulation", generation);
        // newly added
        Console.WriteLine("The basic reproduction rate is {0}", Rnaught);
        Console.WriteLine("--------------------------------------------");
        for (int l = 0; l < Height; l++)
        {
            for (int k = 0; k < Width; k++)
            {
                Console.Write(currentGrid[l, k].IndividualState);
            }
            Console.WriteLine("");
        }

        ConsoleKey keyPressed = Console.ReadKey(true).Key;
        if (keyPressed == ConsoleKey.Enter)
        {
            Console.Clear();
            if (MakeFile == true)
            {
                FileWannaMake = 1;
                FileWriter(FileWannaMake, Width, Height, currentGrid, generation, FileName, Susceptible, Exposed, Infected, Recovered); ;
            }
            Susceptible[generation] = GatherSusNeighbours(currentGrid, Width, Height);
            Exposed[generation] = GatherExposedNeighbours(currentGrid, Width, Height);
            Infected[generation] = GatherInfectNeighbours(currentGrid, Width, Height);
            Recovered[generation] = GatherRecfectNeighbours(currentGrid, Width, Height);
            NextGeneration(currentGrid, Width, Height, population, generation + 1, DaysSimulated, MakeFile, FileName, ActiveVirus, Susceptible, Exposed, Infected, Recovered);
        }
    }
    static Virus Virusyo(int Width, int Height, double Percent, int DaysSimulated, bool MakeFile, string FileName)
    {
        string VirusName; int incubationtimes; double infectionrate; int recoveryTime; double recoveryChance = 0; double FatalityRate;
        List<Virus> viruses = new List<Virus>()
    {
        new Virus("Ebola", 2, 0.7, 21, 0.5, 0.99),
        new Virus("Influenza", 2, 0.5, 7, 0.95, 0.01),
        new Virus("Zika", 7, 0.5, 14, 0.9, 0.01),
        new Virus("HIV", 10, 0.001, 5475, 0.001, 0.8),//
        new Virus("Measles", 10, 0.9, 14, 0.98, 0.02),
        new Virus("SARS-CoV-2", 5, 1.5, 14, 0.95, 0.02),
        new Virus("Rotavirus", 2, 0.9, 7, 0.99, 0.005),//
        new Virus("Norovirus", 1, 0.8, 3, 0.9, 0.01),
        new Virus("Herpes simplex", 3, 0.2, 21, 0.99, 0.001),
        new Virus("Hepatitis A", 2, 0.5, 30, 0.95, 0.01),
        new Virus("Dengue", 5, 0.6, 14, 0.9, 0.01),
        new Virus("Chikungunya", 5, 0.4, 21, 0.9, 0.005),
        new Virus("West Nile", 3, 0.3, 14, 0.7, 0.1),
        new Virus("Yellow fever", 3, 0.5, 14, 0.9, 0.05),//
        new Virus("Rabies", 30, 0.99, 60, 0.001, 0.99),
    };
        Console.WriteLine("Please choose the virus you would like to simulate:");
        for (int i = 0; i < viruses.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {viruses[i].Virusname}");
        }
        Console.WriteLine("{0}. Custom", viruses.Count + 1);
        int Vchoice;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out Vchoice) && Vchoice >= 1 && Vchoice <= viruses.Count + 1)
            {
                if (Vchoice == 16)
                {
                    Console.WriteLine("Please Enter A Virus Name, Then Incubation Time, Then InfectionRate, Then RecoveryChance,Then recovery Time ,Then FatalityRate");
                    VirusName = Console.ReadLine();
                    while (VirusName.Length <= 4)
                    {
                        Console.WriteLine("Please enter a name longer than 4 digits.");
                        VirusName = Console.ReadLine();
                    }
                    string IncuTimes1 = Console.ReadLine(); incubationtimes = int.Parse(IncuTimes1);
                    while (incubationtimes <= 0)
                    {
                        Console.WriteLine("The input has to be a number greater than zero");
                        IncuTimes1 = Console.ReadLine(); incubationtimes = int.Parse(IncuTimes1);
                    }
                    string InfectRate = Console.ReadLine(); infectionrate = double.Parse(InfectRate);


                    while (infectionrate < 0.001)
                    {
                        Console.WriteLine("The infection rate is too small. Please make it at least 0.001.");
                        InfectRate = Console.ReadLine(); infectionrate = double.Parse(InfectRate);
                    }
                    string RecovChance = Console.ReadLine(); recoveryChance = double.Parse(RecovChance);
                    while (recoveryChance <= 0.05)
                    {
                        Console.WriteLine("The recovery rate is too small please make it at least 0.05");
                        RecovChance = Console.ReadLine(); recoveryChance = double.Parse(RecovChance);
                    }
                    string Recovotimee=Console.ReadLine(); recoveryTime = int.Parse(Recovotimee);
                    while (recoveryTime <= 1)
                    {
                        Console.WriteLine("The recovery time has to be greater than 1");
                        Recovotimee = Console.ReadLine(); recoveryTime = int.Parse(Recovotimee);
                    }
                    string Fatality = Console.ReadLine(); FatalityRate = double.Parse(Fatality);
                    while (FatalityRate < 0.001)
                    {
                        Console.WriteLine("The fatality rate is too small please make it at least 0.001");
                        Fatality = Console.ReadLine(); FatalityRate = double.Parse(Fatality);
                    }
                    Virus Custom = new Virus(VirusName, incubationtimes, infectionrate, recoveryTime, recoveryChance, FatalityRate);
                    viruses.Add(Custom);
                }
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please enter a valid number. Please enter the Virus number");
            }
        }
        Virus activeVirus = viruses[Vchoice - 1];
        return activeVirus;
    }
    static void FileWriter(int FileWannaMake, int Width, int Height, Cell[,] currentGrid, int generation, string FileName, int[] Susceptible, int[] Exposed, int[] Infected, int[] Recovered)
    {
        int genny = 0;
        generation = genny;
        switch (FileWannaMake)
        {
            case 1:
                using (StreamWriter writer = new StreamWriter(string.Format("{0}{1}.txt", FileName, generation)))
                {
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            writer.Write(currentGrid[i, j].IndividualState);
                        }
                        writer.WriteLine();
                    }
                }
                break;
            case 2: // creates cvs file to display the data on a graph
                string csvFilePath = "${FileName}simulation_data.csv";
                using (StreamWriter writer = new StreamWriter(csvFilePath))
                {
                    // Write the column headers
                    writer.WriteLine("Generation, Suscetible Cells, Exposed Cells, Infected Cells, Recovered.");


                    // Write the data rows
                    for (int i = 0; i < Susceptible.Length; i++)
                    {


                       writer.WriteLine($"{i}, {Susceptible[i]}, {Exposed[i]}, {Infected[i]}, {Recovered[i]}");
                    }
                }
                break;
            default:
                System.Console.WriteLine("Invalid Input try again.");
                break;
        }
    }
    static void RuleImplamentation(Cell[,] Newgrid, Cell[,] currentGrid, int i, int j, int Width, int Height, Virus ActiveVirus) // I want to implement the rules recurvsively but not sure how to :(
    {
        int x, y;
        double TransmissionRate;
        int Widdy = Width;
        int hitty = Height; // I use this to hitty the griddy >:)
        Random random = new Random();
        // Uses Moore Neighbourhood to get infected neighbours
        double randomDouble;
        int manhatdist = 1; // Allows people to change how many cells are checked.
        int ineighbour = 0;
        for (x = -manhatdist; x < manhatdist; x++)
        {
            for (y = -manhatdist; y < manhatdist; y++)
            {
                int col = (i + x + Widdy) % Widdy;
                int row = (j + y + hitty) % hitty;
                if (currentGrid[col, row].Infectious = true || currentGrid[i, j].IndividualState == "I")
                {
                    ineighbour++;
                }            
            }
        }

        if (currentGrid[i, j].IndividualState == "I") // if infected it removes it self from the count. Mike knows how to remove heroin from peoples houses.
        {
            ineighbour--;
        }
        if (currentGrid[i, j].IndividualState == "E")
        {
            currentGrid[i, j].Age++;
        }
        // Actually implements simulation rules
        TransmissionRate = (ineighbour * ActiveVirus.Infectionrate * (1 - currentGrid[x, j].MaskProtectionRate) * (1 - currentGrid[x, j].HygeineRating) * (1 - currentGrid[x, j].VaxProtectionRate)) / (manhatdist * 8);
        randomDouble = random.NextDouble();
        if (Newgrid[i, j].IndividualState == "S" && (randomDouble < TransmissionRate /*Infection Rate */ && ineighbour > 1) /*I need to get the neighbours and put int into here > 1 */)  //  checks if cell is susceptible and then also checks if the random number is small than the infection rate.
        {
            currentGrid[i, j].IndividualState = "E";
            currentGrid[i, j].Infectious = true;
        }
        else if (Newgrid[i, j].IndividualState == "E" && (Newgrid[i, j].Age > ActiveVirus.Incubationtimes))
        {
            currentGrid[i, j].IndividualState = "I";
        }
        else if (Newgrid[i, j].IndividualState == "I" && (randomDouble > ActiveVirus.RecoveryChance || Newgrid[i, j].InfectedAge > ActiveVirus.RecoveryTime))
        {
            currentGrid[i, j].IndividualState = "R";
            currentGrid[i, j].Infectious = false;
        }
        Newgrid[i, j] = currentGrid[i, j];
    }
    static void CreateOwn()
    {
        // Grid creating method ; Self explanatory using width and height to create grid and using it to get initial population of neighbour hood for CA.
        int Width, Height, DaysSimulated; string Temp, FileName = ""; double Percent; bool MakeFile = false;
        do
        {
            Console.WriteLine("Please input how many columns you would like");
            string temp = Console.ReadLine();

            if (!int.TryParse(temp, out Height) || Height <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive integer value for the number of columns.");
            }
        } while (Height <= 0);

        do
        {
            Console.WriteLine("Please input how many rows you would like");
            string temp = Console.ReadLine();
            if (!int.TryParse(temp, out Width) || Width <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive integer value for the number of columns.");
            }
        } while (Width <= 0);

        do
        {
            Console.WriteLine("Please enter what % of the population you would like to infect at the start of the game.");
            Temp = Console.ReadLine();
            if (!double.TryParse(Temp, out Percent) || Percent < 0.00)
            {
                Console.WriteLine("Please enter a number above 0");
            }
            else if (!double.TryParse(Temp, out Percent) || Percent > 1.00)
            {
                Console.WriteLine("Please enter a number less than 1");
            }

        } while (Percent < 0.00 || Percent > 1.00);

        do
        {
            Console.WriteLine("How many days would you like simulated.");
            Temp = Console.ReadLine();
            int.TryParse(Temp, out DaysSimulated);
            if (!int.TryParse(Temp, out DaysSimulated) || DaysSimulated <= 0)
            {
                Console.WriteLine("Please enter a minimum of 1 day");
            }
            else if (!int.TryParse(Temp, out DaysSimulated) || DaysSimulated >= 40172)
            {
                Console.WriteLine("Please enter a maximum of 30");
            }
        } while (DaysSimulated <= 0 || DaysSimulated >= 40172);

        do
        {
            System.Console.WriteLine("What would you like to name your file it has to be longer than 4 Characters");
            FileName = Console.ReadLine();
        } while (FileName.Length < 4);

        try
        {
            DrawsGrids(Width, Height, Percent, DaysSimulated, MakeFile, FileName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    static void Import()
    {
        bool MakeFile = false;
        int generation = 1;
        int DaysSimulated;
        string Temp;
        double Percent;
        bool NotValid = false;
        Random random = new Random();

        // Read a file to import a simulation and the other surrounding information.
        Console.WriteLine("Please enter the File you want to read from:");
        string FileName = Console.ReadLine();
        FileName = FileName + ".txt";
        do
        {
            Console.WriteLine("How many days would you like simulated?");
            Temp = Console.ReadLine();
        } while (!int.TryParse(Temp, out DaysSimulated) || DaysSimulated <= 0 || DaysSimulated >= 40173);
        int[] Susceptible = new int[DaysSimulated];
        int[] Exposed = new int[DaysSimulated];
        int[] Infected = new int[DaysSimulated];
        int[] Recovered = new int[DaysSimulated];


        string[] lines = File.ReadAllLines(FileName);
        int Height = lines.Length;
        int Width = lines[0].Length;
        Console.WriteLine(lines[0]);
        int population = Width * Height;
        Percent = (double)population / Height;
        Cell[,] currentGrid = new Cell[Height, Width];

        for (int i = 0; i < Height; i++)
        {
            string line = lines[i];
            for (int j = 0; j < Width; j++)
            {
                char c = line[j];
                if (c == 's')
                {
                    Console.Write(c);
                    double randomDouble = random.NextDouble();
                    double randomHyg = random.NextDouble();
                    double randomVax = random.NextDouble();
                    double RandMask = random.NextDouble();
                    currentGrid[i, j] = new Susceptible("Cell"+Width+Height, 0, randomHyg, randomVax, RandMask, 0);
                }
                else if (c == 'e')
                {
                    Console.Write(c);
                    double randomDouble = random.NextDouble();
                    double randomHyg = random.NextDouble();
                    double randomVax = random.NextDouble();
                    double RandMask = random.NextDouble();
                    currentGrid[i, j] = new Exposed("Cell"+Width+Height, 0, randomHyg, randomVax, RandMask, 0);
                }
                else if (c == 'i')
                {
                    Console.Write(c);
                    currentGrid[i, j] = new Infected("Cell"+Width+Height, 0, 0, 0, 0, 1);
                }
                else if (c == 'r')
                {
                    Console.Write(c);
                    currentGrid[i, j] = new Removed("Cell"+Width+Height, 0, 0, 0, 0, 0);
                }
                else
                {
                    Console.WriteLine("Error in the File on Line {0}, {1}", i, j);
                    NotValid = true;
                }
            }
            Console.WriteLine("");
        }
        if (!NotValid)
        {
            Virus ActiveVirus = Virusyo(Width, Height, Percent, DaysSimulated, MakeFile, FileName);
            NextGeneration(currentGrid, Width, Height, population, generation, DaysSimulated, MakeFile, FileName, ActiveVirus, Susceptible, Exposed, Infected, Recovered);
        }
        else
        {
            Console.WriteLine("Please ammend the file with the Errors Locations above.");
        }
    }
    
    static void Main()
    {
        string Start;
        Console.WriteLine("Please enter whether you would like to Import (1) a generation or create your own (2).");
        do
        {
            Start = Console.ReadLine();
            if (Start == "1")
            {
                Import();
            }
            else if (Start == "2")
            {
                CreateOwn();
            }
            if (Start != "1" | Start != "2")
            {
                Console.WriteLine("Please enter an integer Value either being 1 or 2");
            }
        } while (Start != "1" | Start != "2");
    }


}
   