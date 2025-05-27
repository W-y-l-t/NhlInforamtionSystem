CREATE TABLE IF NOT EXISTS draft_info (
  id SERIAL PRIMARY KEY,
  player_id INTEGER REFERENCES players(id),
  draft_date DATE,
  overall_pick SMALLINT,
  draft_team_id SMALLINT REFERENCES teams(id)
);

CREATE TABLE IF NOT EXISTS team_awards (
  id SMALLSERIAL PRIMARY KEY,
  team_id SMALLINT REFERENCES teams(id),
  award_id SMALLINT REFERENCES awards(id),
  season VARCHAR(9),
  award_date DATE
);

CREATE TABLE IF NOT EXISTS player_awards (
  id SMALLSERIAL PRIMARY KEY,
  player_id INTEGER REFERENCES players(id),
  award_id SMALLINT REFERENCES awards(id),
  season VARCHAR(9),
  award_date DATE
);