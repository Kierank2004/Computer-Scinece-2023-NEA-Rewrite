public class Cell
    {
        // Basic traits for each cell
        public string cellName {get; set;}
        public int Age;
        // SEIR model
        public string IndividualState { get; set; } // The only types of states there are are 'S', 'E, 'I', 'R'                
        public bool Infectious { get; set; }
        // How much does the defence help out.
        public double HygeineRating { get; set; }
        public double VaxProtectionRate { get; set; }
        public double MaskProtectionRate { get; set; }
        public int InfectedAge { get; set; }
        public Cell(string cellName ,int age, string individualState, bool infectious, double HygeineRating, double VaxProtectionRate, double MaskProtectionRate, int InfectedAge)
        {
            this.cellName = cellName;
            this.Age = age;
            this.IndividualState = individualState;
            this.Infectious = infectious;
            this.HygeineRating = HygeineRating;
            this.VaxProtectionRate = VaxProtectionRate;
            this.MaskProtectionRate = MaskProtectionRate;
            this.InfectedAge = InfectedAge;
        }
    }
    public class Susceptible : Cell
    {
        public Susceptible(string cellName, int age, double hygieneRating, double vaxProtectionRate, double maskProtectionRate, int InfectedAge)
            : base(cellName, age, "S", false, hygieneRating, vaxProtectionRate, maskProtectionRate, 0)
        {
        }
    }

    public class Exposed : Cell
    {
        public Exposed(string cellName, int age, double hygieneRating, double vaxProtectionRate, double maskProtectionRate, int InfectedAge)
            : base(cellName, age, "E", false, hygieneRating, vaxProtectionRate, maskProtectionRate, 0)
        {
        }
    }

    public class Infected : Cell
    {
        public Infected(string cellName, int age, double hygieneRating, double vaxProtectionRate, double maskProtectionRate, int InfectedAge)
            : base(cellName, age, "I", true, hygieneRating, vaxProtectionRate, maskProtectionRate, 0)
        {
        }
    }

    public class Removed : Cell
    {
        public Removed(string cellName, int age, double hygieneRating, double vaxProtectionRate, double maskProtectionRate, int InfectedAge)
            : base(cellName, age, "R", false, hygieneRating, vaxProtectionRate, maskProtectionRate, 0)
        {
        }
    }