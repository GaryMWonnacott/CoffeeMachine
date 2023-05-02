drop table if exists dbo.CoffeeMachine_Usage_Option_Log
drop table if exists dbo.CoffeeMachine_Usage_Log
drop table if exists dbo.CoffeeMachine_Option
drop table if exists dbo.CoffeeMachine_ValueType
drop table if exists dbo.CoffeeMachine_Usage_Summary_Daily
drop table if exists dbo.CoffeeMachine_Usage_Summary_Hourly
drop table if exists dbo.CoffeeMachine_ActionType

create table dbo.CoffeeMachine_ActionType (
	ActionTypeId smallint,
	Name nvarchar(255) not null,
	BuildDailySummary bit not null,
	BuildHourlySummary bit not null,
	constraint PK_dbo_CoffeeMachine_ActionType primary key (ActionTypeId)
)

create unique index idx on dbo.CoffeeMachine_ActionType(Name)

insert into dbo.CoffeeMachine_ActionType select 3,'TurnOn',0,0
insert into dbo.CoffeeMachine_ActionType select 1,'TurnOff',0,0
insert into dbo.CoffeeMachine_ActionType select 2,'MakeCoffee',1,1

create table dbo.CoffeeMachine_ValueType (
	ValueTypeId tinyint identity(1,1),
	Description nvarchar(255),
	constraint PK_dbo_CoffeeMachine_ValueType primary key (ValueTypeId)
)

insert into dbo.CoffeeMachine_ValueType select 'bit'
insert into dbo.CoffeeMachine_ValueType select 'int'

create unique index idx on dbo.CoffeeMachine_ValueType(Description)

create table dbo.CoffeeMachine_Option (
	OptionId smallint identity(1,1),
	Description nvarchar(255) not null,
	ValueTypeId tinyint not null,
	constraint PK_dbo_CoffeeMachine_Option primary key (OptionId),
	constraint FK_dbo_CoffeeMachine_Option_dbo_CoffeeMachine_ValueType foreign key (ValueTypeId) references dbo.CoffeeMachine_ValueType(ValueTypeId)
)

insert into dbo.CoffeeMachine_Option 
select 'NumEspressoShots', ValueTypeId from dbo.CoffeeMachine_ValueType where Description = 'int'
insert into dbo.CoffeeMachine_Option 
select 'AddMilk', ValueTypeId from dbo.CoffeeMachine_ValueType where Description = 'bit'

create unique index idx on dbo.CoffeeMachine_Option(Description)

create table dbo.CoffeeMachine_Usage_Log (
	UsageLogId int identity(1,1),
	ActionTypeId smallint not null,
	ActionTimeStamp datetime not null,
	IsSuccess bit not null,
	constraint PK_dbo_CoffeeMachine_Usage_Log primary key (UsageLogId),
	constraint FK_dbo_CoffeeMachine_Usage_Log_dbo_CoffeeMachine_ActionType foreign key (ActionTypeId) references dbo.CoffeeMachine_ActionType(ActionTypeId)
)

create index idx_ActionTimeStamp on dbo.CoffeeMachine_Usage_Log(ActionTypeId, ActionTimeStamp, IsSuccess)

create table dbo.CoffeeMachine_Usage_Option_Log (
	UsageOptionLogId int identity(1,1),
	UsageLogId int,
	OptionId smallint not null,
	BitValue bit null,
	IntValue int null,
	constraint PK_dbo_CoffeeMachine_Usage_Option_Log primary key (UsageOptionLogId),
	constraint FK_dbo_CoffeeMachine_Usage_Option_Log_dbo_CoffeeMachine_Usage_Log foreign key (UsageLogId) references dbo.CoffeeMachine_Usage_Log(UsageLogId),
	constraint FK_dbo_CoffeeMachine_Usage_Option_Log_dbo_CoffeeMachine_Option foreign key (OptionId) references dbo.CoffeeMachine_Option(OptionId)
)

create index idx_ActionType on dbo.CoffeeMachine_Usage_Option_Log(UsageLogId, OptionId)

