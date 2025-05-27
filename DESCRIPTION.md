# nhl.com

## Функционалные требования

### 1. Команды

1.1. **Информация о команде**
- Система должна хранить данные о командах НХЛ: 
    - Название (официальное полное и короткое)
    - Город (регион / штат), страна
    - Дата основания, дата вступления в лигу
    - Логотип
    - Домашняя и гостевая форма
    - Принадлежность дивизиону / конференции
    - Трофеи и награды, завоёванные командой

1.2. **Статистика команды**
- Хранение результатов команды за сезон:
    - Количество побед (в основное время / в овертайм / по буллитам)
    - Количество поражений (в основное время / в овертайм / по буллитам)
- Фиксация достижений команды

1.3. **Связи с другими обектами**
- Команда участвует в матчах (должна быть связь с расписанием и результатами)
- У команды есть состав игроков, тренерский штаб и вспомогательный персонал
- Должна быть связь с текущей домашней ареной

### 2. Игроки

2.1. **Информация об игроках**
- Система должна иметь следующие данные об игроках:
    - Фамилия и Имя
    - Дата рождения, место рождения
    - Рост, вес, хват (рабочая рука)
    - Позиция на площадке:
        - Вратарь
        - Защитник
        - Центральный нападающий
        - Правый нападающий
        - Левый нападающий

2.2. **Принадлежность к команде**
- Должна быть информация о текущем контракте игрока

2.3. **Статистика и история выступлений**
- Система должна хранить статистику игрока по каждому матчу:
    - Число голов
    - Число передач
    - Число штрафных минут
    - Время на льду за игру
- Отдельно рассматриваются игры в регулярном чемпионате и в плей-офф

2.4. **Личные награды и достижения**
- Учёт наград, полученных по результатам каждого регулярного чемпионата и плей-офф, информация о дате и сезоне вручения
- Информация об участиях в *NHL All-Star Game*

### 3. Тренерский штаб и вспомогательный персонал

3.1. **Информация о персонале**
- Система должа позволять регистрировать сотрудников помимо игроков:
    - Руководство
    - Тренеры
    - Менеджеры
    - Спортивные врачи
- Для каждого указывается:
    - Фамилия и Имя
    - Дата рождения
    - Должность
    - Специализация - сфера деятельности сотрудника, которая внутри подразделяется на конкретные должности

Примеры специализаций (выделены курсивом) и должностей, к ним относящихся:
- _COACHING STAFF_
    - Head Coach
    - Assistant Coach
    - Goaltending Coach
    - Video Coach
    - Video Coordinator
- _EQUIPMENT_
    - Head Equipment Manager
    - Equipment Managers
    - Assistant Equipment Manager
- _EXECUTIVE MANAGEMENT_
    - Owner/Governor
    - President
    - CEO
    - General Manager
    - Chief Financial Officer
    - Executive Coordinator

3.2. **Принадлежность к команде**
- Должна быть информация о текущей команде сотрудника (одной в текущий момент времени)
- Сведения о контракте сотрудника

### 4. Расписание и проведение матчей

4.1. **Календарь игр**
- Должно веститсь расписание сезона НХЛ, для каждого матча:
    - Дата
    - Время
    - Место проведения
    - Команды противники
    - Судейский состав
- Система должна отображать:
    - Матчи регулярного чемпионата
    - Матчи плей-офф
    - Предсезонные матчи
    - Выставочные матчи

4.2. **Статистика по матчам**
- Для каждого матча система должна хранить результаты:
    - Итоговый счёт
    - Когда завершилась игра (основное время / овертайм / буллиты)
- Сбор игровой статистики в рамках целого матча (основное время + овертайм, если проводится):
    - Голы
    - Передачи
    - Штрафное время игроков
    - Удары в створ ворот
    - Сейвы вратарей

### 5. Арены

5.1. **Информация об аренах**
- Для каждой арены, которые используются или использовались для проведения матчей НХЛ, должна храниться следующая информация:
    - Название
    - Адрес
    - Местоположение
    - Вместимость
    - Клуб, для которого арена - домашняя

- Соответственно, корректные данные про одну арену могут выглядеть так:
    - Amalie Arena
    - 401 Channelside Drive
    - Tampa, Florida, United States
    - 19,092
    - Tampa Bay Lightning

