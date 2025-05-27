MIGRATION = "3.4"

import random
from config import TARGET
from db import cursor

def seed_user_favorite_teams(cur):
    cur.execute("SELECT to_regclass('user_favorite_teams') AS ex")
    if cur.fetchone()['ex'] is None:
        return

    cur.execute("SELECT COUNT(*) AS cnt FROM user_favorite_teams")
    existing = cur.fetchone()['cnt']
    to_create = TARGET['user_favorite_teams'] - existing
    if to_create <= 0:
        return

    cur.execute("SELECT id FROM users")
    users = [r['id'] for r in cur.fetchall()]
    cur.execute("SELECT id FROM teams")
    teams = [r['id'] for r in cur.fetchall()]

    for _ in range(to_create):
        cur.execute("""
            INSERT INTO user_favorite_teams (user_id, team_id)
            VALUES (%s, %s)
            ON CONFLICT DO NOTHING
        """, (random.choice(users), random.choice(teams)))

def seed_user_favorite_players(cur):
    cur.execute("SELECT to_regclass('user_favorite_players') AS ex")
    if cur.fetchone()['ex'] is None:
        return

    cur.execute("SELECT COUNT(*) AS cnt FROM user_favorite_players")
    existing = cur.fetchone()['cnt']
    to_create = TARGET['user_favorite_players'] - existing
    if to_create <= 0:
        return

    cur.execute("SELECT id FROM users")
    users = [r['id'] for r in cur.fetchall()]
    cur.execute("SELECT id FROM players")
    players = [r['id'] for r in cur.fetchall()]

    for _ in range(to_create):
        cur.execute("""
            INSERT INTO user_favorite_players (user_id, player_id)
            VALUES (%s, %s)
            ON CONFLICT DO NOTHING
        """, (random.choice(users), random.choice(players)))

def seed():
    with cursor() as cur:
        seed_user_favorite_teams(cur)
        seed_user_favorite_players(cur)
