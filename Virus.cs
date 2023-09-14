class Virus{
        public string Virusname { get; set; }
        public int Incubationtimes { get; set; }
        public double Infectionrate { get; set; }
        public int RecoveryTime { get; set; }
        public double RecoveryChance { get; set; }
        public double Fatalityrate { get; set; }
        public Virus(string Virusname, int Incubationtimes, double Infectionrate, int RecoveryTime, double RecoveryChance, double Fatalityrate)
        {
            this.Virusname = Virusname;
            this.Incubationtimes = Incubationtimes;
            this.Infectionrate = Infectionrate;
            this.RecoveryTime = RecoveryTime;
            this.RecoveryChance = RecoveryChance;
            this.Fatalityrate = Fatalityrate;
        }
}