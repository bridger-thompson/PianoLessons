set search_path to public;
drop schema if exists piano_lessons cascade;
create schema piano_lessons;
set search_path to piano_lessons;


create table teacher (
	id 		serial primary key,
	name 	text not null
);

create table student (
	id 		serial primary key,
	name 	text not null
);

create table appointment (
	id 			serial primary key,
	subject		text not null,
	start_at	timestamp not null,
	end_at 		timestamp not null,
	teacher_id 	int references teacher(id) not null,
	student_id 	int references student(id) not null
);

create table course (
	id 			serial primary key,
	name 		text not null,
	teacher_id	int references teacher(id) not null
);

create table practice_assignment (
	id 			serial primary key,
	name 		text not null,
	course_id 	int references course(id) not null
);

create table student_assignment (
	id 				serial primary key,
	student_id 		int references student(id) not null,
	assignment_id	int references practice_assignment(id) not null
);

create table practice_log (
	id 				serial primary key,
	start_time		timestamp not null,
	end_time		timestamp not null,
	student_id 		int references student(id) not null,
	duration		interval not null CHECK (duration < '1 day'),
	notes			text,
	assignment_id	int references practice_assignment(id) not null
);

create table student_course (
	id 			serial primary key,
	course_id 	int references course(id) not null,
	student_id 	int references student(id) not null
);

create table payment_history (
	id 			serial primary key,
	teacher_id 	int references teacher(id) not null,
	student_id 	int references student(id) not null,
	amount 		money not null,
	pay_date	timestamp not null default now()
);

INSERT INTO piano_lessons.teacher (id, "name") VALUES(1, 'Bridger');
INSERT INTO piano_lessons.student (id, "name") VALUES(1, 'Bob');
INSERT INTO piano_lessons.student (id, "name") VALUES(2, 'Anthony');
INSERT INTO piano_lessons.student (id, "name") VALUES(3, 'Steve');
INSERT INTO piano_lessons.course (id, "name", teacher_id) VALUES(1, 'Piano', 1);
INSERT INTO piano_lessons.student_course (id, course_id, student_id) VALUES(1, 1, 1);
INSERT INTO piano_lessons.student_course (id, course_id, student_id) VALUES(2, 1, 2);
INSERT INTO piano_lessons.student_course (id, course_id, student_id) VALUES(3, 1, 3);
INSERT INTO piano_lessons.appointment (id, subject, start_at, end_at, teacher_id, student_id) VALUES(1, 'Bob', '2023-03-14 09:00:00.000', '2023-03-14 10:00:00.000', 1, 1);
INSERT INTO piano_lessons.appointment (id, subject, start_at, end_at, teacher_id, student_id) VALUES(2, 'Anthony', '2023-03-16 09:00:00.000', '2023-03-16 10:00:00.000', 1, 2);
INSERT INTO piano_lessons.practice_assignment (id, "name", course_id) VALUES(1, 'Scales', 1);
INSERT INTO piano_lessons.practice_log (log_date, student_id, duration, notes, assignment_id) VALUES('2023-03-12 00:00:00.000', 2, '00:12:00'::interval, NULL, 1);
INSERT INTO piano_lessons.practice_log (log_date, student_id, duration, notes, assignment_id) VALUES('2023-03-13 00:00:00.000', 2, '00:35:00'::interval, 'Nailed it ;)', 1);
INSERT INTO piano_lessons.practice_log (log_date, student_id, duration, notes, assignment_id) VALUES('2023-03-14 00:00:00.000', 1, '00:17:00'::interval, NULL, 1);
INSERT INTO piano_lessons.student_assignment (id, student_id, assignment_id) VALUES(1, 1, 1);
INSERT INTO piano_lessons.student_assignment (id, student_id, assignment_id) VALUES(2, 2, 1);

