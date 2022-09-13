using Peliprojekti;
using System.Threading;

Unit w1 = new Unit("Warrior 1", 10, 30);
Unit w2 = new Unit("Warrior 2", 10, 30);

Unit[] team1 = new Unit[] { w1 };
Unit[] team2 = new Unit[] { w2 };

Console.WriteLine("A battle is starting...");

while (true)
{
    Attack(w1, w2);
    if (CheckTeamsAlive(team1, team2) <= 1)
    {
        break;
    }

    Thread.Sleep(1000);

    Attack(w2, w1);
    if (CheckTeamsAlive(team1, team2) <= 1)
    {
        break;
    }

    Thread.Sleep(1000);
}

Console.WriteLine("Game over!");

void Attack(Unit attacker, Unit victim)
{
    Console.WriteLine(attacker.GetName() + " attacks! " + victim.GetName() + " takes " + attacker.GetAttackPower() + " damage!");
    victim.Damage(attacker.GetAttackPower());
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