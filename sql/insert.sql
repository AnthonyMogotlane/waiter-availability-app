-- Populating weekdays
INSERT INTO weekdays (day) VALUES 
    ('Monday'),
    ('Tuesday'),
    ('Wednesday'),
    ('Thursday'),
    ('Friday'),
    ('Saturday'),
    ('Sunday');

INSERT INTO waiters (firstname, password) VALUES
    ('Anthony', 'anthony123'),
    ('John', 'john123'),
    ('Phumza', 'phumza123'),
    ('Lebalang', 'labalang123'),
    ('Lerato', 'lerato123'),
    ('Kgotso', 'kgotso123'),
    ('Zozo', 'zozo123'),
    ('Shaun', 'shaun123'),
    ('Sofi', 'sofi123');
    ('Dima', 'dima123');

INSERT INTO schedule (day_id, waiter_id) VALUES
    (1, 1),
    (1, 2),
    (1, 3),
    (2, 1),
    (2, 2),
    (2, 3),
    (3, 4),
    (3, 3),
    (3, 10),
    (4, 5),
    (4, 6),
    (4, 1),
    (5, 2),
    (5, 4),
    (5, 8),
    (6, 5),
    (6, 7),
    (6, 6),
    (7, 3),
    (7, 9),
    (7, 8);
  