CREATE TABLE IF NOT EXISTS seats (
  id SERIAL PRIMARY KEY,
  arena_id SMALLINT REFERENCES arenas(id),
  section VARCHAR(50),
  row VARCHAR(20),
  seat_number SMALLINT
);

CREATE TABLE IF NOT EXISTS tickets (
  id SERIAL PRIMARY KEY,
  match_id INTEGER REFERENCES matches(id),
  seat_id INTEGER REFERENCES seats(id),
  category VARCHAR(30),
  price_usd NUMERIC(7,2),
  status ticket_status
);

CREATE TABLE IF NOT EXISTS orders (
  id SERIAL PRIMARY KEY,
  user_id INTEGER REFERENCES users(id),
  order_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  total_amount_usd NUMERIC(10,2),
  order_status payment_status
);

CREATE TABLE IF NOT EXISTS order_items (
  id SERIAL PRIMARY KEY,
  order_id INTEGER REFERENCES orders(id),
  ticket_id INTEGER REFERENCES tickets(id),
  final_price NUMERIC(7,2)
);