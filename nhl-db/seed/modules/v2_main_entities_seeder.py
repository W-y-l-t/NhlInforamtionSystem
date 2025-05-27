MIGRATION = "2.0"

from faker import Faker
import random, datetime
from config import TARGET
from db import cursor

fake = Faker()

SPECIALIZATION_MAP = {
    "COACHING STAFF": [
        "Head Coach", "Assistant Coach", "Goaltending Coach",
        "Video Coach", "Video Coordinator"
    ],
    "EQUIPMENT": [
        "Head Equipment Manager", "Equipment Manager",
        "Assistant Equipment Manager"
    ],
    "EXECUTIVE MANAGEMENT": [
        "Owner/Governor", "President", "CEO",
        "General Manager", "Chief Financial Officer",
        "Executive Coordinator"
    ]
}

POSITIONS = ["goalie", "defenseman", "center", "left_wing", "right_wing"]

def seed_arenas_and_teams(cur):
    cur.execute("SELECT to_regclass('teams') AS ex")
    if cur.fetchone()["ex"] is None:
        return

    cur.execute("SELECT COUNT(*) FROM teams")
    existing  = cur.fetchone()["count"]
    to_create = max(0, TARGET["teams"] - existing)
    if to_create <= 0:
        return

    cur.execute("SELECT id FROM divisions")
    divisions   = [r["id"] for r in cur.fetchall()]
    
    cur.execute("SELECT id FROM conferences")
    conferences = [r["id"] for r in cur.fetchall()]

    logo_base   = "https://cdn.nhl.fake/logo/"
    jersey_base = "https://cdn.nhl.fake/jersey/"

    for _ in range(to_create):
        arena_name = fake.company() + " Arena"
        cur.execute("""
            INSERT INTO arenas(name, address, location, capacity)
            VALUES (%s, %s, %s, %s) RETURNING id
        """, (
            arena_name[:100],
            fake.address().replace("\n", ", ")[:255],
            f"{fake.city()}, {fake.state()}"[:100],
            random.randint(15_000, 22_500)
        ))
        arena_id = cur.fetchone()["id"]

        full    = fake.company()
        short   = "".join(w[0] for w in full.split()).upper()
        first3  = full[:100]
        short3  = short[:50]
        city    = fake.city()[:50]
        region  = fake.state()[:50]
        country = fake.country()[:50]
        founded = fake.date_between_dates(
                    date_start=datetime.date(1917,1,1),
                    date_end=datetime.date(2021,1,1)
                )
        entry   = fake.date_between(start_date = founded, end_date = "+3y")
        logo    = (logo_base + short3.lower() + ".png")[:255]
        home_j  = (jersey_base + short3.lower() + "_home.png")[:255]
        away_j  = (jersey_base + short3.lower() + "_away.png")[:255]

        cur.execute("""
            INSERT INTO teams (
                full_name, short_name, city, region, country,
                founded_date, entry_date,
                logo_url, home_uniform_url, away_uniform_url,
                division_id, conference_id, arena_id
            ) VALUES (
                %s,%s,%s,%s,%s,
                %s,%s,
                %s,%s,%s,
                %s,%s,%s
            )
        """, (
            first3, short3, city, region, country,
            founded, entry,
            logo, home_j, away_j,
            random.choice(divisions),
            random.choice(conferences),
            arena_id
        ))

def seed_users(cur):
    cur.execute("SELECT to_regclass('users') AS ex")
    if cur.fetchone()["ex"] is None:
        return

    N = TARGET["users"]
    cur.execute("SELECT COUNT(*) FROM users")
    existing  = cur.fetchone()["count"]
    to_create = max(0, N - existing)

    for _ in range(to_create):
        pwd_hash  = fake.sha256()
        first     = fake.first_name()[:50]
        last      = fake.last_name()[:50]
        email     = fake.unique.email()[:100]
        country   = fake.country()[:50]
        raw_tel   = fake.phone_number()
        phone     = "".join(ch for ch in raw_tel if ch.isdigit() or ch == "+")[:20]
        birth     = fake.date_of_birth(minimum_age=18, maximum_age=80)

        cur.execute("""
            INSERT INTO users (
                first_name, last_name, email,
                password_hash, country, phone, birth_date
            ) VALUES (%s,%s,%s,%s,%s,%s,%s)
        """, (first, last, email, pwd_hash, country, phone, birth))

