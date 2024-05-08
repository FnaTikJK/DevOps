# Про кубик

Примеры конфигов для кубика лежат в соответсвующей папке. Разница между `persistent` и не `persistent` в том, что `presistent` не обязательно удалять для передеплоя. В нашем случае можно просто забить и писать всё в 1 конфиге

[Вот такая тема есть](https://github.com/mixoil/Deploy-Tutorial/blob/main/README.md) - на ней можно что-то узнать про кубик впринципе. Лучше почитать если ноль инфы
Там же ссылки на видосики обучающие есть

ПОРТЫ приложений можно взять из `docker-compose` или прочитать `Dockerfile-ы`

# Переменные окружения

`YANDEX_USE_CLOUD`
TRUE/FALSE
Определяет, использовать ли Yandex cloud

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
Если подключение падает с ошибкой на `ssl handshake` - поставить `Ssl Mode=Require` в конце строки подключения
Если внутри ВМ Яндекса, то вообще можно не указывать(по идее)


# Переменные которые не надо указывать(вдруг интересно)

`YANDEX_PATH_TO_CERT`
string
Путь до серта Яндекса внутри контейнера бэка
Значение: `Cert/root.crt`
`Уже установлен внутри контейнера`
