﻿using Peliprojekti;
using System.Threading;

Random r = new Random();

Spell shield = new Spell("Shield", 1);
Spell heal = new Spell("Heal", 1);
Spell blast = new Spell("Blast", 3);
Spell charge = new Spell("Charge", 2);
Spell[] spells = new Spell[] { shield, heal, blast, charge };

Unit player = new Unit("Player", 5, 30);
Unit ally1 = new Unit("random", 5, 15);
Unit ally2 = new Unit("random", 5, 15);

Unit enemy1 = new Unit("random", 5, 15);
Unit enemy2 = new Unit("random", 5, 15);
Unit enemy3 = new Unit("random", 5, 15);

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
                Console.WriteLine("Who will you attack?");
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

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("X: Undo");

                target = GetInput();

                // Undo
                if (target == -2)
                {
                    goto LoopEnd;
                }
                else if (target == -1)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Invalid input!");
                    continue;
                }

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
            WriteStatus();
            Attack(player, team2[target - 1]);
            Thread.Sleep(750);
        }
        // Recharge
        else if (input == 2)
        {
            WriteStatus();
            RechargeMana(player);
        }
        // Magic
        else if (input == 3)
        {
            if (player.GetMana() == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No mana! Cannot use magic!");
                Thread.Sleep(750);
                continue;
            }

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Which spell will you use?");
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

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("X: Undo");

                int spell = GetInput();

                // Undo
                if (spell == -2)
                {
                    goto LoopEnd;
                }

                if ((spell <= spells.Length && spell > 0) && CanUseSpell(player, spells[spell - 1]))
                {
                    WriteStatus();
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
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Invalid input!");
            continue;
        }

        break;
    }

    Console.WriteLine();

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

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);

    LoopEnd:
    Console.Clear();
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
            int target = DecideTarget(enemyTeam);

            if (target == -1)
                return;

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

        Console.WriteLine();
    }
}

int GetInput()
{
    ConsoleKeyInfo key = Console.ReadKey(true);

    if (key.KeyChar.Equals('x'))
    {
        return -2;
    }
    else if (char.IsDigit(key.KeyChar) == false)
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
    // Ally status
    Console.ForegroundColor = ConsoleColor.Yellow;
    foreach (Unit ally in team1)
    {
        Console.WriteLine(ally.GetName() + ": | Health: (" + ally.GetHitPoints() + "/" + ally.GetMaxHealth() + ") | Mana: " + ally.GetMana());
    }
    // Enemy status
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Red;
    foreach (Unit enemy in team2)
    {
        Console.WriteLine(enemy.GetName() + ": | Health: " + GetVagueHealth(enemy) + " | Mana: " + GetVagueMana(enemy));
    }

    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine("\n-----------------------------------\n");
}

int DecideTarget(Unit[] enemyTeam)
{
    int bestTarget = -1;

    for (int i = 0; i < enemyTeam.Length; i++)
    {
        if (enemyTeam[i].GetStatus() == Unit.Status.Dead || enemyTeam[i].GetStatus() == Unit.Status.Shield)
        {
            continue;
        }

        // Pick target with most health
        if (bestTarget == -1 || enemyTeam[bestTarget].GetHitPoints() < enemyTeam[i].GetHitPoints())
        {
            bestTarget = i;
        }
    }

    return bestTarget;
}

string GetVagueHealth(Unit unit)
{
    int health = unit.GetHitPoints();
    int maxHealth = unit.GetMaxHealth();

    float healthToMaxRatio = (float)health / maxHealth;
    string[] vagueHealth = { "Unknown" };


    if (healthToMaxRatio > 0.75f)
    {
        vagueHealth = new string[]{ "Healthy", "Lively", "Strong", "Tough" };
    }
    else if (healthToMaxRatio > 0.5f)
    {
        vagueHealth = new string[] { "Damaged", "Scratched", "Bruised" };
    }
    else if (healthToMaxRatio > 0.25f)
    {
        vagueHealth = new string[] { "Injured", "Wounded", "Crushed" };
    }
    else
    {
        vagueHealth = new string[] { "Bleeding", "Barely standing", "Deformed" };
    }

    return vagueHealth[r.Next(vagueHealth.Length)];
}

string GetVagueMana(Unit unit)
{
    int mana = unit.GetMana();

    string[] vagueMana = { "Unknown" };

    if (mana == 3)
    {
        vagueMana = new string[] { "Strong", "Hefty", "Well", "Vogorous" };
    }
    else if (mana == 2)
    {
        vagueMana = new string[] { "Standard", "Usual", "Ordinary", "Habitual" };
    }
    else if (mana == 1)
    {
        vagueMana = new string[] { "Not so much", "Reduced", "Few", "Running low" };
    }
    else
    {
        vagueMana = new string[] { "Drained", "Void", "Removed" };
    }

    return vagueMana[r.Next(vagueMana.Length)]; ;
}