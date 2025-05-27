#!/usr/bin/env sh

export PGUSER="${POSTGRES_USER:?POSTGRES_USER is unset}"
export PGPASSWORD="${POSTGRES_PASSWORD:?POSTGRES_PASSWORD is unset}"
export PGDATABASE="${POSTGRES_DB:?POSTGRES_DB is unset}"
export PGHOST="${PGHOST:-${POSTGRES_DB_CONTAINER_NAME:-postgres}}"

STAMP=$(date +%Y%m%d_%H%M%S)
DEST="/backups/${PGDATABASE}_${STAMP}.dump"

pg_dump -Fc -f "$DEST"
echo "[OK] $(date) → $DEST"

cd /backups || exit 1
ls -1tr *.dump 2>/dev/null | head -n "-${BACKUP_RETENTION_COUNT}" | xargs -r rm -f