### 6. Драфты и контракты

6.1. **Драфты**
- Должна храниться следующая информация для каждого игрока:
    - Дата драфта
    - Общий номер
    - Драфтовавшая команда

6.2. **Контракты**
- Должна храниться следующая информация для каждого игрока и сотрудника:
    - Начало и окончание контракта (даты)
    - Срок контрактка
    - Сумма контракта
    - Команда, с которой заключён контракт

### 7. Судейство

7.1. **Информация о судьях**
- Для каждого судьи должна быть указана следующая информация:
    - Фамилия и Имя
    - Роль судьи на площадке (главный / линейный)
- Необходима связь между судьёй и играми чемпионата, в которых он принимал участие

### 8. Структура чемпионата

8.1. **Информация о дивизионах**
- Кажая команда-участник регалярного чемпионата принадлежит строго одному из дивизионов:
    - Atlantic
    - Metropolitan
    - Central
    - Pacific
- Кажая команда-участник регалярного чемпионата принадлежит строго одной из конференций:
    - Eastern
    - Western

### 9. Награды

9.1. **Командные награды**
- Для каждого сезона должна быть информация об обладателях следующих командных наград:
    - Кубок Стэнли
    - Приз принца Уэльского
    - Приз Кларенса Кэмпбелла
    - Президентский кубок

9.2. **Индивидуальные награды**
- Для каждого сезона должна быть информация об обладателях следующих индивидуальных наград:
    - Харт Трофи
    - Везина Трофи
    - Колдер Трофи
    - Арт Росс Трофи
    - Конн Смайт Трофи
    - Тед Линдсей Эворд

### 10. Взаимодействие с пользователями

10.1. **Регистрация пользователей**
- Система должна позволять новым пользователям регистрироваться. При этом пользователь указывает:
    - Фамилия и Имя
    - Электронная почта
    - Пароль
    - Страна проживания

10.2. **Аутентификация пользователей**
- Зарегистрированные пользователи должны иметь возможность войти в систему, вводя электронную почту и пароль.

10.3. **Управление профилем**
- Пользователь может редактировать свой профиль, в том числе добавлять информацию о себе:
    - До 5 любимых команд
    - До 15 любимых игроков
    - Номер телефона
    - Дата рождения

### 11. Покупка билетов

11.1. **Выбор места**
- Пользователь должен иметь возможность приобрести билет на любой матч.
- Система должа:
    - Отображать доступные к покупке места для выбранного матча.
    - Позволять пользователью видеть стоимость выбранного места

11.2. **Виды билетов**
- Система должна предоставлять пользователю возможность приобретать билеты разных категорий:
    - Стандартный
    - VIP
    - Льготный (детский / студенческий)
    - Абонемент

11.3. **Информация по заказу**
- Система должна сохранять информацию по каждому купленному или забронированному билету (в одном заказе может быть несколько билетов):
    - ID пользователя
    - Дата заказа
    - Вид билета
    - Место на арене
    - Итоговая сумма заказа
    - Статус (оплачен или нет)


## Глоссарий

- **Овертайм** — дополнительное время, которое назначается для выявления победителя по окончании трёх периодов при равном счёте в хоккейном матче.

- После безрезультатного пятиминутного овертайма в НХЛ проводится **серия буллитов**. В этом случае три игрока от каждой команды делают броски в порядке, определённом тренерами команд.

    Правила проведения **буллитов**:

    - Каждая команда делает по три броска.
    - Победителем игры становится команда, забросившая большее количество голов.
    - Если после трёх бросков сохраняется ничья, буллиты продолжаются до первой заброшенной шайбы.
    - Вне зависимости от количества голов, заброшенных в серии буллитов, в финальном счёте команда, выигравшая по буллитам, получает на один гол больше соперника. 

- **Регулярный чемпионат НХЛ** — первый этап сезона Национальной хоккейной лиги (НХЛ). В рамках регулярного чемпионата каждая команда проводит 82 матча: 41 игру дома и 41 — в гостях.

- **Плей-офф НХЛ** — решающая стадия хоккейного сезона, на которой команды играют друг с другом до четырёх побед. Победитель получает главный трофей НХЛ — Кубок Стэнли.

    В плей-офф выходят по три лучшие команды из каждого дивизиона, а также по две команды из каждой конференции, которые добираются по очкам.

