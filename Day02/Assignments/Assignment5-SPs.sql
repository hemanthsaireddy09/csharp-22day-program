--30 Day readmissions
CREATE OR ALTER PROCEDURE _30_DAY_READMISSIONS
@WithinDays INT = 30
AS
BEGIN
SET NOCOUNT ON;
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
) <= @WithinDays and DATEDIFF(
DAY,
PreviousDischarge,
AdmitDate
) >= 0
END;
EXEC _30_DAY_READMISSIONS @WithinDays=30

--High Risk Patients
CREATE OR ALTER PROCEDURE high_risk_patients
AS  
BEGIN
SET NOCOUNT ON
SELECT 
    p.PatientId,
    p.MRN,
    p.FullName,
    p.DateOfBirth,
    DATEDIFF(YEAR, p.DateOfBirth, GETDATE()) AS Age,
    p.Gender,
    p.City
FROM Patient p
WHERE DATEDIFF(YEAR, p.DateOfBirth, GETDATE()) >= 80 and IsActive = 1;
END;

EXEC high_risk_patients

--Provider Workload
CREATE OR ALTER PROCEDURE provider_workload
AS
BEGIN
SET NOCOUNT ON;
SELECT

    p.FullName,

    COUNT(e.EncounterId) AS EncounterCount,

    RANK()
    OVER (
        ORDER BY COUNT(e.EncounterId) DESC
    ) AS VolumeRank

FROM Provider p

LEFT JOIN Encounter e
    ON e.ProviderId = p.ProviderId

GROUP BY
    p.ProviderId,
    p.FullName;
END;

EXEC provider_workload;

--Revenue Analysis
CREATE OR ALTER PROCEDURE revenue_analysis
AS
BEGIN
    SET NOCOUNT ON;

    -- Overall revenue summary
    SELECT 
        SUM(c.BilledAmount) AS TotalBilled,
        SUM(ISNULL(c.ReimbursedAmt,0)) AS TotalReimbursed,
        SUM(c.BilledAmount) - SUM(ISNULL(c.ReimbursedAmt,0)) AS RevenueLeakage,
        COUNT(*) AS TotalClaims
    FROM Claim c;

    -- Revenue by Department
    SELECT 
        d.Name,
        SUM(c.BilledAmount) AS DeptBilled,
        SUM(ISNULL(c.ReimbursedAmt,0)) AS DeptReimbursed,
        SUM(c.BilledAmount) - SUM(ISNULL(c.ReimbursedAmt,0)) AS DeptLeakage,
        COUNT(*) AS ClaimCount
    FROM Claim c
    INNER JOIN Encounter e ON c.EncounterId = e.EncounterId
    INNER JOIN Department d ON e.DepartmentId = d.DepartmentId
    GROUP BY d.Name
    ORDER BY DeptLeakage DESC;

    -- Revenue by Insurance Payer
    SELECT 
        i.Payer,
        SUM(c.BilledAmount) AS PayerBilled,
        SUM(ISNULL(c.ReimbursedAmt,0)) AS PayerReimbursed,
        SUM(c.BilledAmount) - SUM(ISNULL(c.ReimbursedAmt,0)) AS PayerLeakage,
        COUNT(*) AS ClaimCount
    FROM Claim c
    INNER JOIN Insurance i ON c.InsuranceId = i.InsuranceId
    GROUP BY i.Payer
    ORDER BY PayerLeakage DESC;
END;

exec revenue_analysis