USE [ScheduleMaster]
GO

-- [spGetUserEvents]
-- This will get the users list of events
-----------------------------------------
DROP PROCEDURE IF EXISTS [dbo].[spGetUserEvents];  
GO
CREATE PROC [dbo].[spGetUserEvents] @p_UserId NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

	SELECT ev.EventDataId, ev.[Day], ev.Title, ev.Info, ev.[Start], ev.[End], ev.Colour
	FROM [EventData] ev
		INNER JOIN AspNetUsers u ON ev.UserId = u.Id
	WHERE u.Id = @p_UserId
	ORDER BY ev.[Start], ev.[End];
END;
GO



-- [spGetUserEvent]
-- This will get the users event based on EventDataId
-----------------------------------------------------
DROP PROCEDURE IF EXISTS [dbo].[spGetUserEvent];  
GO
CREATE PROC [dbo].[spGetUserEvent] @p_EventDataId NVARCHAR(450), @p_UserId NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

	SELECT ev.EventDataId, ev.[Day], ev.Title, ev.Info, ev.[Start], ev.[End], ev.Colour
	FROM [EventData] ev
		INNER JOIN AspNetUsers u ON ev.UserId = u.Id
	WHERE ev.EventDataId=@p_EventDataId AND u.Id=@p_UserId;
END;
GO



-- [spSaveUserEvent]
-- This will create a new event for the user
--------------------------------------------
DROP PROCEDURE IF EXISTS [dbo].[spSaveUserEvent];  
GO  
CREATE PROC [dbo].[spSaveUserEvent] @p_UserId NVARCHAR(256), @p_Day NVARCHAR(25), @p_Title NVARCHAR(256), @p_Info NVARCHAR(256), @p_Start NVARCHAR(256), @p_End NVARCHAR(256), @p_Colour NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @p_EventDataId NVARCHAR(256) = (SELECT NEWID());
	INSERT INTO [EventData] VALUES(@p_EventDataId, @p_UserId, @p_Day, @p_Title, @p_Info, @p_Start, @p_End, @p_Colour);
END;
GO



-- [spUpdateUserEvent]
-- This will update an existing event
-------------------------------------
DROP PROCEDURE IF EXISTS [dbo].[spUpdateUserEvent];  
GO  
CREATE PROC [dbo].[spUpdateUserEvent] @p_EventDataId NVARCHAR(450), @p_UserId NVARCHAR(256), @p_Day NVARCHAR(25), @p_Title NVARCHAR(256), @p_Info NVARCHAR(256), @p_Start NVARCHAR(256), @p_End NVARCHAR(256), @p_Colour NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

	UPDATE [EventData]
	SET Title=@p_Title, Info=@p_Info, [Start]=@p_Start, [End]=@p_End, Colour=@p_Colour
	WHERE EventDataId=@p_EventDataId AND UserId=@p_UserId;
END;
GO