CREATE TABLE IF NOT EXISTS weekdays(
    id serial PRIMARY KEY,
    day VARCHAR(50) NOT NULL UNIQUE
);

-- Populating weekdays
INSERT INTO weekdays (day) VALUES 
    ('Monday'),
    ('Tuesday'),
    ('Wednesday'),
    ('Thursday'),
    ('Friday'),
    ('Saturday'),
    ('Sunday');

CREATE TABLE IF NOT EXISTS waiters (
    id serial PRIMARY KEY,
    firstname VARCHAR(50) NOT NULL,
    password VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS schedule (
    id serial PRIMARY KEY,
    day_id int NOT NULL,
    waiter_id int NOT NULL,
    FOREIGN KEY (day_id) REFERENCES weekdays(id),
    FOREIGN KEY (waiter_id) REFERENCES waiters(id)
);