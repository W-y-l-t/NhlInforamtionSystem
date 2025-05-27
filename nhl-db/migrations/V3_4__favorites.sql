CREATE TABLE IF NOT EXISTS user_favorite_teams (
  user_id INTEGER REFERENCES users(id),
  team_id SMALLINT REFERENCES teams(id),
  PRIMARY KEY (user_id, team_id)
);

CREATE TABLE IF NOT EXISTS user_favorite_players (
  user_id INTEGER REFERENCES users(id),
  player_id INTEGER REFERENCES players(id),
  PRIMARY KEY (user_id, player_id)
);