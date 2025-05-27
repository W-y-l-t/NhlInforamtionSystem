MIGRATION = "3.1"

from faker import Faker
import random
from config import TARGET
from db import cursor

fake = Faker()

def seed_matches(cur):
    cur.execute("SELECT COUNT(*) FROM matches")
    existing = cur.fetchone()["count"]
    to_create = max(0, TARGET["matches"] - existing)
    if to_create <= 0:
        return []

    cur.execute("SELECT id FROM teams");   
    teams  = [r["id"] for r in cur.fetchall()]
    cur.execute("SELECT id FROM arenas");  
    arenas = [r["id"] for r in cur.fetchall()]

    created = []
    for _ in range(to_create):
        home, away = random.sample(teams, 2)
        mdate = fake.date_time_between(start_date="-100y", end_date="now")
        cur.execute("""
            INSERT INTO matches (
              match_date, arena_id, home_team_id, away_team_id,
              end_type, match_type, home_team_score, away_team_score
            ) VALUES (%s,%s,%s,%s,%s,%s,0,0)
            RETURNING id
        """, (
            mdate,
            random.choice(arenas),
            home, away,
            random.choice(["regular", "overtime", "shootout"]),
            random.choice(["regular", "playoff", "preseason", "exhibition"])
        ))
        mid = cur.fetchone()["id"]
        created.append((mid, home, away))
    return created

def seed_goals(cur, created):
    scores = {}
    for match_id, home, away in created:
        cur.execute(
            "SELECT player_id FROM player_contracts "
            "WHERE team_id=%s AND end_date >= CURRENT_DATE",
            (home,)
        )
        roster_home = [r["player_id"] for r in cur.fetchall()]

        cur.execute(
            "SELECT player_id FROM player_contracts "
            "WHERE team_id=%s AND end_date >= CURRENT_DATE",
            (away,)
        )
        roster_away = [r["player_id"] for r in cur.fetchall()]

        home_goals  = 0
        away_goals  = 0

        if not roster_home or not roster_away:
            scores[match_id] = (0, 0)
            continue

        for team, roster in ((home, roster_home), (away, roster_away)):
            count = random.randint(0, 6)
            for _ in range(count):
                scorer = random.choice(roster)
                assist_1, assist_2 = random.sample(roster, 2)
                cur.execute("""
                    INSERT INTO goals (
                      match_id, scoring_player_id,
                      assist_player_id_1, assist_player_id_2,
                      team_id, period, time_in_period_sec, goal_type
                    ) VALUES (%s,%s,%s,%s,%s,%s,%s,%s)
                """, (
                    match_id, scorer,
                    assist_1, assist_2,
                    team,
                    random.randint(1, 3),
                    random.randint(0, 20 * 60),
                    random.choice(["even_strength", "power_play", "shorthanded"])
                ))
                if team == home:
                    home_goals += 1
                else:
                    away_goals += 1

        scores[match_id] = (home_goals, away_goals)

    return scores

def seed_player_stats(cur, created):
    for match_id, home_id, away_id in created:
        cur.execute(
            "SELECT COUNT(*) FROM player_match_stats WHERE match_id = %s",
            (match_id,)
        )
        if cur.fetchone()["count"] > 0:
            continue

        cur.execute(
            "SELECT player_id FROM player_contracts "
            "WHERE team_id = %s AND end_date >= CURRENT_DATE",
            (home_id,)
        )
        roster_home = [r["player_id"] for r in cur.fetchall()]

        cur.execute(
            "SELECT player_id FROM player_contracts "
            "WHERE team_id = %s AND end_date >= CURRENT_DATE",
            (away_id,)
        )
        roster_away = [r["player_id"] for r in cur.fetchall()]

        roster = set(roster_home + roster_away)
        if not roster:
            continue

        for pid in roster:
            cur.execute("""
                INSERT INTO player_match_stats (
                  match_id, player_id, penalty_minutes, time_on_ice_sec
                ) VALUES (%s, %s, %s, %s)
            """, (
                match_id,
                pid,
                random.randint(0, 10),
                random.randint(300, 2400)
            ))

def update_match_scores(cur, scores):
    for match_id, (h, a) in scores.items():
        cur.execute("""
            UPDATE matches
               SET home_team_score = %s,
                   away_team_score = %s
             WHERE id = %s
        """, (h, a, match_id))

def seed_match_referees(cur):
    cur.execute("SELECT COUNT(*) FROM match_referees")
    existing = cur.fetchone()["count"]
    to_create = max(0, TARGET["match_referees"] - existing)
    if to_create <= 0:
        return

    cur.execute("SELECT id FROM matches");   
    matches = [r["id"] for r in cur.fetchall()]
    cur.execute("SELECT id FROM referees");  
    refs    = [r["id"] for r in cur.fetchall()]

    for _ in range(to_create):
        cur.execute("""
            INSERT INTO match_referees (match_id, referee_id)
            VALUES (%s,%s)
        """, (
            random.choice(matches),
            random.choice(refs)
        ))

def seed():
    with cursor() as cur:
        created = seed_matches(cur)
        if not created:
            return

        scores = seed_goals(cur, created)

        seed_player_stats(cur, created)
        update_match_scores(cur, scores)
        seed_match_referees(cur)
