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

        public void TakeDamage(int amount)
        {
            Health = Health - amount;
            if (Health < 0)
            {
                Health = 0;
            }
        }
        public bool IsFainted()
        {
            if (Health == 0)
            {
                return true;
            }
            return false;
        }
        public void ShowStatus()
        {
            Console.WriteLine(IsFainted() ? $"{Name} has fainted!" : $"{Name} has [{Health}/{MaxHealth}] HP and [{CurrentEnergy}] energy.");
        }
      
        public void UseMove(int moveIndex, Pokemon targetPokemon)
        {
            var move = Moves[moveIndex];
            if (CurrentEnergy < move.EnergyCost)
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
        public Pokemon(string name, int maxhealth, ElementType type, Move normalmove, Move specialmove1, Move specialmove2)
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
    class Trainer
    {
        public string Name { get; set; }
        public List<Pokemon> Tpokemon = new List<Pokemon>();
        public void AddPokemon(Pokemon pokemon)
        {
            Tpokemon.Add(pokemon);
        }
        public void ShowTeam()
        {
            for(int i = 0; i<Tpokemon.Count;i++)
            {
                Console.WriteLine($"{i}: {Tpokemon[i].Name} (HP: {Tpokemon[i].Health}/{Tpokemon[i].MaxHealth}) [{Tpokemon[i].IsFainted()}]");
            }
        }
        public Pokemon GetPokemon(int index)
        {
            if(index < 0 || index >= Tpokemon.Count)
            {
                Console.WriteLine("Invalid index!");
                return null;
            }

            if(Tpokemon[index].IsFainted())
            {
                Console.WriteLine($"{Tpokemon[index].Name} has fainted!");
                return null;
            }   
            return Tpokemon[index];


        }
        public bool HasAlivePokemon()
        {
            foreach (var pokemon in Tpokemon)
            {
                if (!pokemon.IsFainted())
                {
                    return true;
                }
            }
            return false;
        }
        public Trainer()
        {
            Name = "Ash";
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
            Trainer playerTrainer = new Trainer();
            Trainer enemyTrainer = new Trainer();
            Pokemon charmander = new Pokemon("Charmander", 200, ElementType.Fire,
                new Move("Scratch", 30, ElementType.Normal, 0, 15),
                new Move("Ember", 55, ElementType.Fire, 30, 0),
                new Move("Flamethrower", 150, ElementType.Fire, 54, 0)
                );
            Pokemon squirtle = new Pokemon("Squirtle", 200, ElementType.Water,
                new Move("Tackle", 20, ElementType.Normal, 0, 30),
                new Move("Water Gun", 45, ElementType.Water, 20, 5),
                new Move("Hydro Pump", 90, ElementType.Water, 50, 0)
                );
            Pokemon pikachu = new Pokemon("Pikachu", 200, ElementType.Electric,
                new Move("Quick Attack", 25, ElementType.Normal, 0, 25),
                new Move("Thunder Shock", 40, ElementType.Electric, 30, 5),
                new Move("Thunderbolt", 100, ElementType.Electric, 45, 0)
                );
            Pokemon flareon = new Pokemon("Flareon", 210, ElementType.Fire,
                new Move("Flaming Breath", 20, ElementType.Normal, 0, 25),
                new Move("Flamethrower", 70, ElementType.Fire, 30, 5),
                new Move("Scorching Column", 120, ElementType.Fire, 60, 0)
                );
            Pokemon leafeon = new Pokemon("Leafeon", 210, ElementType.Grass,
                new Move("Soothing Scent", 30, ElementType.Normal, 0, 15),
                new Move("Magical Leaf", 50, ElementType.Grass, 20, 5),
                new Move("Slashing Strike", 140, ElementType.Grass, 60, 0)
                );
            Pokemon vaporeon = new Pokemon("Vaporeon", 210, ElementType.Water,
                new Move("Spiral Drain", 30, ElementType.Water, 0, 20),
                new Move("Aqua Sonic", 50, ElementType.Water, 25, 5),
                new Move("Splash Jump", 90, ElementType.Water, 55, 0)
                );
            playerTrainer.AddPokemon(pikachu);
            playerTrainer.AddPokemon(vaporeon);
            playerTrainer.AddPokemon(charmander);
            enemyTrainer.AddPokemon(squirtle);
            enemyTrainer.AddPokemon(flareon);
            enemyTrainer.AddPokemon(leafeon);
            while (playerTrainer.HasAlivePokemon() && enemyTrainer.HasAlivePokemon())
            {
                for (int i = 0; i < playerTrainer.Tpokemon.Count; i++)
                {
                    var attacker = playerTrainer.Tpokemon[i];
                    if (attacker.IsFainted())
                        continue;
                    Console.WriteLine($"Its {attacker.Name}'s turn.");
                    enemyTrainer.ShowTeam();
                    Console.Write("Who are you going to attack?\n: ");
                    int index = int.Parse(Console.ReadLine() ?? "0");
                    var target = enemyTrainer.Tpokemon[index];
                    for (int j = 0; j < attacker.Moves.Count; j++)
                    {
                        var move = attacker.Moves[j];
                        Console.WriteLine($"{j}: {move.Name} (Damage: {move.Damage}, Type: {move.Type}, Energy Cost: {move.EnergyCost}, Energy Gain: {move.EnergyGain})");
                    }
                    Console.WriteLine("Which skill will you use?: ");
                    int chosenSkill = int.Parse(Console.ReadLine());
                    attacker.UseMove(chosenSkill, target);

                }
                for (int i = 0; i < enemyTrainer.Tpokemon.Count; i++)
                {
                    var attacker = enemyTrainer.Tpokemon[i];
                    if (attacker.IsFainted())
                        continue;
                    var alivePokemons = playerTrainer.Tpokemon.Where(p => !p.IsFainted()).ToList();//
                    var target = alivePokemons[new Random().Next(alivePokemons.Count)];
                    var moveIndex = new Random().Next(attacker.Moves.Count);
                    attacker.UseMove(moveIndex, target);
                }
            }
        }
    }
}


