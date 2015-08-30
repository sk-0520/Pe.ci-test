create table M_NOTE (
    NOTE_ID      integer  primary key,
    CMN_ENABLED  integer  not null,
    CMN_CREATE   text     not null,
    CMN_UPDATE   text     not null,
    NOTE_TITLE   text,
    NOTE_TYPE    integer  not null
)
