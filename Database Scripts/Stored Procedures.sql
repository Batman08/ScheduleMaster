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

	SELECT ev.EventDataId, ev.Title, ev.Info, ev.[Start], ev.[End], ev.Colour
	FROM [EventData] ev
		INNER JOIN Users u ON ev.UserId = u.UserId
	WHERE u.UserId = 'F5CF2364-5ACF-4360-B9C6-4BC9636B87BF'
	ORDER BY ev.[Start], ev.[End];
END;
GO



-- [spSaveUserEvent]
-- This will create a new event for the user
--------------------------------------------
DROP PROCEDURE IF EXISTS [dbo].[spSaveUserEvent];  
GO  
CREATE PROC [dbo].[spSaveUserEvent] @p_UserId NVARCHAR(256), @p_Title NVARCHAR(256), @p_Info NVARCHAR(256), @p_Start NVARCHAR(256), @p_End NVARCHAR(256), @p_Colour NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @p_EventDataId NVARCHAR(256) = (SELECT NEWID());
	INSERT INTO [EventData] VALUES(@p_EventDataId, @p_UserId, @p_Title, @p_Info, @p_Start, @p_End, @p_Colour);
END;
GO