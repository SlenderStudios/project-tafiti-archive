use Tafiti;

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[shelf]') and OBJECTPROPERTY(id, N'IsTable') = 1)
begin
	drop table [dbo].[shelf]
	drop table [dbo].[snapshots]
end