#!/usr/bin/env sh

export PGPASSWORD="$POSTGRES_PASSWORD"
echo "Waiting for PostgreSQL @$POSTGRES_DB_CONTAINER_NAME..."
until psql \
      -h "$PGHOST" \
      -p "$PGPORT" \
      -U "$POSTGRES_USER" \
      -d "$POSTGRES_DB" \
      -c '\q' 2>&1
do
  sleep 1
done

echo "PostgreSQL is ready."

if [ "$APP_ENV" = "dev" ]; then
  echo "Running DB seeder (SEED_COUNT=$SEED_COUNT)..."
  python -u seed.py
else
  echo "APP_ENV=$APP_ENV — seeding skipped."
fi
