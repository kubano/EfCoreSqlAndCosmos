﻿-- SQL script file to add SQL code to improve performance
-- I have built this as an Idempotent Script, that is, it can be applied even if there isn't a change and it will ensure the database is up to date

IF OBJECT_ID('dbo.AuthorsStringUdf') IS NOT NULL
	DROP FUNCTION dbo.AuthorsStringUdf
GO

CREATE FUNCTION AuthorsStringUdf (@bookId uniqueidentifier)
RETURNS NVARCHAR(4000)
AS
BEGIN
-- Thanks to https://stackoverflow.com/a/194887/1434764
DECLARE @Names AS NVARCHAR(4000)
SELECT @Names = COALESCE(@Names + ', ', '') + a.Name
FROM Authors AS a, Books AS b, BookAuthor AS ba 
WHERE ba.BookId = @bookId
      AND ba.AuthorId = a.AuthorId 
	  AND ba.BookId = b.BookId
ORDER BY ba.[Order]
RETURN @Names
END
GO

IF OBJECT_ID('dbo.AverageVotesUdf', N'FN') IS NOT NULL 
	DROP FUNCTION dbo.AverageVotesUdf
GO

CREATE FUNCTION AverageVotesUdf (@bookId uniqueidentifier)
RETURNS float
AS
BEGIN
DECLARE @result AS float
SELECT @result = AVG(CAST([NumStars] AS float)) 
     FROM dbo.Review AS r
     WHERE @bookId = r.BookId
RETURN @result
END
GO
