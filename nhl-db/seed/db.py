import psycopg2, psycopg2.extras
from contextlib import contextmanager
from config import DB

conn = psycopg2.connect(
    dbname=DB["db"], user=DB["user"], password=DB["password"],
    host=DB["host"], port=DB["port"]
)
conn.autocommit = True

@contextmanager
def cursor():
    with conn.cursor(cursor_factory=psycopg2.extras.RealDictCursor) as cur:
        yield cur
