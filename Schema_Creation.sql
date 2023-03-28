set search_path to public;
drop schema if exists piano_lessons cascade;
create schema piano_lessons;
set search_path to piano_lessons;


create table teacher (
	id 				text primary key,
	name 			text not null
);

create table student (
	id 		text primary key,
	name 	text not null
);

create table appointment (
	id 			serial primary key,
	subject		text not null,
	start_at	timestamp not null,
	end_at 		timestamp not null,
	teacher_id 	text references teacher(id) not null,
	student_id 	text references student(id) not null
);

create table course (
	id 			serial primary key,
	name 		text not null,
	teacher_id	text references teacher(id) not null
);

create table course_invite (
	id			serial primary key,
	course_id	int not null references course(id) ON DELETE CASCADE,
	code		varchar(4) not null,
	expire_date	timestamp not null,
	used		boolean default false
);

create table practice_assignment (
	id 			serial primary key,
	name 		text not null,
	course_id 	int not null references course(id) ON DELETE CASCADE
);

create table student_assignment (
	id 				serial primary key,
	student_id 		text references student(id) not null,
	assignment_id	int not null references practice_assignment(id) ON DELETE CASCADE
);

create table practice_log (
	id 				serial primary key,
	start_time		timestamp not null,
	end_time		timestamp not null,
	student_id 		text references student(id) not null,
	notes			text,
	assignment_id	int not null references practice_assignment(id) ON DELETE CASCADE
);

create table student_course (
	id 			serial primary key,
	course_id 	int not null references course(id) ON DELETE CASCADE,
	student_id 	text references student(id) not null
);

create table payment_history (
	id 			serial primary key,
	teacher_id 	text references teacher(id) not null,
	student_id 	text references student(id) not null,
	amount 		money not null,
	pay_date	timestamp not null default now()
);

create table recording (
	id			serial primary key,
	file_path	text not null,
	created		timestamp not null default NOW(),
	course_id 	int references course(id) ON DELETE SET null,
	student_id 	text references student(id) not null
);

-- INSERT INTO piano_lessons.teacher (id, "name") VALUES(1, 'Bridger');
-- INSERT INTO piano_lessons.student (id, "name") VALUES(1, 'Bob');
-- INSERT INTO piano_lessons.student (id, "name") VALUES(2, 'Anthony');
-- INSERT INTO piano_lessons.student (id, "name") VALUES(3, 'Steve');
-- INSERT INTO piano_lessons.course ("name", teacher_id) VALUES('Piano', 1);
-- INSERT INTO piano_lessons.student_course (id, course_id, student_id) VALUES(1, 1, 1);
-- INSERT INTO piano_lessons.student_course (id, course_id, student_id) VALUES(2, 1, 2);
-- INSERT INTO piano_lessons.student_course (id, course_id, student_id) VALUES(3, 1, 3);
-- INSERT INTO piano_lessons.practice_assignment (id, "name", course_id) VALUES(1, 'Scales', 1);
-- INSERT INTO piano_lessons.practice_log (student_id, start_time, end_time, notes, assignment_id) VALUES(2, '2023-03-16 09:00:00.000', '2023-03-16 10:00:00.000', NULL, 1);
-- INSERT INTO piano_lessons.practice_log (student_id, start_time, end_time, notes, assignment_id) VALUES(2, '2023-03-16 09:00:00.000', '2023-03-16 10:00:00.000', 'Nailed it ;)', 1);
-- INSERT INTO piano_lessons.practice_log (student_id, start_time, end_time, notes, assignment_id) VALUES(1, '2023-03-16 09:00:00.000', '2023-03-16 10:00:00.000', NULL, 1);
-- INSERT INTO piano_lessons.student_assignment (id, student_id, assignment_id) VALUES(1, 1, 1);
-- INSERT INTO piano_lessons.student_assignment (id, student_id, assignment_id) VALUES(2, 2, 1);

