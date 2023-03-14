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
	log_date		timestamp not null,
	student_id 		int references student(id) not null,
	duration		interval not null,
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



