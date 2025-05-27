CREATE TABLE IF NOT EXISTS arenas (
  id SMALLSERIAL PRIMARY KEY,
  name VARCHAR(100) NOT NULL,
  address VARCHAR(255) NOT NULL,
  location VARCHAR(100) NOT NULL,
  capacity INTEGER NOT NULL CHECK (capacity > 0)
);

CREATE TABLE IF NOT EXISTS teams (
  id SMALLSERIAL PRIMARY KEY,
  full_name VARCHAR(100) NOT NULL,
  short_name VARCHAR(50) NOT NULL,
  city VARCHAR(50),
  region VARCHAR(50),
  country VARCHAR(50),
  founded_date DATE,
  entry_date DATE,
  logo_url VARCHAR(255),
  home_uniform_url VARCHAR(255),
  away_uniform_url VARCHAR(255),
  division_id SMALLINT REFERENCES divisions(id),
  conference_id SMALLINT REFERENCES conferences(id),
  arena_id SMALLINT REFERENCES arenas(id)
);

CREATE TABLE IF NOT EXISTS players (
  id SERIAL PRIMARY KEY,
  first_name VARCHAR(50) NOT NULL,
  last_name VARCHAR(50) NOT NULL,
  birth_date DATE,
  birth_place VARCHAR(100),
  height_sm SMALLINT CHECK (height_sm > 0),
  weight_kg NUMERIC(3,1) CHECK (weight_kg > 0),
  shot shot_type,
  position player_position
);

CREATE TABLE IF NOT EXISTS player_contracts (
  id SERIAL PRIMARY KEY,
  player_id INTEGER REFERENCES players(id) ON DELETE CASCADE,
  team_id SMALLINT REFERENCES teams(id),
  start_date DATE,
  end_date DATE,
  contract_term SMALLINT,
  amount_usd INTEGER CHECK (amount_usd >= 0)
);

CREATE TABLE IF NOT EXISTS staff (
  id SERIAL PRIMARY KEY,
  first_name VARCHAR(50) NOT NULL,
  last_name VARCHAR(50) NOT NULL,
  birth_date DATE,
  position VARCHAR(50) NOT NULL,
  specialization VARCHAR(50),
  team_id SMALLINT REFERENCES teams(id)
);

CREATE TABLE IF NOT EXISTS staff_contracts (
  id SERIAL PRIMARY KEY,
  staff_id INTEGER REFERENCES staff(id) ON DELETE CASCADE,
  team_id SMALLINT REFERENCES teams(id),
  start_date DATE,
  end_date DATE,
  contract_term SMALLINT,
  amount_usd INTEGER CHECK (amount_usd >= 0)
);

CREATE TABLE IF NOT EXISTS referees (
  id SERIAL PRIMARY KEY,
  first_name VARCHAR(50) NOT NULL,
  last_name VARCHAR(50) NOT NULL,
  referee_role VARCHAR(20) NOT NULL
);

CREATE TABLE IF NOT EXISTS users (
  id SERIAL PRIMARY KEY,
  first_name VARCHAR(50) NOT NULL,
  last_name VARCHAR(50) NOT NULL,
  email VARCHAR(100) NOT NULL UNIQUE,
  password_hash VARCHAR(255) NOT NULL,
  country VARCHAR(50),
  phone VARCHAR(20),
  birth_date DATE
);
