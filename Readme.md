# Переменные окружения

`YANDEX_USE_CLOUD`
TRUE/FALSE
Определяет, использовать ли Yandex cloud

`YANDEX_PATH_TO_CERT`
string
Путь до серта Яндекса внутри контейнера бэка
Значение: `./Cert/root.crt`

`AWS_ACCESS_KEY`
string
API-ключ от сервисного аккаунта

`AWS_SECRET_KEY`
string
Секрет от ключа сервисного аккаунта

`AWS_BUCKET_NAME`
string
Название Бакета в Object storage

`DATABASE_CONNECTION`
string
Строка подключения к базе данных
Для яндекса - берётся вверху на странице управления кластера Postgres - `Подключиться`
