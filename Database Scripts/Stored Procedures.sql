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
		INNER JOIN Users u ON ev.UserId = u.UserId
	WHERE u.UserId = @p_UserId
	ORDER BY ev.[Start], ev.[End];
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