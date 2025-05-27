import os
from dotenv import load_dotenv

load_dotenv()

APP_ENV = os.getenv("APP_ENV", "dev")
SEED_COUNT = int(os.getenv("SEED_COUNT", "32"))
MIGRATION_VERSION = os.getenv("MIGRATION_VERSION", "latest")

DB = dict(
    host=os.getenv("PGHOST", "haproxy"),
    port=os.getenv("PGPORT", 5001),
    db=os.getenv("POSTGRES_DB"),
    user=os.getenv("POSTGRES_USER", "postgres"),
    password=os.getenv("POSTGRES_PASSWORD")
)

TARGET = dict(
    teams=SEED_COUNT,
    arenas=SEED_COUNT,
    divisions=SEED_COUNT // 8,
    conferences=SEED_COUNT // 16,
    awards=SEED_COUNT // 4,
    users=SEED_COUNT * 202,
    players=SEED_COUNT * 30,
    staff=SEED_COUNT * 52,
    matches=SEED_COUNT * 200,
    orders=SEED_COUNT * 100,
    drafts=SEED_COUNT * 33,
    seats=SEED_COUNT * 200,
    tickets=SEED_COUNT * 152,
    referees=SEED_COUNT * 64,
    match_referees=SEED_COUNT * 48,
    user_favorite_teams=SEED_COUNT * 4,
    user_favorite_players=SEED_COUNT * 8,
)