def seed_players_and_contracts(cur):
    cur.execute("SELECT to_regclass('players') AS ex")
    if cur.fetchone()["ex"] is None:
        return

    M = TARGET["players"]
    cur.execute("SELECT COUNT(*) FROM players")
    existing = cur.fetchone()["count"]
    to_create = max(0, M - existing)

    for _ in range(to_create):
        bd       = fake.date_between_dates(
                    date_start=datetime.date(1985,1,1),
                    date_end=datetime.date(2005,1,1)
                 )
        height   = random.randint(160,210)
        weight   = round(random.uniform(50,99.9),1)
        shot     = random.choice(["left","right"])
        place    = fake.city()[:100]
        position = POSITIONS[random.randint(0, 4)]

        cur.execute("""
            INSERT INTO players (
                first_name, last_name, birth_date, birth_place,
                height_sm, weight_kg, shot, position
            ) VALUES (
                %s,%s,%s,%s,%s,%s,%s,%s
            ) RETURNING id
        """, (
            fake.first_name()[:50],
            fake.last_name()[:50],
            bd, place, height, weight, shot, position
        ))
        pid = cur.fetchone()["id"]

        cur.execute("SELECT id FROM teams ORDER BY RANDOM() LIMIT 1")
        team  = cur.fetchone()["id"]
        start = fake.date_between(start_date="-3y", end_date="today")
        end   = fake.date_between(start_date=start, end_date="+3y")
        term  = end.year - start.year + 1
        amt   = random.randint(500_000, 8_000_000)

        cur.execute("""
            INSERT INTO player_contracts (
                player_id, team_id, start_date, end_date,
                contract_term, amount_usd
            ) VALUES (%s,%s,%s,%s,%s,%s)
        """, (pid, team, start, end, term, amt))

def seed_staff_and_contracts(cur):
    cur.execute("SELECT to_regclass('staff') AS ex")
    if cur.fetchone()["ex"] is None:
        return

    S = TARGET["staff"]
    cur.execute("SELECT COUNT(*) FROM staff")
    existing  = cur.fetchone()["count"]
    to_create = max(0, S - existing)

    all_pos  = [p for ps in SPECIALIZATION_MAP.values() for p in ps]
    pos2spec = {p: spec for spec, ps in SPECIALIZATION_MAP.items() for p in ps}

    for _ in range(to_create):
        pos  = random.choice(all_pos)
        spec = pos2spec[pos]
        bd   = fake.date_of_birth(minimum_age=25, maximum_age=75)
        team = random.randint(1, TARGET["teams"])

        cur.execute("""
            INSERT INTO staff (
                first_name, last_name, birth_date,
                position, specialization, team_id
            ) VALUES (%s,%s,%s,%s,%s,%s) RETURNING id
        """, (fake.first_name(), fake.last_name(), bd, pos, spec, team))
        sid = cur.fetchone()["id"]

        start = fake.date_between(start_date="-5y", end_date="today")
        end   = fake.date_between(start_date=start, end_date="+5y")
        term  = end.year - start.year + 1
        amt   = random.randint(50_000, 1_000_000)

        cur.execute("""
            INSERT INTO staff_contracts (
                staff_id, team_id, start_date, end_date,
                contract_term, amount_usd
            ) VALUES (%s,%s,%s,%s,%s,%s)
        """, (sid, random.randint(1, TARGET["teams"]), start, end, term, amt))

def seed_referees(cur):
    cur.execute("SELECT to_regclass('referees') AS ex")
    if cur.fetchone()["ex"] is None:
        return

    R = TARGET["referees"]
    cur.execute("SELECT COUNT(*) FROM referees")
    existing  = cur.fetchone()["count"]
    to_create = max(0, R - existing)

    for _ in range(to_create):
        role = random.choice(["head","linesman"])
        cur.execute("""
            INSERT INTO referees (first_name, last_name, referee_role)
            VALUES (%s,%s,%s)
        """, (fake.first_name(), fake.last_name(), role))

def seed():
    with cursor() as cur:
        seed_arenas_and_teams(cur)
        seed_users(cur)
        seed_players_and_contracts(cur)
        seed_staff_and_contracts(cur)
        seed_referees(cur)