create table dbo.CoffeeMachine_Usage_Summary_Daily (
	ActionTypeId smallint not null,
	ActionDate date not null,
	WeekDayId tinyint not null,
	MinTime time null,
	MaxTime time null,
	Instances int null
	constraint PK_dbo_CoffeeMachine_Usage_Summary_Daily primary key (ActionDate, WeekDayId, ActionTypeId),
	constraint FK_dbo_CoffeeMachine_Usage_Summary_Daily_dbo_CoffeeMachine_ActionType foreign key (ActionTypeId) references dbo.CoffeeMachine_ActionType(ActionTypeId)
)
create index idx_Time on dbo.CoffeeMachine_Usage_Summary_Daily(MinTime, MaxTime)


create table dbo.CoffeeMachine_Usage_Summary_Hourly (
	ActionTypeId smallint not null,
	ActionDate date not null,
	TimeFrom time not null,
	TimeTo time not null,
	HourDescription as left(cast(TimeFrom as nvarchar),5) + ' - ' + left(cast(TimeTo as nvarchar),5),
	Instances int null,
	constraint PK_dbo_CoffeeMachine_Usage_Summary_Hourly primary key (ActionDate, TimeFrom)
)
create index idx_Time on dbo.CoffeeMachine_Usage_Summary_Hourly(TimeFrom, TimeTo)

go
create or alter procedure dbo.CoffeeMachine_Usage_Summary_Daily_Get(@ActionType nvarchar(255))
as
declare @ActionTypeId int = (select ActionTypeId from dbo.CoffeeMachine_ActionType where Name = @ActionType)

declare @Days table (WeekDayId int not null, WeekDayName varchar(9) not null)
declare @i int = 1, @date datetime

while @i <= 7
begin

	select @date = dateadd(day,@i,'1900-01-01')

	insert into @Days(WeekDayName, WeekDayId)
	select datename(weekday,@date), datepart(weekday,@date)

	select @i+=1

end

select d.WeekDayName WeekDayName,
	isnull(left(cast(a.MinTime as nvarchar),5),'--:--') MinTime,
	isnull(left(cast(a.MaxTime as nvarchar),5),'--:--') MaxTime,
	isnull(a.AvgInstances,0)
from @Days d
	left join (
			select WeekDayId, min(s.MinTime) MinTime, max(s.MaxTime) MaxTime, sum(s.Instances)/count(s.ActionDate) AvgInstances
			from dbo.CoffeeMachine_Usage_Summary_Daily s
			where ActionTypeId = @ActionTypeId
			group by WeekDayId
		) a
		on a.WeekDayId = d.WeekDayId
order by d.WeekDayId
go
create or alter procedure dbo.CoffeeMachine_Usage_Summary_Hourly_Get(@ActionType nvarchar(255))
as
declare @ActionTypeId int = (select ActionTypeId from dbo.CoffeeMachine_ActionType where Name = @ActionType)
declare @Hours table (TimeFrom time not null, TimeDesc varchar(13) not null)
declare @i int = 1, @time time

while @i <= 24
begin

	select @time = cast(dateadd(hour,@i,'1900-01-01') as time)

	insert into @Hours(TimeDesc, TimeFrom)
	select left(cast(@time as nvarchar),5) + ' - ' + left(cast(dateadd(second,-1,dateadd(hour,1,@time)) as nvarchar),5), @time

	select @i+=1

end

select h.TimeDesc, isnull(t.AvgInstances,0)
from @Hours h
	left join (
		select TimeFrom, 
			sum(Instances)/count(ActionDate) AvgInstances
		from dbo.CoffeeMachine_Usage_Summary_Hourly
		where ActionTypeId = @ActionTypeId
		group by TimeFrom
	) t
	on t.TimeFrom = h.TimeFrom
order by h.TimeFrom
go
create or alter procedure dbo.CoffeeMachine_Action_Log (@ActionTypeId int, @ActionOptions nvarchar(1023), @Timestamp datetime, @IsSuccess bit)
as
declare @BuildDailySummary bit,
	@BuildHourlySummary bit

select @BuildDailySummary = BuildDailySummary, 
	@BuildHourlySummary = BuildHourlySummary
from dbo.CoffeeMachine_ActionType
where ActionTypeId = @ActionTypeId

insert into dbo.CoffeeMachine_Usage_Log (ActionTypeId, ActionTimeStamp, IsSuccess)
select @ActionTypeId, @Timestamp, @IsSuccess

declare @UsageLogId int = scope_identity()

if @ActionOptions is not null
begin
	exec dbo.CoffeeMachine_Action_Option_Log @UsageLogId, @ActionOptions
end

if @IsSuccess = 0
begin
	return
end

