```
INSERT INTO Areas (Id, Name) VALUES
(1, 'District 1'),
(2, 'District 2'),
(3, 'District 3'),
(4, 'District 4'),
(5, 'District 5'),
(6, 'District 6'),
(7, 'District 7'),
(8, 'District 8'),
(9, 'District 9'),
(10, 'District 10'),
(11, 'District 11'),
(12, 'District 12'),
(13, 'District 13'),
(14, 'District 14'),
(15, 'District 15');
```
```
INSERT INTO public."Orders"(
	"Id", "Weight", "AreaId", "DeliveryTime")
	VALUES
	(uuid_generate_v4(), 5.5, 1, '2025-10-21 09:00:00'),
(uuid_generate_v4(), 7.3, 1, '2025-10-21 09:15:00'),

(uuid_generate_v4(), 3.2, 2, '2025-10-21 09:15:00'),
(uuid_generate_v4(), 8.1, 2, '2025-10-21 09:35:00'),

(uuid_generate_v4(), 4.0, 3, '2025-10-21 09:30:00'),
(uuid_generate_v4(), 9.5, 3, '2025-10-21 09:40:00'),

(uuid_generate_v4(), 6.7, 4, '2025-10-21 08:45:00'),
(uuid_generate_v4(), 7.8, 4, '2025-10-21 08:35:00'),

(uuid_generate_v4(), 2.9, 5, '2025-10-21 09:35:00'),
(uuid_generate_v4(), 5.0, 5, '2025-10-21 10:00:00'),

(uuid_generate_v4(), 4.1, 6, '2025-10-21 09:30:00'),
(uuid_generate_v4(), 7.3, 6, '2025-10-21 09:35:00'),

(uuid_generate_v4(), 6.1, 7, '2025-10-21 09:15:00'),
(uuid_generate_v4(), 4.2, 7, '2025-10-21 09:15:00'),

(uuid_generate_v4(), 8.4, 8, '2025-10-21 09:00:00'),
(uuid_generate_v4(), 2.1, 8, '2025-10-21 09:25:00'),

(uuid_generate_v4(), 3.9, 9, '2025-10-21 09:45:00'),
(uuid_generate_v4(), 5.6, 9, '2025-10-21 09:35:00'),

(uuid_generate_v4(), 7.2, 10, '2025-10-21 09:00:00'),
(uuid_generate_v4(), 4.8, 10, '2025-10-21 09:10:00'),

(uuid_generate_v4(), 2.5, 11, '2025-10-21 09:15:00'),
(uuid_generate_v4(), 6.9, 11, '2025-10-21 08:55:00'),

(uuid_generate_v4(), 4.3, 12, '2025-10-21 09:30:00'),
(uuid_generate_v4(), 8.0, 12, '2025-10-21 09:50:00'),

(uuid_generate_v4(), 7.4, 13, '2025-10-21 09:00:00'),
(uuid_generate_v4(), 5.9, 13, '2025-10-21 08:50:00'),

(uuid_generate_v4(), 3.2, 14, '2025-10-21 09:15:00'),
(uuid_generate_v4(), 9.1, 14, '2025-10-21 09:25:00'),

(uuid_generate_v4(), 6.6, 15, '2025-10-21 09:30:00'),
(uuid_generate_v4(), 2.4, 15, '2025-10-21 09:35:00'),
(uuid_generate_v4(), 6.2, 1, '2025-10-21 10:00:00'),
(uuid_generate_v4(), 8.5, 1, '2025-10-21 12:00:00'),

(uuid_generate_v4(), 4.4, 2, '2025-10-21 10:15:00'),
(uuid_generate_v4(), 9.0, 2, '2025-10-21 13:15:00'),

(uuid_generate_v4(), 5.6, 3, '2025-10-21 09:55:00'),
(uuid_generate_v4(), 7.7, 3, '2025-10-21 12:30:00'),

(uuid_generate_v4(), 6.8, 4, '2025-10-21 10:00:00'),
(uuid_generate_v4(), 5.9, 4, '2025-10-21 12:45:00'),

(uuid_generate_v4(), 7.1, 5, '2025-10-21 11:00:00'),
(uuid_generate_v4(), 8.3, 5, '2025-10-21 12:00:00'),

(uuid_generate_v4(), 3.8, 6, '2025-10-21 09:40:00'),
(uuid_generate_v4(), 6.4, 6, '2025-10-21 13:30:00'),

(uuid_generate_v4(), 4.9, 7, '2025-10-21 09:45:00'),
(uuid_generate_v4(), 7.6, 7, '2025-10-21 12:15:00'),

(uuid_generate_v4(), 6.5, 8, '2025-10-21 09:30:00'),
(uuid_generate_v4(), 9.3, 8, '2025-10-21 12:00:00'),

(uuid_generate_v4(), 8.2, 9, '2025-10-21 11:45:00'),
(uuid_generate_v4(), 3.7, 9, '2025-10-21 12:45:00'),

(uuid_generate_v4(), 7.9, 10, '2025-10-21 09:30:00'),
(uuid_generate_v4(), 5.1, 10, '2025-10-21 14:00:00'),

(uuid_generate_v4(), 6.3, 11, '2025-10-21 11:15:00'),
(uuid_generate_v4(), 8.7, 11, '2025-10-21 08:45:00'),

(uuid_generate_v4(), 7.0, 12, '2025-10-21 11:30:00'),
(uuid_generate_v4(), 4.5, 12, '2025-10-21 12:30:00'),

(uuid_generate_v4(), 9.2, 13, '2025-10-21 08:55:00'),
(uuid_generate_v4(), 2.6, 13, '2025-10-21 12:00:00'),

(uuid_generate_v4(), 3.4, 14, '2025-10-21 09:30:00'),
(uuid_generate_v4(), 8.9, 14, '2025-10-21 12:15:00'),

(uuid_generate_v4(), 7.5, 15, '2025-10-21 10:00:00'),
(uuid_generate_v4(), 5.7, 15, '2025-10-21 12:30:00');
```
