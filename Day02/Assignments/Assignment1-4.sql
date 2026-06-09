--Assignment 1

SELECT

    p.FullName,
	d.Name,
    COUNT(e.EncounterId) AS EncounterCount,

    DENSE_RANK()
    OVER ( 

        ORDER BY COUNT(e.EncounterId) DESC
    ) AS VolumeRank

FROM Provider p

LEFT JOIN Encounter e
    ON e.ProviderId = p.ProviderId
JOIN  Department d
	ON p.DepartmentId = d.DepartmentId
GROUP BY
	p.ProviderId,
    p.FullName,
	d.Name;

--Assignment 2
	
	--adding columns
	ALTER TABLE Insurance
ADD
    ValidFrom DATETIME2
        GENERATED ALWAYS AS ROW START HIDDEN
        CONSTRAINT DF_Insurance_From
        DEFAULT SYSUTCDATETIME(),

    ValidTo DATETIME2
        GENERATED ALWAYS AS ROW END HIDDEN
        CONSTRAINT DF_Insurance_To
        DEFAULT '9999-12-31 23:59:59.9999999',

    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo);

	--enable versioning
	ALTER TABLE Insurance
SET (
    SYSTEM_VERSIONING = ON
    (
        HISTORY_TABLE = dbo.Insurance_History
    )
);
 --updating the row
Update Insurance set PolicyNumber='POL-546795-0' where PatientId=1

SELECT
    PatientId,
    InsuranceId,
	Payer,
	PolicyNumber,
    ValidFrom,
    ValidTo
FROM Insurance
FOR SYSTEM_TIME ALL
WHERE PatientId = 1
ORDER BY ValidFrom;

--Assignment 3
CREATE OR ALTER VIEW vw_BillingClaims AS
SELECT 
    ClaimID,
    Status,
    BilledAmount,
    ISNULL(ReimbursedAmt,0) AS ReimbursedAmount,
    (BilledAmount - ISNULL(ReimbursedAmt,0)) AS OutstandingAmount
FROM Claim

CREATE OR ALTER PROCEDURE usp_MonthlyClaimLossReport
AS
BEGIN
    SELECT 
        Status,
        COUNT(*) AS TotalClaims,
        SUM(BilledAmount) AS TotalBilledAmount,
        SUM(ReimbursedAmount) AS TotalReimbursedAmount,
        SUM(BilledAmount - ReimbursedAmount) AS TotalOutstandingAmount,
        RANK() OVER (ORDER BY SUM(BilledAmount - ReimbursedAmount) DESC) AS LossRank
    FROM vw_BillingClaims
    GROUP BY Status
    ORDER BY LossRank;
END;
EXEC usp_MonthlyClaimLossReport


--Assignment 4
CREATE OR ALTER PROCEDURE usp_analytics
@WithinDays INT = 30,
@Departments INT = 5

AS
BEGIN

SET NOCOUNT ON;
--Total Active Patients
Select Count(*) AS TotalActivePatients
FROM Patient
Where IsActive = 1;
-- Build a patient timeline

WITH OrderedEncounters AS (

SELECT

PatientId,
EncounterId,
AdmitDate,

LAG(DischargeDate)
OVER (
PARTITION BY PatientId
ORDER BY AdmitDate
) AS PreviousDischarge

FROM Encounter

WHERE EncounterType = 'Inpatient'

)

-- Find readmissions

SELECT

PatientId,
EncounterId,
AdmitDate,

DATEDIFF(
DAY,
PreviousDischarge,
AdmitDate
) AS DaysSincePreviousVisit

FROM OrderedEncounters

WHERE PreviousDischarge IS NOT NULL

AND DATEDIFF(
DAY,
PreviousDischarge,
AdmitDate
) <= @WithinDays;

select TOP (@Departments) d.Name, Count(e.EncounterId) AS EncounterCount
from Department d 
JOIN Encounter e ON d.DepartmentId = e.DepartmentId
GROUP BY d.Name ORDER BY EncounterCount DESC 

END;
 
EXEC usp_analytics @WithinDays=10, @Departments=5;
