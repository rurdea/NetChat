USE MASTER 
GO 

IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = 'NetChat' OR name = 'NetChat')))
BEGIN
ALTER DATABASE NetChat SET SINGLE_USER WITH ROLLBACK IMMEDIATE
DROP DATABASE NetChat
END

GO



RESTORE DATABASE NetChat
FROM DISK = 'D:\projects\NetChat\src\NetCharDb\NetChat.bak'
WITH  REPLACE,
MOVE 'NetChat' to 'C:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\NetChat.mdf',
MOVE 'NetChat_log' to 'C:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\NetChat_log.ldf'

GO

ALTER DATABASE NetChat SET ENABLE_BROKER
ALTER DATABASE NetChat SET MULTI_USER

GO

-- SELECT is_broker_enabled FROM sys.databases WHERE name = 'NetChat'