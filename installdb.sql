USE MASTER 
GO 

IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = 'CriChat' OR name = 'CriChat')))
BEGIN
ALTER DATABASE CriChat SET SINGLE_USER WITH ROLLBACK IMMEDIATE
DROP DATABASE CriChat
END

GO



RESTORE DATABASE CriChat
FROM DISK = [calea catre baza de date]'CriChat.bak' -- exemplu 'C:\Temp projects\CriChat\sql\CriChat.bak'
WITH  REPLACE,
MOVE 'CriChat' to 'C:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\CriChat.mdf',
MOVE 'CriChat_log' to 'C:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\CriChat_log.ldf'

GO

ALTER DATABASE CriChat SET ENABLE_BROKER
ALTER DATABASE CriChat SET MULTI_USER

GO

-- SELECT is_broker_enabled FROM sys.databases WHERE name = 'CriChat'