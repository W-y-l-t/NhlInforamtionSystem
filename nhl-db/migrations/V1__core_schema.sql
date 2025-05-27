DO $$ BEGIN
  CREATE TYPE match_end_type AS ENUM ('regular', 'overtime', 'shootout');
EXCEPTION WHEN duplicate_object THEN NULL; END $$;

DO $$ BEGIN
  CREATE TYPE match_type AS ENUM ('regular', 'playoff', 'preseason', 'exhibition');
EXCEPTION WHEN duplicate_object THEN NULL; END $$;

DO $$ BEGIN
  CREATE TYPE award_category AS ENUM ('team', 'individual');
EXCEPTION WHEN duplicate_object THEN NULL; END $$;

DO $$ BEGIN
  CREATE TYPE ticket_status AS ENUM ('available', 'reserved', 'sold');
EXCEPTION WHEN duplicate_object THEN NULL; END $$;

DO $$ BEGIN
  CREATE TYPE payment_status AS ENUM ('paid', 'pending');
EXCEPTION WHEN duplicate_object THEN NULL; END $$;

DO $$ BEGIN
  CREATE TYPE goal_type AS ENUM ('even_strength', 'power_play', 'shorthanded');
EXCEPTION WHEN duplicate_object THEN NULL; END $$;

DO $$ BEGIN
  CREATE TYPE player_position AS ENUM ('goalie', 'defenseman', 'center', 'left_wing', 'right_wing');
EXCEPTION WHEN duplicate_object THEN NULL; END $$;

DO $$ BEGIN
  CREATE TYPE shot_type AS ENUM ('left', 'right');
EXCEPTION WHEN duplicate_object THEN NULL; END $$;

CREATE TABLE IF NOT EXISTS divisions (
  id SMALLSERIAL PRIMARY KEY,
  division_name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS conferences (
  id SMALLSERIAL PRIMARY KEY,
  conference_name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS awards (
  id SMALLSERIAL PRIMARY KEY,
  name VARCHAR(50) NOT NULL UNIQUE,
  category award_category NOT NULL
);
