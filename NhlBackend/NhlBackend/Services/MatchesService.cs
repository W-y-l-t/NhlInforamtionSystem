using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using NhlBackend.Contracts.Services;
using NhlBackend.Extensions;
using NhlBackend.Models.DataTransferObjects;
using Npgsql;

namespace NhlBackend.Services;

public class MatchesService : IMatchesService
{
    private readonly string? _connectionString;
    
    public MatchesService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("NhlDatabase");
    }

    public async Task<MatchDetailsDto> GetMatchDetailsAsync(int matchId, CancellationToken cancellationToken)
    {
        const string sqlHeader = """
                                    WITH base_match AS (
                                    SELECT m.id, m.match_date, 
                                           ht.short_name AS home, at.short_name AS away, a.name AS arena
                                    FROM matches m
                                    JOIN teams ht ON m.home_team_id = ht.id
                                    JOIN teams at ON m.away_team_id = at.id
                                    JOIN arenas a ON m.arena_id = a.id
                                    WHERE m.id = @matchId
                                    ) SELECT * FROM base_match;
                                 """;

            await using var conn = new NpgsqlConnection(_connectionString);
            
            await conn.OpenAsync(cancellationToken);
            
            await using var cmd = new NpgsqlCommand(sqlHeader, conn);
            cmd.Parameters.AddWithValue("matchId", matchId);

            var stopWatch = Stopwatch.StartNew();
            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            stopWatch.Stop();
            
            MetricsRegistry.DbDuration
                .WithLabels("GetMatchDetails")
                .Observe(stopWatch.Elapsed.TotalSeconds);

            if (!await reader.ReadAsync(cancellationToken))
                throw new KeyNotFoundException($"Match {matchId} not found");

            var detail = new MatchDetailsDto(
                reader.GetInt32(reader.GetOrdinal("id")),
                reader.GetDateTime(reader.GetOrdinal("match_date")),
                reader.GetString(reader.GetOrdinal("home")),
                reader.GetString(reader.GetOrdinal("away")),
                reader.GetString(reader.GetOrdinal("arena")),
                new List<GoalDetailsDto>(),
                new List<string>()
            );
            
            await reader.CloseAsync();
            
            const string sqlGoals = """
                                        SELECT g.id AS goal_id,
                                            p1.first_name || ' ' || p1.last_name AS scorer,
                                            p2.first_name || ' ' || p2.last_name AS assist1,
                                            p3.first_name || ' ' || p3.last_name AS assist2,
                                            t.short_name AS team,
                                            g.period, g.time_in_period_sec
                                        FROM goals g
                                        JOIN players p1 ON g.scoring_player_id = p1.id
                                        LEFT JOIN players p2 ON g.assist_player_id_1 = p2.id
                                        LEFT JOIN players p3 ON g.assist_player_id_2 = p3.id
                                        JOIN teams t ON g.team_id = t.id
                                        WHERE g.match_id = @matchId
                                        ORDER BY g.id;
                                    """;
            
            await using var gcmd = new NpgsqlCommand(sqlGoals, conn);
            gcmd.Parameters.AddWithValue("matchId", matchId);
            
            stopWatch.Restart();
            await using var grow = await gcmd.ExecuteReaderAsync(cancellationToken);
            stopWatch.Stop();
            
            MetricsRegistry.DbDuration
                .WithLabels("GetMatchGoals")
                .Observe(stopWatch.Elapsed.TotalSeconds);

            var goals = new List<GoalDetailsDto>();

            while (await grow.ReadAsync(cancellationToken))
            {
                goals.Add(new GoalDetailsDto(
                    grow.GetInt32(grow.GetOrdinal("goal_id")),
                    grow.GetString(grow.GetOrdinal("scorer")),
                    grow.IsDBNull(grow.GetOrdinal("assist1")) ? null : 
                        grow.GetString(grow.GetOrdinal("assist1")),
                    grow.IsDBNull(grow.GetOrdinal("assist2")) ? null : 
                        grow.GetString(grow.GetOrdinal("assist2")),
                    grow.GetString(grow.GetOrdinal("team")),
                    grow.GetInt32(grow.GetOrdinal("period")),
                    grow.GetInt32(grow.GetOrdinal("time_in_period_sec"))
                ));
            }
            
            await grow.CloseAsync();

            const string sqlRefs = """
                                       SELECT r.first_name || ' ' || r.last_name AS referee
                                       FROM match_referees mr
                                       JOIN referees r ON mr.referee_id = r.id
                                       WHERE mr.match_id = @matchId;
                                   """;
            
            await using var rcmd = new NpgsqlCommand(sqlRefs, conn);
            rcmd.Parameters.AddWithValue("matchId", matchId);
            
            stopWatch.Restart();
            await using var rdr = await rcmd.ExecuteReaderAsync(cancellationToken);
            stopWatch.Stop();
            
            MetricsRegistry.DbDuration
                .WithLabels("GetMatchReferees")
                .Observe(stopWatch.Elapsed.TotalSeconds);

            var refs = new List<string>();
            var refOrd = rdr.GetOrdinal("referee");
            while (await rdr.ReadAsync(cancellationToken))
            {
                refs.Add(rdr.GetString(refOrd));
            }

            return detail with { Goals = goals, Referees = refs };
    }
}