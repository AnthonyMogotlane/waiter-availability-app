-- Populating weekdays
INSERT INTO weekdays (day) VALUES 
    ('Monday'),
    ('Tuesday'),
    ('Wednesday'),
    ('Thursday'),
    ('Friday'),
    ('Saturday'),
    ('Sunday');

INSERT INTO waiters (firstname) VALUES
    ('Anthony'),
    ('John'),
    ('Phumza'),
    ('Lebalang'),
    ('Lerato'),
    ('Phumza'),
    ('Kgotso'),
    ('Zozo'),
    ('Shaun'),
    ('Sofi');

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
  