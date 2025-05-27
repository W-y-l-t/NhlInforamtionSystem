MIGRATION = "3.2"

from faker import Faker
import random
import datetime
from config import TARGET
from db import cursor

fake = Faker()

def seed_draft_info(cur):
    cur.execute("SELECT to_regclass('draft_info') AS ex")
    if cur.fetchone()["ex"] is None:
        return

    cur.execute("SELECT COUNT(*) FROM draft_info")
    existing = cur.fetchone()["count"]
    to_create = max(0, TARGET["drafts"] - existing)
    if to_create == 0:
        return

    cur.execute("SELECT id FROM players")
    players = [r["id"] for r in cur.fetchall()]

    for pid in random.sample(players, min(len(players), to_create)):
        ddate = fake.date_between_dates(
            date_start = datetime.date(2000, 1, 1),
            date_end = datetime.date(2024, 1, 1)
        )
        cur.execute("""
            INSERT INTO draft_info (
              player_id, draft_date, overall_pick, draft_team_id
            ) VALUES (%s, %s, %s,
              (SELECT id FROM teams ORDER BY RANDOM() LIMIT 1)
            )
        """, (pid, ddate, random.randint(1, 224)))


def seed_team_awards(cur):
    cur.execute("SELECT to_regclass('team_awards') AS ex")
    if cur.fetchone()["ex"] is None:
        return

    seasons = [f"{y}-{str(y + 1)[-2:]}" for y in range(1915, 2025)]

    cur.execute("SELECT id FROM teams")
    teams = [r["id"] for r in cur.fetchall()]
    cur.execute("SELECT id FROM awards WHERE category='team'")
    team_award_ids = [r["id"] for r in cur.fetchall()]
    if not teams or not team_award_ids:
        return

    for season in seasons:
        for award_id in team_award_ids:
            team_id    = random.choice(teams)
            award_date = fake.date_between_dates(
                date_start=datetime.date(int(season[:4]), 4, 30),
                date_end=datetime.date(int(season[:4]), 6, 30)
            )
            cur.execute("""
                INSERT INTO team_awards (
                  team_id, award_id, season, award_date
                ) VALUES (%s, %s, %s, %s)
                ON CONFLICT DO NOTHING
            """, (team_id, award_id, season, award_date))


def seed_player_awards(cur):
    cur.execute("SELECT to_regclass('player_awards') AS ex")
    if cur.fetchone()["ex"] is None:
        return

    seasons = [f"{y}-{str(y + 1)[-2:]}" for y in range(1915, 2025)]

    cur.execute("SELECT id FROM players")
    players = [r["id"] for r in cur.fetchall()]
    cur.execute("SELECT id FROM awards WHERE category='individual'")
    player_award_ids = [r["id"] for r in cur.fetchall()]
    if not players or not player_award_ids:
        return

    for season in seasons:
        for award_id in player_award_ids:
            player_id  = random.choice(players)
            award_date = fake.date_between_dates(
                date_start=datetime.date(int(season[:4]), 4, 30),
                date_end=datetime.date(int(season[:4]), 6, 30)
            )
            cur.execute("""
                INSERT INTO player_awards (
                  player_id, award_id, season, award_date
                ) VALUES (%s, %s, %s, %s)
                ON CONFLICT DO NOTHING
            """, (player_id, award_id, season, award_date))


def seed():
    with cursor() as cur:
        seed_draft_info(cur)
        seed_team_awards(cur)
        seed_player_awards(cur)