- **Драфт НХЛ** — ежегодное мероприятие в Национальной хоккейной лиге, во время которого клубы получают права на молодых хоккеистов, удовлетворяющих определённым критериям отбора.

- **Награды**
    - *Командные*
        - Кубок Стэнли (Stanley Cup) - Награждается команда, выигравшая финальную серию плей-офф
        - Приз принца Уэльского (Prince of Wales Trophy) - Награждается команда-чемпион Восточной конференции
        - Приз Кларенса Кэмпбелла (Clarence S. Campbell Bowl) - Награждается команда-чемпион Западной конференции
        - Президентский кубок (Presidents' Trophy) - Награждается команда, набравшая наибольшее количество очков в регулярном чемпионате
    - *Индивидуальные*
        - Харт Трофи (Hart Memorial Trophy) - Награждается игрок, который внёс наибольший вклад в успехи своей команды в регулярном чемпионате
        - Везина Трофи (Vezina Trophy) - Награждается голкипер, сыгравший в регулярном чемпионате не менее 25 матчей и продемонстрировавший лучшую игру среди всех конкурентов
        - Колдер Трофи (Calder Memorial Trophy) - 	Награждается игрок, наиболее ярко проявивший себя среди тех, кто проводит первый полный сезон в составе клуба НХЛ
        - Арт Росс Трофи (Art Ross Trophy) - Награждается игрок, набравший наибольшее количество очков по системе гол+пас в регулярном чемпионате
        - Конн Смайт Трофи (Conn Smythe Trophy) - Награждается игрок, лучше других зарекомендовавший себя в играх плей-офф.
        - Тед Линдсей Эворд (Ted Lindsay Award) -
        Награждается игрок, лучший по мнению самих хоккеистов


## Entity Relation Diagram

```plantuml

entity "teams" {
  *id : smallserial
  --
  full_name : varchar(100)
  short_name : varchar(50)
  city : varchar(50)
  region : varchar(50)
  country : varchar(50)
  founded_date : date
  entry_date : date
  logo_url : varchar(255)
  home_uniform_url : varchar(255)
  away_uniform_url : varchar(255)
  division_id : smallint <<FK>>
  conference_id : smallint <<FK>>
  arena_id : smallint <<FK>>
}

entity "divisions" {
  *id : smallserial
  --
  division_name : varchar(50)
}

entity "conferences" {
  *id : smallserial
  --
  conference_name : varchar(50)
}

entity "team_awards" {
  *id : smallserial
  --
  team_id : smallint <<FK>>
  award_id : smallint <<FK>>
  season : varchar(9)
  award_date : date
}

entity "players" {
  *id : serial
  --
  first_name : varchar(50)
  last_name : varchar(50)
  birth_date : date
  birth_place : varchar(100)
  height_sm : smallint
  weight_kg : numeric(3,1)
  shot : varchar(10)
  position : player_position
}

entity "player_contracts" {
  *id : serial
  --
  player_id : integer <<FK>>
  team_id : smallint <<FK>>
  start_date : date
  end_date : date
  contract_term : smallint
  amount_usd : integer
}

entity "goals" {
  *id : serial
  --
  match_id : integer <<FK>>
  scoring_player_id : integer <<FK>>
  assist_player_id_1 : integer <<FK>> [nullable]
  assist_player_id_2 : integer <<FK>> [nullable]
  team_id : smallint <<FK>>
  period : smallint
  time_in_period_sec : smallint
  goal_type : goal_type
}

entity "player_match_stats" {
  *id : serial
  --
  match_id : integer <<FK>>
  player_id : integer <<FK>>
  penalty_minutes : smallint
  time_on_ice_sec : smallint
}

entity "player_awards" {
  *id : smallserial
  --
  player_id : integer <<FK>>
  award_id : smallint <<FK>>
  season : varchar(9)
  award_date : date
}

entity "draft_info" {
  *id : serial
  --
  player_id : integer <<FK>>
  draft_date : date
  overall_pick : smallint
  draft_team_id : smallint <<FK>>
}

entity "staff" {
  *id : serial
  --
  first_name : varchar(50)
  last_name : varchar(50)
  birth_date : date
  position : varchar(50)
  specialization : varchar(50)
  team_id : smallint <<FK>>
}

entity "staff_contracts" {
  *id : serial
  --
  staff_id : integer <<FK>>
  team_id : smallint <<FK>>
  start_date : date
  end_date : date
  contract_term : smallint
  amount_usd : integer
}

entity "matches" {
  *id : serial
  --
  match_date : timestamp
  arena_id : smallint <<FK>>
  home_team_id : smallint <<FK>>
  away_team_id : smallint <<FK>>
  end_type : match_end_type
  match_type : math_type
  home_team_score : smallint
  away_team_score : smallint
}

entity "arenas" {
  *id : smallserial
  --
  name : varchar(100)
  address : varchar(255)
  location : varchar(100)
  capacity : integer
}

entity "match_referees" {
  *id : serial
  --
  match_id : integer <<FK>>
  referee_id : integer <<FK>>
}

entity "referees" {
  *id : serial
  --
  first_name : varchar(50)
  last_name : varchar(50)
  referee_role : varchar(20)
}

entity "awards" {
  *id : smallserial
  --
  name : varchar(50)
  category : award_category
}

entity "users" {
  *id : serial
  --
  first_name : varchar(50)
  last_name : varchar(50)
  email : varchar(100)
  password_hash : varchar(255)
  country : varchar(50)
  phone : varchar(20)
  birth_date : date
}

entity "user_favorite_teams" {
  *user_id : integer <<FK>>
  *team_id : smallint <<FK>>
}

entity "user_favorite_players" {
  *user_id : integer <<FK>>
  *player_id : integer <<FK>>
}

entity "seats" {
  *id : serial
  --
  arena_id : smallint <<FK>>
  section : varchar(50)
  row : varchar(20)
  seat_number : smallint
}

entity "tickets" {
  *id : serial
  --
  match_id : integer <<FK>>
  seat_id : integer <<FK>>
  category : varchar(30)
  price_usd : numeric(7,2)
  status : ticket_status
}

entity "orders" {
  *id : serial
  --
  user_id : integer <<FK>>
  order_date : timestamp
  total_amount_usd : numeric(10,2)
  order_status : payment_status
}

entity "order_items" {
  *id : serial
  --
  order_id : integer <<FK>>
  ticket_id : integer <<FK>>
  final_price : numeric(7,2)
}

matches::home_team_id }|-l-|| teams::id
matches::away_team_id }|-l-|| teams::id
player_contracts }|-d-|| teams : "team_id"
player_contracts }|-l-|| players : "player_id"
teams }o--d-|| divisions : "division_id"
teams }o--d-|| conferences : "conference_id"
matches }|-d-|| arenas : "arena_id"
teams }o-d-|| arenas : "arena_id"
draft_info }o-l-|| players : "player_id"
draft_info }o-d-|| teams : "draft_team_id"
team_awards }o-r-|| teams : "team_id"
player_awards }o-r-|| players : "player_id"
team_awards }o-l-|| awards : "award_id"
player_awards }o-d-|| awards : "award_id"
staff }o--u--|| teams : "team_id"
staff_contracts }|-l-|| staff : "staff_id"
staff_contracts }|--u--|| teams : "team_id"
player_match_stats }|--l--|| players : "player_id"
player_match_stats }|--d--|| matches : "match_id"
user_favorite_players }o---r---|| players : "player_id"
user_favorite_players }o---r---|| users : "user_id"
user_favorite_teams }o---u---|| users : "user_id"
user_favorite_teams }o---d---|| teams : "team_id"
tickets }|-l-|| matches : "match_id"
tickets }|-d-|| seats : "seat_id"
seats::arena_id }|-l-|| arenas::id
order_items::ticket_id }|-l-|| tickets::id
order_items::order_id }|-r-|| orders::id
match_referees::match_id }|--l--|| matches::id
match_referees::referee_id }|-r-|| referees::id
goals }|-d-|| matches : "match_id"
goals::scoring_player_id }|-l-|| players::id
goals::assist_player_id_1 }o-l-|| players::id
goals::assist_player_id_2 }o-l-|| players::id
goals }|-d-|| teams : "team_id"

```