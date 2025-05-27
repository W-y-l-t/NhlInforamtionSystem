import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
  vus: 200,
  duration: '20m',
  thresholds: {
    'http_req_failed': ['rate<0.01']
  }
};

const urls = [
  { u: 'http://nhl_backend:80/players/top-snipers/2023/50',  t: 'TopSnipers' },
  { u: 'http://nhl_backend:80/players/of-team/15',           t: 'PlayersOfTeam' },
  { u: 'http://nhl_backend:80/players/awards-of/42',         t: 'PlayerAwards' },
  { u: 'http://nhl_backend:80/teams/all',                    t: 'TeamsAll' },
  { u: 'http://nhl_backend:80/users/tickets-of/17',          t: 'UserTickets' },
  { u: 'http://nhl_backend:80/matches/details/32',           t: 'MatchDetails' },
  { u: 'http://nhl_backend:80/teams/performance/15/2023',    t: 'TeamPerformance' },
  { u: 'http://nhl_backend:80/users/favorites/17',           t: 'UserFavorites' }
];

export default function () {
  urls.forEach(({u,t}) => http.get(u, { tags: { name: t } }));
  sleep(1);
}
