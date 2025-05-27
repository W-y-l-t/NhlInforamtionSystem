#!/usr/bin/env sh

IFS=','

for name in $ANALYST_NAMES
do
  echo "> Creating user $name"
  psql -h "$PGHOST" -p "$POSTGRES_PORT" \
       -U "$POSTGRES_USER" -d "$POSTGRES_DB" \
       -v ON_ERROR_STOP=1 \
       -c "DO \$\$ BEGIN
             CREATE USER $name WITH PASSWORD '${name}_123';
           EXCEPTION WHEN duplicate_object THEN
             RAISE NOTICE 'user exists, skipping';
           END \$\$;" \
       -c "GRANT analytic TO $name;"
done