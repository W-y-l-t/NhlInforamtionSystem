MIGRATION = "1.0"

from config import TARGET
from db import cursor
from math   import ceil, floor

def _next_name(prefix: str, taken: set[int]) -> str:
    n = 1
    while n in taken:
        n += 1
    taken.add(n)
    return f"{prefix}{n}"

def seed_divisions(cur):
    cur.execute("SELECT to_regclass('divisions') as ex")
    if cur.fetchone()["ex"] is None:
        return
    
    cur.execute("SELECT division_name FROM divisions")
    existing  = {r["division_name"] for r in cur.fetchall()}
    to_create = max(0, TARGET["divisions"] - len(existing))

    taken_idx = {int(name.removeprefix("Division"))
                 for name in existing if name.startswith("Division") and
                 name.removeprefix("Division").isdigit()}

    while to_create != 0:
        name = _next_name("Division", taken_idx)
        cur.execute("INSERT INTO divisions (division_name) VALUES (%s)", (name,))
        existing.add(name)
        to_create -= 1


def seed_conferences(cur):
    cur.execute("SELECT to_regclass('conferences') as ex")
    if cur.fetchone()["ex"] is None:
        return
    
    cur.execute("SELECT conference_name FROM conferences")
    existing  = {r["conference_name"] for r in cur.fetchall()}
    to_create = max(0, TARGET["conferences"] - len(existing))

    taken_idx = {int(name.removeprefix("Conference"))
                 for name in existing if name.startswith("Conference") and
                 name.removeprefix("Conference").isdigit()}

    while to_create != 0:
        name = _next_name("Conference", taken_idx)
        cur.execute("INSERT INTO conferences (conference_name) VALUES (%s)", (name,))
        existing.add(name)
        to_create -= 1


def seed_awards(cur):
    cur.execute("SELECT to_regclass('awards') as ex")
    if cur.fetchone()["ex"] is None:
        return

    goal_total = int(TARGET["awards"])
    goal_team  = int(ceil(goal_total / 2))
    goal_individual = int(floor(goal_total / 2))

    cur.execute("SELECT name, category FROM awards")
    existing = {(r["name"], r["category"]) for r in cur.fetchall()}

    taken_team       = {int(n.removeprefix("TeamAward"))
                   for n, c in existing if c == "team" and
                   n.startswith("TeamAward") and n.removeprefix("TeamAward").isdigit()}
    taken_individual = {int(n.removeprefix("IndividualAward"))
                   for n, c in existing if c == "individual" and
                   n.startswith("IndividualAward") and n.removeprefix("IndividualAward").isdigit()}

    while sum(1 for _, c in existing if c == "team") < goal_team:
        name = _next_name("TeamAward", taken_team)
        cur.execute("INSERT INTO awards (name, category) VALUES (%s,'team')", (name,))
        existing.add((name, "team"))

    while sum(1 for _, c in existing if c == "individual") < goal_individual:
        name = _next_name("IndividualAward", taken_individual)
        cur.execute("INSERT INTO awards (name, category) VALUES (%s,'individual')", (name,))
        existing.add((name, "individual"))

def seed():
    with cursor() as cur:
        seed_divisions(cur)
        seed_conferences(cur)
        seed_awards(cur)
