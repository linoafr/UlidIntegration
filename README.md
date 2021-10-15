This project help using the excellent library `Ulid` made by `Cysharp`

The repository can be found here : [Cysharp/Ulid on github](https://github.com/Cysharp/Ulid)

Before running unit tests, run the following commands at the project root directory.

Servers are not running on standard port in order to avoid port collisions :

SQL Server is running on port : 11433
Postgre Sql is running on port: 15432

Start db servers

```console
docker compose up -d
```

Wait a few seconds in order to have db servers up and ready.

When done stop the servers

```console
docker compose stop
```

And remove the containers

```console
docker compose rm --force
```