if @BuildDailySummary = 1
begin
	exec dbo.CoffeeMachine_Usage_Summary_Daily_Update @ActionTypeId, @Timestamp
end

if @BuildHourlySummary = 1
begin
	exec dbo.CoffeeMachine_Usage_Summary_Hourly_Update @ActionTypeId, @Timestamp
end

go
create or alter procedure dbo.CoffeeMachine_Action_Option_Log (@UsageLogId int, @ActionOptions nvarchar(1023))
as
declare @Option table (ActionOption nvarchar(255), Value nvarchar(255))

while len(@ActionOptions) > 0
begin

	if charindex(':',@ActionOptions) = 0
	begin
		raiserror('Incorrect option syntax: %s',11,0,@ActionOptions)
	end

	insert into @Option (ActionOption, Value)
	select left(@ActionOptions,charindex(':',@ActionOptions)-1), substring(@ActionOptions, charindex(':',@ActionOptions)+1, isnull(nullif(charindex('|',@ActionOptions),0),len(@ActionOptions)+1)-charindex(':',@ActionOptions)-1)

	select @ActionOptions = right(@ActionOptions, isnull(nullif(charindex('|', reverse(@ActionOptions)),0),1)-1)

end

insert into dbo.CoffeeMachine_Usage_Option_Log (OptionId, BitValue, IntValue)
select cmo.OptionId, case when vt.Description = 'bit' then o.Value end, case when vt.Description = 'int' then o.Value end
from @Option o
	join dbo.CoffeeMachine_Option cmo
		on cmo.Description = o.ActionOption
	join dbo.CoffeeMachine_ValueType vt
		on vt.ValueTypeId = cmo.ValueTypeId

go 
create or alter procedure dbo.CoffeeMachine_Usage_Summary_Daily_Update (@ActionTypeId int, @Timestamp datetime)
as

merge into dbo.CoffeeMachine_Usage_Summary_Daily t
using (select cast(@TimeStamp as date) ActionDate, cast(@Timestamp as time) TimeStamp) s
	on @ActionTypeId = t.ActionTypeId
	and s.ActionDate = t.ActionDate
when matched then
update set
	MinTime = case when cast(s.Timestamp as time) > isnull(MinTime, '23:59:59') then MinTime else cast(s.Timestamp as time) end,
	MaxTime = case when cast(s.Timestamp as time) < isnull(MaxTime, '00:00:00') then MaxTime else cast(s.Timestamp as time) end,
	Instances = Instances + 1
when not matched then
insert(ActionTypeId, ActionDate, WeekDayId, MinTime, MaxTime, Instances)
values(@ActionTypeId, s.ActionDate, datepart(weekday,s.ActionDate), s.Timestamp, s.Timestamp, 1);


go
create or alter procedure dbo.CoffeeMachine_Usage_Summary_Hourly_Update (@ActionTypeId int, @Timestamp datetime)
as

declare @HourDetail table (ActionDate date, TimeFrom time)

insert into @HourDetail(ActionDate, TimeFrom)
select cast(@TimeStamp as date), cast(dateadd(hour, datepart(hour, @TimeStamp), '1900-01-01') as time)

merge into dbo.CoffeeMachine_Usage_Summary_Hourly t
using @HourDetail s
	on @ActionTypeId = t.ActionTypeId
	and s.ActionDate = t.ActionDate
	and s.TimeFrom = t.TimeFrom
when matched then
update set
	Instances = Instances + 1
when not matched then
insert(ActionTypeId, ActionDate, TimeFrom, TimeTo, Instances)
values(@ActionTypeId, s.ActionDate, s.TimeFrom, dateadd(hour,1,s.TimeFrom), 1);

go
--select * from dbo.CoffeeMachine_Usage_Option_Log  order by 1 desc
--select * from dbo.CoffeeMachine_Usage_Log order by 1 desc
--select * from dbo.CoffeeMachine_Option
--select * from dbo.CoffeeMachine_ValueType
--select * from dbo.CoffeeMachine_Usage_Summary_Daily
--select * from dbo.CoffeeMachine_Usage_Summary_Hourly
--select * from dbo.CoffeeMachine_ActionType

--begin tran

--exec dbo.CoffeeMachine_Action_Log 'Made Coffee',null,'2023-01-01 18:00'

--select * from dbo.CoffeeMachine_Usage_Summary_Daily

--rollback