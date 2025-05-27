CREATE INDEX IF NOT EXISTS idx_player_contracts_team_id
    ON player_contracts(team_id);

CREATE INDEX IF NOT EXISTS idx_player_awards_player_id
    ON player_awards(player_id);

CREATE INDEX IF NOT EXISTS idx_player_awards_award_id
    ON player_awards(award_id);


CREATE INDEX IF NOT EXISTS idx_matches_match_date
    ON matches(match_date);

CREATE INDEX IF NOT EXISTS idx_matches_home_away
    ON matches(home_team_id, away_team_id);

CREATE INDEX IF NOT EXISTS idx_goals_match_id
    ON goals(match_id);

CREATE INDEX IF NOT EXISTS idx_goals_team_id
    ON goals(team_id);


CREATE INDEX IF NOT EXISTS idx_orders_user_id
    ON orders(user_id);

CREATE INDEX IF NOT EXISTS idx_order_items_order_id
    ON order_items(order_id);

CREATE INDEX IF NOT EXISTS idx_order_items_ticket_id
    ON order_items(ticket_id);


CREATE INDEX IF NOT EXISTS idx_user_favorite_teams_user
    ON user_favorite_teams(user_id);

CREATE INDEX IF NOT EXISTS idx_user_favorite_players_user
    ON user_favorite_players(user_id);
