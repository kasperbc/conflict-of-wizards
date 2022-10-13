using Peliprojekti;
using System.Threading;

Random r = new Random();

Spell shield = new Spell("Shield", 1);
Spell heal = new Spell("Heal", 1);
Spell blast = new Spell("Blast", 3);
Spell charge = new Spell("Charge", 2);
Spell[] spells = new Spell[] { shield, heal, blast, charge };

Unit player = new Unit("Player", 5, 30);
Unit ally1 = new Unit("Ally 1", 5, 15);
Unit ally2 = new Unit("Ally 2", 5, 15);

Unit enemy1 = new Unit("Enemy 1", 5, 15);
Unit enemy2 = new Unit("Enemy 2", 5, 15);
Unit enemy3 = new Unit("Enemy 3", 5, 15);

Unit[] team1 = new Unit[] { player, ally1, ally2 };
Unit[] team2 = new Unit[] { enemy1, enemy2, enemy3 };

Console.WriteLine("A battle is starting...");

// Main loop
while (true)
{
    // Write menu
    WriteStatus();

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("\nYour turn!\n");
    Thread.Sleep(750);

    while (true)
    {
        if (player.GetStatus() == Unit.Status.Dead)
        {
            break;
        }

        // Write menu
        WriteStatus();

        // Player's turn
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("What will you do?");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("1: Attack");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("2: Recharge");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("3: Magic");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("4: Info");

        int input = GetInput();

        // Write menu
        WriteStatus();

        // Attack
        if (input == 1)
        {
            if (player.GetMana() == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No mana! Cannot attack!");
                Thread.Sleep(750);
                continue;
            }

            int target = 1;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\nWho will you attack?");
                for (int i = 1; i <= team2.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    if (team2[i - 1].GetStatus() == Unit.Status.Dead)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("(Dead) ");
                    }

                    Console.WriteLine(i + ": " + team2[i - 1].GetName());
                }

                target = GetInput();

                if (team2[target - 1].GetStatus() == Unit.Status.Dead)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Cannot attack the dead!");
                    continue;
                }
                else
                {
                    break;
                }
            }

            // Attack target
            Attack(player, team2[target - 1]);
            Thread.Sleep(750);
        }
        // Recharge
        else if (input == 2)
        {
            RechargeMana(player);
        }
        // Magic
        else if (input == 3)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\nWhich spell will you use?");
                for (int i = 1; i <= spells.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;

                    if (!CanUseSpell(player, spells[i - 1]))
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("(Not enough mana) ");
                    }

                    Console.WriteLine(i + ": " + spells[i - 1].GetName() + " (Cost: " + spells[i - 1].GetCost() + ")");
                }

                int spell = GetInput();

                if ((spell <= spells.Length && spell > 0) && CanUseSpell(player, spells[spell - 1]))
                {
                    UseSpell(player, spells[spell - 1], team2);
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Cannot use spell!");
                }
            }
        }
        // Info
        else if (input == 4)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (Unit enemy in team2)
            {
                Console.WriteLine(enemy.GetName() + " has " + enemy.GetHitPoints() + " health and " + enemy.GetMana() + " mana.");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Invalid input!");
            continue;
        }

        break;
    }

    // Allies turn
    DoNPCActions(team1, team2);

    if (CheckTeamsAlive(team1, team2) < 2)
    {
        break;
    }

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("\nEnemy's turn!\n");
    Thread.Sleep(1000);

    // Enemy turn
    DoNPCActions(team2, team1);

    if (CheckTeamsAlive(team1, team2) < 2)
    {
        break;
    }
}

Console.WriteLine("Game over!");

void Attack(Unit attacker, Unit victim)
{
    int baseDamage = (int)(attacker.GetAttackPower() * attacker.GetAttackMultiplier());

    int damage = baseDamage - r.Next(-1, 1);

    Console.ForegroundColor = ConsoleColor.Red;

    if (victim.GetStatus() == Unit.Status.Shield)
    {
        Console.WriteLine(attacker.GetName() + " attacks! " + victim.GetName() + " is shielded and doesn't take damage!");
    }
    else
    {
        Console.WriteLine(attacker.GetName() + " attacks! " + victim.GetName() + " takes " + damage + " damage!");
    }
    victim.Damage(damage);

    if (victim.GetStatus() == Unit.Status.Dead)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(victim.GetName() + " falls over in defeat!");
        Thread.Sleep(1000);
    }

    attacker.AddMana(-1);
}

