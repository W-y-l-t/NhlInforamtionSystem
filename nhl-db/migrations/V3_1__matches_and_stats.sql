CREATE TABLE IF NOT EXISTS matches (
  id SERIAL PRIMARY KEY,
  match_date TIMESTAMP NOT NULL,
  arena_id SMALLINT REFERENCES arenas(id),
  home_team_id SMALLINT REFERENCES teams(id),
  away_team_id SMALLINT REFERENCES teams(id),
  end_type match_end_type,
  match_type match_type,
  home_team_score SMALLINT,
  away_team_score SMALLINT
);

CREATE TABLE IF NOT EXISTS goals (
  id SERIAL PRIMARY KEY,
  match_id INTEGER REFERENCES matches(id) ON DELETE CASCADE,
  scoring_player_id INTEGER REFERENCES players(id),
  assist_player_id_1 INTEGER REFERENCES players(id),
  assist_player_id_2 INTEGER REFERENCES players(id),
  team_id SMALLINT REFERENCES teams(id),
  period SMALLINT,
  time_in_period_sec SMALLINT,
  goal_type goal_type
);

CREATE TABLE IF NOT EXISTS player_match_stats (
  id SERIAL PRIMARY KEY,
  match_id INTEGER REFERENCES matches(id),
  player_id INTEGER REFERENCES players(id),
  penalty_minutes SMALLINT,
  time_on_ice_sec SMALLINT
);

CREATE TABLE IF NOT EXISTS match_referees (
  id SERIAL PRIMARY KEY,
  match_id INTEGER REFERENCES matches(id),
  referee_id INTEGER REFERENCES referees(id)
);