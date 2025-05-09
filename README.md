Параметры конфигурации:

1. Logging (Настройки логирования)
   
ApplicationName - Название приложения в логах

DefaultLogLevel - Уровень логирования по умолчанию (Information, Debug, Warning, Error)

AspNetCoreLogLevel - Уровень логирования для ASP.NET Core

ConsoleLogLevel - Уровень логирования в консоль

FileLogLevel - Уровень логирования в файл

ElasticSearchLogLevel - Уровень логирования в Elasticsearch

FilePath - Путь и шаблон имени лог-файлов (например, logs/log-.json)

RetainedFileCountLimit - Макс. количество хранимых лог-файлов (создается 1 раз в день)

ElasticIndexFormat - Префикс названия индекса в Elasticsearch

ElasticSearchNumberOfShards - Количество шардов в Elasticsearch

ElasticSearchNumberOfReplicas - Количество реплик в Elasticsearch

2. ConnectionStrings (Строки подключения)
   
DeliveryDbContext - Подключение к PostgreSQL для основного контекста БД

3. ElasticConfiguration (Elasticsearch)
   
Uri - Адрес сервера Elasticsearch

4. OrderSettings (Настройки заказов)
   
MaxTotalWeight - Максимальный допустимый вес заказа (в граммах)

5. DataGenerationSettings (Генерация тестовых данных)
   
RegionsCount - Количество регионов для генерации

DeliveryPersonsCount - Количество курьеров

SuppliersCount - Количество поставщиков

CustomersCount - Количество клиентов

OrdersCount - Количество тестовых заказов

6. BatchProcessing (Пакетная обработка)

BatchSize - Размер пачки для дальнейшего сохранения в БД

7. Uploads (Загрузки)
   
RootPath - Корневая директория для загружаемых файлов

8. AllowedHosts
    
Разрешённые хосты для CORS (значение * разрешает все)

---

Настройка логирования

Elasticsearch
1. Скачивание и установка
Перейдите на официальный сайт Elasticsearch (https://www.elastic.co/downloads/elasticsearch) и скачайте подходящую версию для вашей операционной системы.
Распакуйте архив в удобное место на вашем компьютере.
2. Запуск Elasticsearch
Windows: Откройте командную строку, перейдите в каталог bin внутри распакованной директории Elasticsearch и запустите elasticsearch.bat командой elasticsearch.bat.
3. Проверка запуска
Откройте браузер и перейдите по адресу http://localhost:9200. Если Elasticsearch запустился успешно, вы увидите информацию о кластере в формате JSON.

Kibana
1. Скачивание и установка
Перейдите на официальный сайт Kibana (https://www.elastic.co/downloads/kibana) и скачайте подходящую версию для вашей операционной системы.
Распакуйте архив в удобное место на вашем компьютере.
2. Настройка Kibana
Откройте файл config/kibana.yml в текстовом редакторе.
Убедитесь, что параметр elasticsearch.hosts указывает на адрес Elasticsearch (обычно http://localhost:9200).
3. Запуск Kibana
Windows: Откройте командную строку, перейдите в каталог bin внутри распакованной директории Kibana и запустите kibana.bat командой kibana.bat.
4. Проверка запуска
Откройте браузер и перейдите по адресу http://localhost:5601. Если Kibana запустился успешно, откроется его веб - интерфейс.
5. Просмотр логов
Перейдите во вкладку Discover