void RechargeMana(Unit target)
{
    target.AddMana(3);
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(target.GetName() + " recharges their mana...");
    Thread.Sleep(750);
}

int CheckTeamsAlive(Unit[] team1, Unit[] team2)
{
    int teamsAlive = 0;

    if (GetTeamAliveCount(team1) > 0)
    {
        teamsAlive++;
    }

    if (GetTeamAliveCount(team2) > 0)
    {
        teamsAlive++;
    }

    return teamsAlive;
}

int GetTeamAliveCount(Unit[] t)
{
    int alive = 0;

    foreach (Unit u in t)
    {
        if (u.GetStatus() != Unit.Status.Dead)
        {
            alive++;
        }
    }

    return alive;
}

int DecideAction(Unit target)
{
    if (r.Next(100) < 75 - (target.GetMana() * 25))
    {
        return 2;
    }
    if (target.GetMana() == 3 && r.Next(2) == 1)
    {
        return 3;
    }
    return 1;
}

Spell DecideSpell(Unit target)
{
    if (target.GetHitPoints() < target.GetMaxHealth() / 2)
    {
        if (r.Next(100) > 100 - (target.GetHitPoints() / (target.GetMaxHealth() / 2) * 100))
        {
            return heal;
        }
        return shield;
    }
    else
    {
        if (r.Next(2) == 1)
        {
            return blast;
        }
        return charge;
    }
}

void UseSpell(Unit user, Spell spell, Unit[] enemyTeam)
{
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Write(user.GetName() + " casts " + spell.GetName() + "! ");

    switch (spell.GetName())
    {
        // Shield
        case "Shield":
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(user.GetName() + " is shielded for 1 turn!");
            user.SetStatus(Unit.Status.Shield);
            break;
        // Heal
        case "Heal":
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(user.GetName() + "'s health is replenished!");
            user.Damage(-5);
            break;
        // Blast
        case "Blast":
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(user.GetName() + " blasts the other team!");

            foreach(Unit u in enemyTeam)
            {
                if (u.GetStatus() == Unit.Status.Dead)
                {
                    continue;
                }

                user.SetAttackMultiplier(0.33f);
                Attack(user, u);
                user.SetAttackMultiplier(1);
            }

            break;
        // Charge
        case "Charge":
            if (user.GetStatus() != Unit.Status.Charged)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(user.GetName() + " charges up a strong attack!");
                user.SetStatus(Unit.Status.Charged);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(user.GetName() + " casts a strong attack!");
                foreach (Unit u in enemyTeam)
                {
                    if (u.GetStatus() == Unit.Status.Dead)
                    {
                        continue;
                    }

                    user.SetAttackMultiplier(1.5f);
                    Attack(user, u);
                    user.SetAttackMultiplier(1);
                }
                user.SetStatus(Unit.Status.Neutral);
            }
            break;
    }

    user.AddMana(-spell.GetCost());

    Thread.Sleep(1000);
}

bool CanUseSpell(Unit user, Spell spell)
{
    return user.GetMana() >= spell.GetCost();
}

void DoNPCActions(Unit[] thisTeam, Unit[] enemyTeam)
{
    foreach (Unit unit in thisTeam)
    {
        if (unit.GetStatus() == Unit.Status.Dead || unit.GetName() == "Player")
        {
            continue;
        }

        int action = DecideAction(unit);

        // Attack
        if (action == 1)
        {
            int target = r.Next(enemyTeam.Length);

            Attack(unit, enemyTeam[target]);
            Thread.Sleep(750);
        }
        else if (action == 2)
        {
            RechargeMana(unit);
        }
        else if (action == 3)
        {
            Spell s = DecideSpell(unit);

            UseSpell(unit, s, enemyTeam);
        }
    }
}

int GetInput()
{
    ConsoleKeyInfo key = Console.ReadKey(true);

    if (char.IsDigit(key.KeyChar) == false)
    {
        return -1;
    }

    return int.Parse(key.KeyChar.ToString());
}

void WriteStatus()
{
    Console.Clear();

    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("\n---------------STATUS--------------\n");
    Console.ForegroundColor = ConsoleColor.Yellow;
    foreach (Unit ally in team1)
    {
        Console.WriteLine(ally.GetName() + ": | Health: (" + ally.GetHitPoints() + "/" + ally.GetMaxHealth() + ") | Mana: " + ally.GetMana());
    }
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("\n-----------------------------------");
}