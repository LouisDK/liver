if exists (select name from sys.databases where name='InoDemo1')
alter database InoDemo1 set single_user with rollback immediate
go
if exists (select name from sys.databases where name='InoDemo1')
drop database InoDemo1