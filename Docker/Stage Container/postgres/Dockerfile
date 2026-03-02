FROM postgres:latest

COPY init/*.sql /docker-entrypoint-initdb.d/
COPY init/*.csv /docker-entrypoint-initdb.d/