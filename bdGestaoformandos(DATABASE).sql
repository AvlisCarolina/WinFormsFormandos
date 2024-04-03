drop database if exists gestaoformandos;
create database gestaoformandos;

use gestaoformandos;

create table Nacionalidade (
	id_nacionalidade int(11) not null auto_increment, 
    alf2 varchar(2) default null unique,
    nacionalidade varchar(100) not null,
    primary key (id_nacionalidade)
);

create table formando (
	id_formando int primary key,
    nome varchar(100) not null,
    morada varchar(100) not null,
    contacto varchar(9) null,
    iban varchar(50) not null default '0',
    sexo char(1) not null,
    data_nascimento date not null,
    id_nacionalidade int,
    foreign key (id_nacionalidade) references Nacionalidade(id_nacionalidade)
);

insert into Nacionalidade (alf2, nacionalidade) values
('PT', 'Portugal'),
('BR', 'Brasil'),
('FR', 'França'),
('ES', 'Espanha');

insert into formando (id_formando, nome, morada, contacto, iban, sexo, data_nascimento, id_nacionalidade) values
(1, 'Ana Silva', 'Coimbra', 916325548, 'PT5000215400215', 'F', '2000-12-31', 1),
(2, 'Maria Alice', 'Braga', 963254888, 'PT5000252245698', 'F', '1995-10-12', 2),
(3, 'Afonso Silva', 'Lisboa', 936325948, 'PT5000215466215', 'M', '1990-02-25', 4);


create table utilizador (
	id_utilizador int auto_increment primary key,
    nome_utilizador varchar(15) not null unique,
    palavra_passe varchar(128) not null,
    userRole varchar(25) not null,
    falhas tinyint default 0,
    estado enum('A', 'I') default 'A'
);

insert into utilizador (nome_utilizador, palavra_passe, userRole) values 
('user1', sha2('user1',512), 'Manager'),
('user2', sha2('user2',512), 'Administrator'),
('user3', sha2('user3',512), 'User');


delimiter //
create procedure pUSuccessLogin (userName varchar(15), result char(1))
begin 
	if (result = 'S') then
		update utilizador set falhas = 0 where nome_utilizador = userName;
	else	
		update utilizador set falhas = falhas + 1 where nome_utilizador = userName;
	end if;
end //
delimiter ;

delimiter //
create trigger tLogin before update on utilizador
for each row
begin
	if (new.falhas = 5) then
		set new.estado = 'I';
	end if;
end //
delimiter ;

create table area (
	id_area			int auto_increment,
    area			varchar(100) not null unique,
    primary key (id_area)
);

create table formador (
	id_formador		int auto_increment,
    nome			varchar(100) not null,
    nif				varchar(9),
    dataNascimento	date,
    id_area			int,
    id_utilizador	int,
    primary key (id_formador),
    foreign key (id_area) references area (id_area)
		on update cascade,
    foreign key (id_utilizador) references utilizador (id_utilizador)
		on update cascade
);

insert into area (area) values
('Programação'),
('Contabilidade'),
('Medicina');

insert into formador (nome, nif, dataNascimento, id_area, id_utilizador) values
('Aurora Alberto', '315400215','2000-12-31', 1, 1),
('Cândido Costa', '245777986', '1995-10-12', 2, 3);

select * from area


