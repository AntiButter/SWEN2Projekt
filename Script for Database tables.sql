create table tour
(
    tourid              serial
        constraint tour_pk
            primary key,
    name                varchar not null,
    description         varchar,
    fromdb              varchar not null,
    todb                varchar not null,
    transporttype       varchar not null,
    distance            double precision,
    time                varchar,
    popularity          integer,
    "childFriendliness" integer
);

alter table tour
    owner to postgres;

create table tourlogs
(
    logid      integer   default nextval('"tourLogs_logID_seq"'::regclass) not null
        constraint tourlogs_pk
            primary key,
    logtime    timestamp default CURRENT_TIMESTAMP                         not null,
    comment    varchar,
    difficulty integer                                                     not null,
    totaltime  integer                                                     not null,
    rating     integer                                                     not null,
    touridfk   integer
        constraint tourlogs_tour_tourid_fk
            references tour
            on update cascade on delete cascade
);

alter table tourlogs
    owner to postgres;


