CREATE PROCEDURE [dbo].[CleanseDb]
AS
	DELETE FROM MeasurementData;	
	DELETE FROM Measurements;	
	DELETE FROM Operators;
	DELETE FROM Organisations;
	DELETE FROM Patients;