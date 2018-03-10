USE sql_profile_log
go
CREATE TABLE dbo.heartbeat
(
    [timestamp] datetime NOT NULL
)
go
IF OBJECT_ID(N'dbo.heartbeat') IS NOT NULL
    PRINT N'<<< CREATED TABLE dbo.heartbeat >>>'
ELSE
    PRINT N'<<< FAILED CREATING TABLE dbo.heartbeat >>>'
go
CREATE TABLE dbo.statement_history
(
    statement_history_id int          IDENTITY,
    LoginName            varchar(128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    HostName             varchar(128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    ServerName           varchar(128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    DatabaseName         varchar(128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    RowCounts            int          NULL,
    Error                int          NULL,
    CPU                  int          NULL,
    Reads                int          NULL,
    Writes               int          NULL,
    Duration             int          NULL,
    SPID                 int          NULL,
    StartTime            datetime     NULL,
    EndTime              datetime     NULL,
    DBID                 int          NULL,
    TextData             varchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    create_dt            datetime     CONSTRAINT DF__statement__creat__164452B1 DEFAULT getdate() NOT NULL,
    create_suser_sname   varchar(128) COLLATE SQL_Latin1_General_CP1_CI_AS CONSTRAINT DF__statement__creat__173876EA DEFAULT suser_sname() NOT NULL,
    create_app_user_id   varchar(128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    modify_dt            datetime     CONSTRAINT DF__statement__modif__182C9B23 DEFAULT getdate() NOT NULL,
    modify_suser_sname   varchar(128) COLLATE SQL_Latin1_General_CP1_CI_AS CONSTRAINT DF__statement__modif__1920BF5C DEFAULT suser_sname() NOT NULL,
    modify_app_user_id   varchar(128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    CONSTRAINT PK_statement_history
    PRIMARY KEY NONCLUSTERED (statement_history_id)
)
go
IF OBJECT_ID(N'dbo.statement_history') IS NOT NULL
    PRINT N'<<< CREATED TABLE dbo.statement_history >>>'
ELSE
    PRINT N'<<< FAILED CREATING TABLE dbo.statement_history >>>'
go
