namespace GCTA
{
    enum ElementType
    {
        Fire,
        Water,
        Grass,
        Electric,
        Normal
    }
    class Move
    {
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; }
        public ElementType Type { get; set; }
        public int EnergyCost { get; set; }
        public int EnergyGain { get; set; }
        public Move(string name, int damage, ElementType type, int energyCost, int energyGain)
        {
            Name = name;
            Damage = damage;
            Type = type;
            EnergyCost = energyCost;
            EnergyGain = energyGain;
        }

    }
    class Pokemon
    {
        public string Name { get; set; } = string.Empty;
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public ElementType Type { get; set; }
        public int CurrentEnergy { get; private set; }
        public List<Move> Moves = new List<Move>();

        public void TakeDamage(int amount) {
        Health = Health - amount;
            if(Health < 0)
            {
                Health = 0;
            }
        }
        public void GainEnergy(int amount) { }
        public void UseMove(int moveIndex, Pokemon targetPokemon) {
            var move = Moves[moveIndex];
            if(CurrentEnergy < move.EnergyCost)
            {
                Console.WriteLine($"{Name} does not have enough energy to use {move.Name}!");
                return;
            }

            float multiplier = BattleCalculator.ElementMultiplier(move.Type, targetPokemon.Type);
            int TotalDamage = (int)(move.Damage * multiplier);
            targetPokemon.TakeDamage(TotalDamage);
            CurrentEnergy -= move.EnergyCost;
            CurrentEnergy += move.EnergyGain;
            Console.WriteLine($"{Name} used {move.Name} on {targetPokemon.Name} dealing {TotalDamage} damage!");





        }
        public Pokemon(string name, int maxhealth, ElementType type,Move normalmove,Move specialmove1,Move specialmove2)
        {
            Name = name;
            Health = maxhealth;
            MaxHealth = maxhealth;
            Type = type;
            CurrentEnergy = 0;
            Moves = new List<Move>();
            Moves.Add(normalmove);
            Moves.Add(specialmove1);
            Moves.Add(specialmove2);
        }

    }
    class BattleCalculator
    {
        public static float ElementMultiplier(ElementType attackerType, ElementType defenderType)
        {
            if (attackerType == ElementType.Fire && defenderType == ElementType.Grass) return 1.5f;
            if (attackerType == ElementType.Water && defenderType == ElementType.Fire) return 1.5f;
            if (attackerType == ElementType.Grass && defenderType == ElementType.Water) return 1.5f;
            if (attackerType == ElementType.Electric && defenderType == ElementType.Water) return 1.5f;
            if (attackerType == ElementType.Grass && defenderType == ElementType.Fire) return 0.5f;
            if (attackerType == ElementType.Fire && defenderType == ElementType.Water) return 0.5f;
            if (attackerType == ElementType.Water && defenderType == ElementType.Grass) return 0.5f;
            if (attackerType == ElementType.Water && defenderType == ElementType.Electric) return 0.5f;
            if (attackerType == ElementType.Normal && defenderType == ElementType.Normal) return 1.0f;

            return 1.0f;
        }
    }
    class Program
    {
        public static void Main(string[] args)
        {
            //Pikachu: Thundershock,Thunderbolt,Quick Attack hp70
            //Charmander: Ember,Flamethrower,Scratch    hp80
            //Squirtle: Water Gun,Hydro Pump,Tackle     hp70
            Pokemon charmander = new Pokemon("Charmander", 80, ElementType.Fire,
                new Move("Scratch", 10, ElementType.Normal, 0, 20),
                new Move("Ember", 20, ElementType.Fire, 10, 5),
                new Move("Flamethrower", 35, ElementType.Fire, 30, 0)
                );
            Pokemon squirtle = new Pokemon("Squirtle", 70, ElementType.Water,
                new Move("Tackle", 5, ElementType.Normal, 0, 30),
                new Move("Water Gun", 15, ElementType.Water, 10, 5),
                new Move("Hydro Pump", 30, ElementType.Water, 35, 0)
                );
            charmander.UseMove(0, squirtle);
            squirtle.UseMove(1, charmander);
            charmander.UseMove(1, squirtle);
            squirtle.UseMove(0, charmander);
            charmander.UseMove(1, squirtle);
            squirtle.UseMove(1, charmander);

        }
    }
}
    
