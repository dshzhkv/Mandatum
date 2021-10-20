# Mandatum

## Aвторы

Ильичев Матвей

Карпова Юля

Жукова Дарья

Крохин Егор

## Проблема
Для эффективной работы каждый член команды должен знать свои задачи и выполнять их в срок. 
Но есть несколько проблем:
1. Многие задачи в проекте взаимосвязаны. Член команды не может приступить к своей части работы, пока свою задачу не завершил его сокомандник
2. Один человек может участвовать в нескольких проектах, а также иметь свои личные дела. Возникает путаница мз-за большого количества разных задач

Наше приложение решает эти проблемы, предоставляя командам площадку, на которой они могут:
1. распределять задачи между друг другом
2. видеть прогресс своих сокомандников
3. видеть сроки выполнения задачи, и когда они могут к ней приступить
Благодаря этому каждый член команды знает свои задачи и может выполнять их в срок.
Так же пользователь приложения может иметь свои личные доски и доски нескольких проектов, что позволяет ему хранить все свои задачи в одном месте, сопоставлять приоритеты и дедлайны и вовремя выполнять все свои рабочие обязанности, не забывая о личной жизни.

## Сценарии пользования

| Пользователь хочет | Для этого он |
| ------------- | ------------- |
| войти в свой аккаунт в приложении | вводит свои данные и нажимает на кнопку "Войти" |
| создать новую публичную доску | нажимает на кнопку "Создать доску", Выбирает параметр доски: "Публичная" и формат представления, подвержает свой выбор |
| поделиться публичной доской с другими членами команды  | копирует ее идентификатор или ссылку |
| создать новую приватную доску | нажимает на кнопку "Создать доску". Выбирает параметр доски:"Приватная" и формат представления.Подвержает свой выбор |
| присоединиться к существующей доске определенной команды | нажимает на кнопку "Присоединиться к команде" и вводит идентификатор или ссылку доски |
| добавить задачу | нажимает на кнопку "Новая задача" |
| отредактировать задачу (добавить срок выполнения, приоритет, статус (выполнено / не выполнено / в процессе) и др.) | заполняет соответсвующие поля в задаче |
| удалить задачу | в параметрах задачи нажимает на кнопку "Удалить" |


## Компоненты

Приложение строится с использованием слоистой архитектуры

### Infrastructure

* Репозиторий для работы с базой данных
* IFormatable - интерфейс для различных форматов
* IAuthenticatable - интерфейс для различных способов аутентификации
* IStatistics - интерфейс для сбора статистики

### Domain

* class User - сущность пользователя. Хранит ссылки на доски, к которым пользователь имеет доступ. User умеет создавать/удалять/редактировать доски и задачи.
* class Board - сущность доски. Принимает формат IFormatable. Хранит в себе список задач (объектов класса Task)
* class Task - сущность задачи. Хранит в себе какие-то параметры (время, приоритет, статус и т.д.)
* value объекты - параметры задачи
* классы форматов, реализующие интерфейс IFormatable
* классы статистик, реализующие интерфейс IStatistics
* классы аутентификаций, реализующие интерфейс IAuthenticatable

### User Interface

## Точки расширения

1. Форматы представления досок (таблица, граф, календарь и др.) 
    - в первой версии: канбан-доска, граф
2. Различные способы аутентификации
    - в первой версии: аутентифкация через GitHub и Google
4. Статистика (по количеству задач, по количеству затраченных часов и др.)
    - в первой версии: количество выполненных задач