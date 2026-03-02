# Use a slim, official Postgres image
FROM postgres:16-alpine

# Optional: initialize cluster with UTF-8
ENV POSTGRES_INITDB_ARGS="--encoding=UTF8 --lc-collate=C --lc-ctype=C"

# Copy bootstrap SQL (runs only on first init when data dir is empty)
COPY init.sql /docker-entrypoint-initdb.d/001_init.sql

# Postgres listens on 5432 in the container; map it to 9000 on the host at run-time
EXPOSE 5432
