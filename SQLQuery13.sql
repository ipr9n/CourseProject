USE [MYCOUS_ae2e080f4fa043f483e292777e9ab65d]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[FullTextSearch]
		@text = N'asdfsdfasdfa'

SELECT	@return_value as 'Return Value'

GO
