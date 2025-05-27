MIGRATION = "3.3"

from faker import Faker
import random
from config import TARGET
from db import cursor

fake = Faker()

def seed_seats(cur):
    cur.execute("SELECT to_regclass('seats') AS ex")
    if cur.fetchone()["ex"] is None:
        return

    cur.execute("SELECT COUNT(*) FROM seats")
    existing  = cur.fetchone()["count"]
    to_create = max(0, TARGET["seats"] - existing)

    for _ in range(to_create):
        cur.execute("SELECT id FROM arenas ORDER BY RANDOM() LIMIT 1")
        arena_id   = cur.fetchone()["id"]
        section    = random.choice(["A","B","C","D","E"])
        row        = str(random.randint(1, 50))
        seat_number= random.randint(1, 30)
        cur.execute(
            "INSERT INTO seats (arena_id, section, row, seat_number) VALUES (%s,%s,%s,%s)",
            (arena_id, section, row, seat_number)
        )

def seed_tickets(cur):
    cur.execute("SELECT to_regclass('tickets') AS ex")
    if cur.fetchone()["ex"] is None:
        return

    cur.execute("SELECT COUNT(*) FROM tickets")
    existing  = cur.fetchone()["count"]
    to_create = max(0, TARGET["tickets"] - existing)

    for _ in range(to_create):
        cur.execute("""
            SELECT m.id AS match_id, t.arena_id
            FROM matches m
            JOIN teams t ON m.home_team_id = t.id
            ORDER BY RANDOM()
            LIMIT 1
        """)
        row = cur.fetchone()
        if not row:
            return
        match_id, arena_id = row["match_id"], row["arena_id"]

        cur.execute(
            "SELECT id FROM seats WHERE arena_id=%s ORDER BY RANDOM() LIMIT 1",
            (arena_id,)
        )
        seat = cur.fetchone()
        if not seat:
            return
        seat_id = seat["id"]

        category = random.choice(["Standard", "VIP", "Reduced"])
        price    = round(random.uniform(20, 300), 2)
        status   = random.choice(["available", "reserved", "sold"])

        cur.execute("""
            INSERT INTO tickets (match_id, seat_id, category, price_usd, status)
            VALUES (%s, %s, %s, %s, %s)
        """, (match_id, seat_id, category, price, status))


def seed_orders(cur):
    cur.execute("SELECT to_regclass('orders') AS ex")
    if cur.fetchone()["ex"] is None:
        return

    cur.execute("SELECT COUNT(*) FROM orders")
    existing  = cur.fetchone()["count"]
    to_create = max(0, TARGET["orders"] - existing)

    for _ in range(to_create):
        cur.execute("SELECT id FROM users ORDER BY RANDOM() LIMIT 1")
        user = cur.fetchone()
        if not user:
            return
        user_id = user["id"]

        od = fake.date_time_between(start_date="-30d", end_date="now")
        status = random.choice(["paid","pending"])

        cur.execute("""
            INSERT INTO orders (user_id, order_date, total_amount_usd, order_status)
            VALUES (%s, %s, %s, %s)
            RETURNING id
        """, (user_id, od, 0, status))
        order_id = cur.fetchone()["id"]

        total = 0
        for _ in range(random.randint(1, 5)):
            cur.execute("""
                SELECT id, price_usd FROM tickets
                WHERE status='sold'
                ORDER BY RANDOM() LIMIT 1
            """)
            t = cur.fetchone()
            if not t:
                break
            ticket_id, price = t["id"], t["price_usd"]
            cur.execute("""
                INSERT INTO order_items (order_id, ticket_id, final_price)
                VALUES (%s, %s, %s)
            """, (order_id, ticket_id, price))
            total += float(price)

        cur.execute(
            "UPDATE orders SET total_amount_usd=%s WHERE id=%s",
            (round(total, 2), order_id)
        )

def seed():
    with cursor() as cur:
        seed_seats(cur)
        seed_tickets(cur)
        seed_orders(cur)
